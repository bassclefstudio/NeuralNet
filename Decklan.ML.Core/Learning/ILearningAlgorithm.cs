using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decklan.ML.Core.Learning
{
    /// <summary>
    /// Represents a service that can pass <see cref="SampleData"/> objects to a <see cref="NeuralNetwork"/> and adapt the parameters based on the output.
    /// </summary>
    public interface ILearningAlgorithm
    {
        /// <summary>
        /// Passes a <see cref="SampleData"/> object to a given <see cref="NeuralNetwork"/> and adapts the parameters based on the <see cref="SampleData.ExpectedOutput"/>. Returns a <see cref="double"/> value indicating the value of the <see cref="SampleData.GetCost(double[])"/> function of the network before propagation.
        /// </summary>
        /// <param name="network">The <see cref="NeuralNetwork"/> to test and teach.</param>
        /// <param name="data">The <see cref="SampleData"/> input and expected output to use for learning.</param>
        double Teach(NeuralNetwork network, SampleData data);
    }

    public static class LearningExtensions
    {
        /// <summary>
        /// Passes a collection of <see cref="SampleData"/> objects to a given <see cref="NeuralNetwork"/> and adapts the parameters based on the <see cref="SampleData.ExpectedOutput"/>. Returns a <see cref="double"/> value indicating the average value of the <see cref="SampleData.GetCost(double[])"/> function of the network over each iteration.
        /// </summary>
        /// <param name="network">The <see cref="NeuralNetwork"/> to test and teach.</param>
        /// <param name="data">The <see cref="SampleData"/> inputs and expected outputs to use for learning.</param>
        public static double Teach(this ILearningAlgorithm algorithm, NeuralNetwork network, IEnumerable<SampleData> data)
        {
            List<double> costs = new List<double>();
            foreach (var d in data)
            {
                costs.Add(
                    algorithm.Teach(network, d));
            }

            return costs.Sum() / costs.Count;
        }
    }
}
