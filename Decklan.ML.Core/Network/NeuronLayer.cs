using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decklan.ML.Core.Network
{
    /// <summary>
    /// Represents a layer of <see cref="Neuron"/> objects.
    /// </summary>
    public class NeuronLayer
    {
        /// <summary>
        /// The collection of child <see cref="Neuron"/>s.
        /// </summary>
        public Neuron[] Neurons { get; }

        /// <summary>
        /// Creates a basic <see cref="NeuronLayer"/> with the given number of <see cref="Neuron"/>.
        /// </summary>
        /// <param name="size">The number of <see cref="Neuron"/>s in the layer.</param>
        public NeuronLayer(int size)
        {
            Neurons = new Neuron[size];
            for (int i = 0; i < size; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        /// <summary>
        /// Creates a <see cref="NeuronLayer"/> with the given number of <see cref="ConnectedNeuron"/>s, each connected to each neuron in another given <see cref="NeuronLayer"/>.
        /// </summary>
        /// <param name="size">The number of <see cref="Neuron"/>s in the layer.</param>
        /// <param name="prior">The prior <see cref="NeuronLayer"/> to connect to.</param>
        /// <param name="squishFunction">The squish function to use for all <see cref="ConnectedNeuron"/>s.</param>
        public NeuronLayer(int size, NeuronLayer prior, ISquishFunction squishFunction)
        {
            IEnumerable<Connection> getConnections()
            {
                return prior.Neurons.Select(n => new Connection(n));
            }

            Neurons = new Neuron[size];
            for (int i = 0; i < size; i++)
            {
                Neurons[i] = new ConnectedNeuron(getConnections(), squishFunction);
            }
        }

        /// <summary>
        /// Creates a basic <see cref="NeuronLayer"/> from a collection of <see cref="Neuron"/>s.
        /// </summary>
        /// <param name="neurons">A collection of <see cref="Neuron"/>s in the layer.</param>
        public NeuronLayer(IEnumerable<Neuron> neurons)
        {
            Neurons = neurons.ToArray();
        }

        /// <summary>
        /// Creates a basic <see cref="NeuronLayer"/> from an array of <see cref="Neuron"/>s.
        /// </summary>
        /// <param name="neurons">An array of <see cref="Neuron"/>s in the layer.</param>
        public NeuronLayer(Neuron[] neurons)
        {
            Neurons = neurons;
        }
    }
}
