using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    public class Layer
    {
        public Neuron[] Neurons { get; set; }

        public Layer(IEnumerable<Neuron> neurons) : this(neurons.ToArray()) { }
        public Layer(Neuron[] neurons)
        {
            Neurons = neurons;
        }
    }
}
