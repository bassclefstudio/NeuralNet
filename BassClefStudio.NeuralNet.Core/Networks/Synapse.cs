using BassClefStudio.NeuralNet.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    public class Synapse
    {
        public DoubleStore Weight { get; set; }

        public Synapse(double weight)
        {
            Weight = (DoubleStore)weight;
        }
    }
}
