using System;
using System.Collections.Generic;
using System.Text;

namespace Decklan.ML.Core.Network
{
    /// <summary>
    /// Represents a weighted connection between two <see cref="Neuron"/>s.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Represents the weight of this <see cref="Connection"/> (how strongly the input affects the output).
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Represents the input <see cref="Neuron"/> that influences another <see cref="Neuron"/> via this <see cref="Connection"/>.
        /// </summary>
        public Neuron InputNeuron { get; }

        /// <summary>
        /// Creates a new <see cref="Connection"/> from the given input <see cref="Neuron"/>.
        /// </summary>
        /// <param name="input">The input <see cref="Neuron"/> in this <see cref="Connection"/>.</param>
        public Connection(Neuron input)
        {
            InputNeuron = input;
        }

        /// <summary>
        /// Calculates the output value to pass to the output <see cref="Neuron"/> using the current <see cref="Weight"/> and input <see cref="Neuron"/>.
        /// </summary>
        /// <returns></returns>
        public double GetOutput()
        {
            return InputNeuron.Value * Weight;
        }
    }
}
