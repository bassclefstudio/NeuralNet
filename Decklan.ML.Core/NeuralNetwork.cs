using System;
using System.Collections.Generic;
using System.Text;

namespace Decklan.ML.Core
{
    public class NeuralNetwork
    {
        #region Properties

        public int[] Layers { get; private set; }
        public double[][] Neurons { get; private set; }
        public double[][] Biases { get; private set; }
        public double[][][] Weights { get; private set; }

        public double Fitness { get; } = 0;

        #endregion
        #region Initialize

        public NeuralNetwork(int[] layers)
        {
            Layers = layers;

            InitNeurons();
            InitBiases();
            InitWeights();
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
        private void InitBiases()
        {
            var random = new Random();

            List<double[]> biasList = new List<double[]>();
            for (int i = 0; i < Layers.Length; i++)
            {
                double[] bias = new double[Layers[i]];
                for (int j = 0; j < Layers[i]; j++)
                {
                    //bias[j] = random.NextDouble() - 0.5;
                    bias[j] = 0;
                }
                biasList.Add(bias);
            }
            Biases = biasList.ToArray();
        }

        //initializes random array for the weights being held in the network.
        private void InitWeights()
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
                        //neuronWeights[k] = random.NextDouble() - 0.5;
                        neuronWeights[k] = 1;
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            Weights = weightsList.ToArray();
        }

        #endregion
        #region Evaluate

        private double Activate(double value)
        {
            return Math.Tanh(value);
        }

        //feed forward, inputs => outputs.
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
    }
}
