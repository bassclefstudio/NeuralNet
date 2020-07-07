using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Decklan.ML.Core.Learning.Backpropagation
{
    /// <summary>
    /// Represents a service that teaches a <see cref="NeuralNetwork"/> using backpropagation to adjust relevant parameters.
    /// </summary>
    public class BackpropagationLearningAlgorithm : ILearningAlgorithm
    {
        /// <summary>
        /// A <see cref="double"/> value indicating how much of a change to the parameters should be made each time the <see cref="Teach(NeuralNetwork, SampleData)"/> method is called.
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// Creates a basic instance of the <see cref="BackpropagationLearningAlgorithm"/> from the given learning rate.
        /// </summary>
        /// <param name="learningRate">A <see cref="double"/> value indicating how much of a change to the parameters should be made each time the <see cref="Teach(NeuralNetwork, SampleData)"/> method is called.</param>
        public BackpropagationLearningAlgorithm(double learningRate = 0.01)
        {
            LearningRate = learningRate;
        }

        /// <inheritdoc/>
        public double Teach(NeuralNetwork network, SampleData data)
        {
            double[] output = network.FeedForward(data.Input);
            double cost = data.GetCost(output);

            Debug.WriteLine($"Neural network cost: {cost}");

            double[][] gamma;

            //// Initialize the gamma two-dimensional array from layers and layer sizes.
            List<double[]> gammaList = new List<double[]>();
            for (int i = 0; i < network.Layers.Length; i++)
            {
                gammaList.Add(new double[network.Layers[i]]);
            }
            gamma = gammaList.ToArray();

            for (int i = 0; i < output.Length; i++)
            {
                gamma[network.Layers.Length - 1][i] = (output[i] - data.ExpectedOutput[i]) * network.ActivateDer(output[i]);
            }

            int layer = network.Layers.Length - 2;
            //// Calculates w' and b' for the last layer in the network.
            for (int i = 0; i < network.Layers[network.Layers.Length - 1]; i++)
            {
                network.Biases[network.Layers.Length - 2][i] -= gamma[network.Layers.Length - 1][i] * LearningRate;
                for (int j = 0; j < network.Layers[layer]; j++)
                {
                    network.Weights[layer][i][j] -= gamma[network.Layers.Length - 1][i] * network.Neurons[layer][j] * LearningRate;
                }
            }

            //// Calculates derivatives for all hidden layers
            for (int i = network.Layers.Length - 2; i > 0; i--)
            {
                layer = i - 1;
                //// Looking at layer after for the outputs of this layer.
                for (int j = 0; j < network.Layers[i]; j++)
                {
                    gamma[i][j] = 0;
                    for (int k = 0; k < gamma[i + 1].Length; k++)
                    {
                        gamma[i][j] = gamma[i + 1][k] * network.Weights[i][k][j];
                    }
                    //// Calculate resulting gamma.
                    gamma[i][j] *= network.ActivateDer(network.Neurons[i][j]);
                }

                //// Looking at layer after for the outputs of this layer.
                for (int j = 0; j < network.Layers[i]; j++)
                {
                    //// Change biases by -gamma.
                    //network.Biases[layer][j] -= gamma[i][j] * LearningRate;
                    network.Biases[i][j] -= gamma[i][j] * LearningRate;

                    //// Looking at current layer.
                    for (int k = 0; k < network.Layers[layer]; k++)
                    {
                        //// Change weights by -gamma.
                        network.Weights[layer][j][k] -= gamma[i][j] * network.Neurons[i - 1][k] * LearningRate;
                    }
                }
            }

            return cost;
        }
    }
}
