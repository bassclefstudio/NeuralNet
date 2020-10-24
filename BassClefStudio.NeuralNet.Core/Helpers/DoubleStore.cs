using BassClefStudio.NeuralNet.Core.Networks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Helpers
{
    /// <summary>
    /// Represents a reference store for a <see cref="double"/> parameter in a <see cref="NeuralNetwork"/>.
    /// </summary>
    public class DoubleStore : IEquatable<DoubleStore>, IEquatable<double>
    {
        public double Value { get; set; }

        public DoubleStore(double value = 0)
        {
            Value = value;
        }

        public static implicit operator double(DoubleStore store) => store.Value;

        public static explicit operator DoubleStore(double value) => new DoubleStore(value);

        /// <inheritdoc/>
        public bool Equals(DoubleStore other)
        {
            return this == other;
        }

        /// <inheritdoc/>
        public bool Equals(double other)
        {
            return this.Value == other;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is DoubleStore store
                && this == store;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(DoubleStore a, DoubleStore b)
        {
            return a.Value == b.Value;
        }

        /// <inheritdoc/>
        public static bool operator !=(DoubleStore a, DoubleStore b)
        {
            return !(a == b);
        }
    }
}
