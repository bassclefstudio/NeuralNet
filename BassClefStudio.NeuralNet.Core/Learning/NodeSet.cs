using Medallion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents a collection of <see cref="ShuffledData"/> objects for learning and testing purposes.
    /// </summary>
    public class NodeSet : INodeProvider
    {
        /// <summary>
        /// Represents the shuffled collection of <see cref="SetData"/> in the <see cref="NodeSet"/>.
        /// </summary>
        public Node[] ShuffledData { get; protected set; }

        private IEnumerable<Node> setData;
        /// <summary>
        /// The collection of <see cref="Node"/> in this <see cref="NodeSet"/>.
        /// </summary>
        public IEnumerable<Node> SetData { get => setData; protected set { setData = value; ShuffledData = IsShuffled ? value.Shuffled().ToArray() : value.ToArray(); } }

        /// <summary>
        /// A <see cref="bool"/> value indicating whether 
        /// </summary>
        public bool IsShuffled { get; }

        /// <summary>
        /// Creates a new <see cref="NodeSet"/> from a collection of <see cref="Learning.Node"/> objects.
        /// </summary>
        /// <param name="data">A collection of <see cref="Learning.Node"/> objects to add to the <see cref="NodeSet"/>.</param>
        /// <param name="shuffle">A <see cref="bool"/> value indicating whether the data in <see cref="ShuffledData"/> should be shuffled. Defaults to true.</param>
        public NodeSet(IEnumerable<Node> data, bool shuffle = true)
        {
            IsShuffled = shuffle;
            SetData = data;
        }

        /// <summary>
        /// Creates a new <see cref="NodeSet"/> from a collection of <see cref="Learning.Node"/> objects and related <see cref="int"/> values indicating how common that data should be in the <see cref="NodeSet"/>.
        /// </summary>
        /// <param name="data">A collection of <see cref="Tuple{T1, T2}"/> values containing the <see cref="Learning.Node"/> to add and an <see cref="int"/> indicating how many times to include it.</param>
        /// <param name="shuffle">A <see cref="bool"/> value indicating whether the data in <see cref="ShuffledData"/> should be shuffled. Defaults to true.</param>
        public NodeSet(IEnumerable<Tuple<Node, int>> data, bool shuffle = true)
        {
            IsShuffled = shuffle;
            var dataList = new List<Node>();
            foreach (var d in data)
            {
                for (int i = 0; i < d.Item2; i++)
                {
                    dataList.Add(d.Item1);
                }
            }
            SetData = dataList;
        }

        private int index = 0;
        /// <inheritdoc/>
        public IEnumerable<Node> GetData()
        {
            while (true)
            {
                yield return ShuffledData[index];
                index = (index + 1) % ShuffledData.Length;
            }
        }

        /// <summary>
        /// Creates a copy of this <see cref="NodeSet"/>.
        /// </summary>
        public NodeSet Copy()
        {
            return new NodeSet(SetData, IsShuffled);
        }

        /// <inheritdoc/>
        public static bool operator ==(NodeSet a, NodeSet b)
        {
            return a.SetData.SequenceEqual(b.SetData);
        }

        /// <inheritdoc/>
        public static bool operator !=(NodeSet a, NodeSet b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is NodeSet set
                && this == set;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
