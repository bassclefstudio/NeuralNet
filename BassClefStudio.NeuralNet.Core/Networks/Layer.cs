using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    /// <summary>
    /// Represents a group of <see cref="Neuron"/>s in a given layer.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Gets the <see cref="Neuron"/> in this <see cref="Layer"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="Neuron"/> in this <see cref="Layer"/>'s <see cref="Neurons"/>.</param>
        public Neuron this[int index]
        { 
            get => Neurons[index];
            set => Neurons[index] = value;
        }

        /// <summary>
        /// The collection of <see cref="Neurons"/> in this <see cref="Layer"/>.
        /// </summary>
        public Neuron[] Neurons { get; set; }

        /// <summary>
        /// Creates a new <see cref="Layer"/> from an array of <see cref="Neuron"/>s.
        /// </summary>
        /// <param name="neurons">The <see cref="Neuron"/>s in this array.</param>
        public Layer(Neuron[] neurons)
        {
            Neurons = neurons;
        }
    }
}
