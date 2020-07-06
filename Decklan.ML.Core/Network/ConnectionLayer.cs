using System;
using System.Collections.Generic;
using System.Text;

namespace Decklan.ML.Core.Network
{
    public class ConnectionLayer
    {
        public double[,] Weights { get; }

        public ConnectionLayer(int inputSize, int outputSize)
        {
            Weights = new double[outputSize, inputSize];
        }
    }
}
