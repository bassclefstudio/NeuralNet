using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents a learning model that changes the structure of a <see cref="NeuralNetwork"/> continuously based on <see cref="Node"/> that is passed as inputs.
    /// </summary>
    public interface INodeLearningAlgorithm
    {
        /// <summary>
        /// Passes a <see cref="Node"/> object to a given <see cref="NeuralNetwork"/> and adapts the parameters based on the <see cref="Node.ExpectedOutput"/>. Returns a <see cref="double"/> value indicating the value of the <see cref="Node.GetCost(double[])"/> function of the network before teaching.
        /// </summary>
        /// <param name="network">The <see cref="NeuralNetwork"/> to test and teach.</param>
        /// <param name="data">The <see cref="Node"/> input and expected output to use for learning.</param>
        double Teach(NeuralNetwork network, Node data);
    }

    /// <summary>
    /// Provides extension methods for running <see cref="INodeLearningAlgorithm"/>s over sets of data.
    /// </summary>
    public static class NodeLearningExtensions
    {
        /// <summary>
        /// Passes a collection of <see cref="Node"/> objects to a given <see cref="NeuralNetwork"/> and adapts the parameters based on the <see cref="Node.ExpectedOutput"/>. Returns a <see cref="double"/> value indicating the average value of the <see cref="Node.GetCost(double[])"/> function of the network over each iteration.
        /// </summary>
        /// <param name="algorithm">The given <see cref="INodeLearningAlgorithm"/> used to teach the <see cref="NeuralNetwork"/>.</param>
        /// <param name="network">The <see cref="NeuralNetwork"/> to test and teach.</param>
        /// <param name="data">The <see cref="Node"/> inputs and expected outputs to use for learning.</param>
        public static double Teach(this INodeLearningAlgorithm algorithm, NeuralNetwork network, IEnumerable<Node> data)
        {
            List<double> costs = new List<double>();
            foreach (var d in data)
            {
                costs.Add(
                    algorithm.Teach(network, d));
            }

            return costs.Sum() / costs.Count;
        }

        /// <summary>
        /// Passes a collection of <see cref="Node"/> objects to a given <see cref="NeuralNetwork"/> and adapts the parameters based on the <see cref="Node.ExpectedOutput"/>. Returns a <see cref="double"/> value indicating the average value of the <see cref="Node.GetCost(double[])"/> function of the network over each iteration.
        /// </summary>
        /// <param name="algorithm">The given <see cref="INodeLearningAlgorithm"/> used to teach the <see cref="NeuralNetwork"/>.</param>
        /// <param name="network">The <see cref="NeuralNetwork"/> to test and teach.</param>
        /// <param name="data">The <see cref="NodeSet"/> containing inputs and expected outputs (as <see cref="Node"/>) to use for learning.</param>
        /// <param name="sampleSize">The number of <see cref="Node"/> objects to retrive from the <see cref="NodeSet"/> during this stage of learning.</param>
        public static double Teach(this INodeLearningAlgorithm algorithm, NeuralNetwork network, NodeSet data, int sampleSize)
        {
            return Teach(algorithm, network, data.GetData().Take(sampleSize));
        }
    }
}
