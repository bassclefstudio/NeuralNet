using BassClefStudio.NeuralNet.Core.Networks;
using Medallion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning.Backpropagation
{
    /// <summary>
    /// Represents a service that teaches a <see cref="NeuralNetwork"/> using backpropagation to adjust relevant parameters.
    /// </summary>
    public class BackpropagationLearningAlgorithm : INodeLearningAlgorithm
    {
        /// <summary>
        /// A <see cref="double"/> value indicating how much of a change to the parameters should be made each time the <see cref="Teach(NeuralNetwork, Node)"/> method is called.
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// A <see cref="double"/> value indicating how much random change to the parameters should be made each time the <see cref="Teach(NeuralNetwork, Node)"/> method is called.
        /// </summary>
        public double Randomness { get; set; }

        /// <summary>
        /// Creates a basic instance of the <see cref="BackpropagationLearningAlgorithm"/> from the given learning rate.
        /// </summary>
        /// <param name="learningRate">A <see cref="double"/> value indicating how much of a change to the parameters should be made each time the <see cref="Teach(NeuralNetwork, Node)"/> method is called.</param>
        /// <param name="randomness">A <see cref="double"/> value indicating how much random change to the parameters should be made each time the <see cref="Teach(NeuralNetwork, Node)"/> method is called.</param>
        public BackpropagationLearningAlgorithm(double learningRate = 0.1, double randomness = 0)
        {
            LearningRate = learningRate;
            Randomness = randomness;
        }

        /// <inheritdoc/>
        public double Teach(NeuralNetwork network, Node data)
        {
            double[] output = network.FeedForward(data.Input);
            double cost = data.GetCost(output);

            Debug.WriteLine($"Neural network cost: {cost}");

            double[][] gamma;

            //// Initialize the gamma two-dimensional array from layers and layer sizes.
            List<double[]> gammaList = new List<double[]>();
            for (int i = 0; i < network.Layers.Length; i++)
            {
                gammaList.Add(new double[network.Layers[i].Size]);
            }
            gamma = gammaList.ToArray();

            for (int i = 0; i < output.Length; i++)
            {
                gamma[network.Layers.Length - 1][i] = (output[i] - data.ExpectedOutput[i]) * network.ActivateDer(output[i]);
            }

            int layer = network.Layers.Length - 2;
            //// Calculates w' and b' for the last layer in the network.
            for (int i = 0; i < network.Layers[network.Layers.Length - 1].Size; i++)
            {
                //// Previous: layer
                network.Layers[layer + 1][i].Bias -= gamma[network.Layers.Length - 1][i] * LearningRate;
                for (int j = 0; j < network.Layers[layer].Size; j++)
                {
                    network.Layers[layer][i].Synapses[j].Weight -= gamma[network.Layers.Length - 1][i] * network.Layers[layer][j].Activation * LearningRate;
                    //// Randomness:
                    if (Randomness != 0)
                    {
                        network.Layers[layer][i].Synapses[j].Weight += Rand.Current.NextDouble(-Randomness, Randomness);
                    }
                }
            }

            //// Calculates derivatives for all hidden layers
            for (int i = network.Layers.Length - 2; i > 0; i--)
            {
                layer = i - 1;
                //// Looking at layer after for the outputs of this layer.
                for (int j = 0; j < network.Layers[i].Size; j++)
                {
                    gamma[i][j] = 0;
                    for (int k = 0; k < gamma[i + 1].Length; k++)
                    {
                        gamma[i][j] = gamma[i + 1][k] * network.Layers[i][k].Synapses[j].Weight;
                    }
                    //// Calculate resulting gamma.
                    gamma[i][j] *= network.ActivateDer(network.Neurons[i][j]);
                }

                //// Looking at layer after for the outputs of this layer.
                for (int j = 0; j < network.Layers[i].Size; j++)
                {
                    //// Change biases by -gamma.
                    //network.Biases[layer][j] -= gamma[i][j] * LearningRate;
                    network.Layers[i][j].Bias -= gamma[i][j] * LearningRate;

                    //// Randomness:
                    if (Randomness != 0)
                    {
                        network.Layers[i][j].Bias += Rand.Current.NextDouble(-Randomness, Randomness);
                    }

                    //// Looking at current layer.
                    for (int k = 0; k < network.Layers[layer].Size; k++)
                    {
                        //// Change weights by -gamma.
                        network.Layers[layer][j].Synapses[k].Weight -= gamma[i][j] * network.Layers[i - 1][k].Activation * LearningRate;

                        //// Randomness:
                        if (Randomness != 0)
                        {
                            network.Layers[layer][j].Synapses[k].Weight += Rand.Current.NextDouble(-Randomness, Randomness);
                        }
                    }
                }
            }

            return cost;
        }
    }
}
