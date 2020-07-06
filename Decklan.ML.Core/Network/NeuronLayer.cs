using System;
using System.Collections.Generic;
using System.Text;

namespace Decklan.ML.Core.Network
{
    public class NeuronLayer
    {
        public double[] Activations { get; }

        public double[] Biases { get; }

        public NeuronLayer(int size)
        {
            Activations = new double[size];
            Biases = new double[size];
        }

        public
    }
}
