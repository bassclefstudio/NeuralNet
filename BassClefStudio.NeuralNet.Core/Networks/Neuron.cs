using BassClefStudio.NeuralNet.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    /// <summary>
    /// Represents a single <see cref="Neuron"/> in a <see cref="NeuralNetwork"/> that speicifies how a single feed-forward operation should function.
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// The current activation value of this <see cref="Neuron"/>.
        /// </summary>
        public double Activation { get; set; }

        /// <summary>
        /// The bias of this <see cref="Neuron"/>.
        /// </summary>
        public double Bias { get; set; }

        /// <summary>
        /// An array of <see cref="Synapse"/> values describing how this <see cref="Neuron"/> should be connected to <see cref="Neuron"/>s in the previous layer.
        /// </summary>
        public Synapse[] Synapses { get; set; }

        /// <summary>
        /// Creates a new <see cref="Neuron"/>.
        /// </summary>
        /// <param name="activation">The initial <see cref="Activation"/> value. Defaults to 0.</param>
        public Neuron(double activation = 0)
        {
            Activation = activation;
        }
    }
}
