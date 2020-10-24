using BassClefStudio.NeuralNet.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    /// <summary>
    /// Represents a connection between a given <see cref="Neuron"/> and a <see cref="Neuron"/> of a previous <see cref="Layer"/>.
    /// </summary>
    public class Synapse
    {
        /// <summary>
        /// The weight of the <see cref="Synapse"/> connection.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Creates a new <see cref="Synapse"/> with the given <see cref="Weight"/>.
        /// </summary>
        /// <param name="weight">The initial <see cref="Weight"/> of the <see cref="Synapse"/>.</param>
        public Synapse(double weight)
        {
            Weight = weight;
        }
    }
}
