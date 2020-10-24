using BassClefStudio.NeuralNet.Core.Helpers;
using Medallion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    /// <summary>
    /// Represents a basic neural network with weights, biases, and layers that can evaluate an array of <see cref="double"/> inputs and return an array of <see cref="double"/> outputs (see <seealso cref="FeedForward(double[])"/>).
    /// </summary>
    public class NeuralNetwork
    {
        #region Properties

        /// <summary>
        /// An array of layer sizes, indicating the number of neurons per layer.
        /// </summary>
        public int[] Layers { get; private set; }
        
        /// <summary>
        /// A two-dimensional array of neuron activation values, with indexes [LayerId, NeuronId].
        /// </summary>
        public double[][] Neurons { get; private set; }

        /// <summary>
        /// A two-dimensional array of neuron bias values, with indexes [LayerId, NeuronId].
        /// </summary>
        public double[][] Biases { get; private set; }

        /// <summary>
        /// A three-dimensional array of neuron connection weights, with indexes [LayerId, NeuronId, ConnectionId].
        /// </summary>
        public double[][][] Weights { get; private set; }

        #endregion
        #region Initialize

        /// <summary>
        /// Creates a new, randomized <see cref="NeuralNetwork"/> with the given layer sizes.
        /// </summary>
        /// <param name="layers">An array of <see cref="int"/> sizes for each layer of the <see cref="NeuralNetwork"/>.</param>
        /// <param name="randMin">The minimum random value for a weight or bias.</param>
        /// <param name="randMax">The maximum random value for a weight or bias.</param>
        public NeuralNetwork(int[] layers, double randMin = -1, double randMax = 1)
        {
            Layers = layers;

            InitNeurons();
            InitBiases(randMin, randMax);
            InitWeights(randMin, randMax);
        }

        /// <summary>
        /// Creates a <see cref="NeuralNetwork"/> from the given collection of layers, weights, and biases.
        /// </summary>
        /// <param name="layers">An array of <see cref="int"/> sizes for each layer of the <see cref="NeuralNetwork"/>.</param>
        /// <param name="biases">An array of <see cref="double"/> arrays for each neuron in each layer.</param>
        /// <param name="weights">An array of <see cref="double"/> arrays for each layer except the first with an array for each neuron, with each item representing the weight of the connection between that neuron and the n-th neuron in the previous layer. See <see cref="Weights"/>.</param>
        public NeuralNetwork(int[] layers, double[][] biases, double[][][] weights)
        {
            if(biases.Length != layers.Length || weights.Length != layers.Length - 1)
            {
                throw new NetworkCreationException("The lengths of the weights and biases arrays must equal the number of layers.");
            }
            else
            {
                for (int i = 0; i < biases.Length; i++)
                {
                    if(biases[i].Length != layers[i])
                    {
                        throw new NetworkCreationException($"The length of the biases array for layer {i} must equal the value of Layers[{i}] ({layers[i]})");
                    }
                    
                    if(i > 0)
                    {
                        if (weights[i - 1].Length != layers[i])
                        {
                            throw new NetworkCreationException($"The length of the weights array for layer {i - 1} must equal the value of Layers[{i - 1}] ({layers[i - 1]})");
                        }
                        else
                        {
                            for (int j = 0; j < weights[i - 1].Length; j++)
                            {
                                if (weights[i - 1][j].Length != layers[i - 1])
                                {
                                    throw new NetworkCreationException($"The length of each array in the weights for layer {i} must equal the value of Layers[{i - 1}] ({layers[i - 1]})");
                                }
                            }
                        }
                    }
                }
            }

            Layers = layers;

            InitNeurons();
            Biases = biases;
            Weights = weights;
        }

        //create empty storage array for the neurons in the network.
        private void InitNeurons()
        {
            List<double[]> neuronsList = new List<double[]>();
            for (int i = 0; i < Layers.Length; i++)
            {
                neuronsList.Add(new double[Layers[i]]);
            }
            Neurons = neuronsList.ToArray();
        }

        //initializes and populates array for the biases being held within the network.
        private void InitBiases(double randMin, double randMax)
        {
            var random = new Random();

            List<double[]> biasList = new List<double[]>();
            for (int i = 0; i < Layers.Length; i++)
            {
                double[] bias = new double[Layers[i]];
                for (int j = 0; j < Layers[i]; j++)
                {
                    bias[j] = Rand.Current.NextDouble(randMin, randMax);
                }
                biasList.Add(bias);
            }
            Biases = biasList.ToArray();
        }

        //initializes random array for the weights being held in the network.
        private void InitWeights(double randMin, double randMax)
        {
            var random = new Random();

            List<double[][]> weightsList = new List<double[][]>();
            for (int i = 1; i < Layers.Length; i++)
            {
                List<double[]> layerWeightsList = new List<double[]>();
                int neuronsInPreviousLayer = Layers[i - 1];
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double[] neuronWeights = new double[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = Rand.Current.NextDouble(randMin, randMax);
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            Weights = weightsList.ToArray();
        }

        #endregion
        #region Evaluate

        /// <summary>
        /// Converts a value on the real number line non-linearly to a number between 0 and 1.
        /// </summary>
        /// <param name="value">The input <see cref="double"/>.</param>
        public double Activate(double value)
        {
            double k = (double)Math.Exp(value);
            return k / (1.0f + k);
        }

        /// <summary>
        /// The derivative of the <see cref="Activate(double)"/> function.
        /// </summary>
        /// <param name="value">The input <see cref="double"/>.</param>
        public double ActivateDer(double value)
        {
            return value * (1 - value);
        }

        /// <summary>
        /// Feeds an array of <see cref="double"/> inputs into the <see cref="NeuralNetwork"/> and retuns an array of <see cref="double"/> outputs.
        /// </summary>
        /// <param name="inputs">The collection of <see cref="double"/> values fed into the first layer of the <see cref="NeuralNetwork"/>.</param>
        public double[] FeedForward(double[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                Neurons[0][i] = inputs[i];
            }
            for (int i = 1; i < Layers.Length; i++)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double value = 0f;
                    for (int k = 0; k < Neurons[i - 1].Length; k++)
                    {
                        value += Weights[i - 1][j][k] * Neurons[i - 1][k];
                    }
                    Neurons[i][j] = Activate(value + Biases[i][j]);
                }
            }
            return Neurons[Neurons.Length - 1];
        }

        #endregion
        #region Equality

        /// <inheritdoc/>
        public static bool operator ==(NeuralNetwork a, NeuralNetwork b)
        {
            return a.Layers.SequenceEqual(b.Layers) 
                && ArrayExtensions.AreEqual(a.Biases, b.Biases, (a2, b2) => ArrayExtensions.AreEqual(a2, b2)) 
                && ArrayExtensions.AreEqual(a.Weights, b.Weights, (a2, b2) => ArrayExtensions.AreEqual(a2, b2, (a3, b3) => ArrayExtensions.AreEqual(a3, b3)));
        }

        /// <inheritdoc/>
        public static bool operator !=(NeuralNetwork a, NeuralNetwork b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is NeuralNetwork net
                && this == net;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// An <see cref="Exception"/> thrown if the initialization of a <see cref="NeuralNetwork"/> fails.
    /// </summary>
    public class NetworkCreationException : Exception
    {
        /// <inheritdoc/>
        public NetworkCreationException() { }
        /// <inheritdoc/>
        public NetworkCreationException(string message) : base(message) { }
        /// <inheritdoc/>
        public NetworkCreationException(string message, Exception inner) : base(message, inner) { }
    }
}
