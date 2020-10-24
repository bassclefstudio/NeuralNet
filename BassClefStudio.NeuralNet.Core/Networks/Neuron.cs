using BassClefStudio.NeuralNet.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    public class Neuron
    {
        public double Activation { get; set; }

        public double Bias { get; set; }

        public Synapse[] Synapses { get; set; }

        public Neuron()
        {
        }
    }
}
