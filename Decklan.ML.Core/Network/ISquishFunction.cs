using System;
using System.Collections.Generic;
using System.Text;

namespace Decklan.ML.Core.Network
{
    /// <summary>
    /// Represents a service which can compress the real number line into a value between 0 and 1 for setting a <see cref="Neuron"/>.
    /// </summary>
    public interface ISquishFunction
    {
        /// <summary>
        /// Compresses the real number line into a value between 0 and 1 for setting a <see cref="Neuron"/>.
        /// </summary>
        /// <param name="value">The input value.</param>
        double Squish(double value);
    }
}
