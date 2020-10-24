using BassClefStudio.NeuralNet.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    public class Neuron
    {
        public DoubleStore Activation { get; set; }

        public DoubleStore Bias { get; set; }

        public Synapse[] Synapses { get; set; }

        public Neuron(double activation)
        {
            Activation = (DoubleStore)activation;
        }
    }
}
