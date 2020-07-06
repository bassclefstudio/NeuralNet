using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decklan.ML.Core.Network
{
    /// <summary>
    /// Represents a node that can store an activation value between 0 and 1.
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// The value stored in the <see cref="Neuron"/>.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Neuron"/> with an initial <see cref="Value"/>.
        /// </summary>
        /// <param name="value">The value to set. Defaults to 0.</param>
        public Neuron(double value = 0)
        {
            Set(value);
        }

        /// <summary>
        /// Sets the <see cref="Value"/> stored in the neuron to a <see cref="double"/> between 0 and 1.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public void Set(double value)
        {
            if(value <= 1 && value >= 0)
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException("Value in a neuron must be between 0 and 1, inclusive.");
            }
        }
    }

    public class ConnectedNeuron : Neuron
    {
        /// <summary>
        /// Represents the <see cref="Connection"/> between this <see cref="Neuron"/> and the input <see cref="Neuron"/> on the previous layer.
        /// </summary>
        public IEnumerable<Connection> Connections { get; }

        /// <summary>
        /// The <see cref="ISquishFunction"/> service for evaluating <see cref="SetFromConnection"/>.
        /// </summary>
        public ISquishFunction SquishFunction { get; }

        /// <summary>
        /// The bias value of the <see cref="ConnectedNeuron"/>, which affects the value set in <see cref="SetFromConnection"/>.
        /// </summary>
        public double Bias { get; set; }

        /// <summary>
        /// Creates a new <see cref="ConnectedNeuron"/> with an initial <see cref="Value"/> and a collection of <see cref="Connection"/>s.
        /// </summary>
        /// <param name="connections">The <see cref="Connection"/> objects which influence the <see cref="Value"/> of the <see cref="Neuron"/> (see <see cref="SetFromConnection"/>).</param>
        /// <param name="bias">The bias value affects the value set in <see cref="SetFromConnection"/>.</param>
        /// <param name="squishFunction">Function service used for <see cref="SetFromConnection"/>.</param>
        /// <param name="value">The value to set. Defaults to 0.</param>
        public ConnectedNeuron(IEnumerable<Connection> connections, ISquishFunction squishFunction, double bias = 0, double value = 0) : base(value)
        {
            Connections = connections;
            SquishFunction = squishFunction;
            Bias = bias;
        }

        /// <summary>
        /// Sets the value of the <see cref="ConnectedNeuron"/> using the collection <see cref="Connections"/>.
        /// </summary>
        public void SetFromConnection()
        {
            double sum = Connections.Select(c => c.GetOutput()).Sum();
            Set(SquishFunction.Squish(sum) - Bias);
        }
    }
}
