using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Networks
{
    public class Synapse
    {
        public double Weight { get; set; }

        public Synapse(double weight)
        {
            Weight = weight;
        }
    }
}
