using Medallion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents a collection of <see cref="ShuffledData"/> objects for learning and testing purposes.
    /// </summary>
    public class SampleSet
    {
        /// <summary>
        /// Represents the shuffled collection of <see cref="SetData"/> in the <see cref="SampleSet"/>.
        /// </summary>
        public IEnumerable<SampleData> ShuffledData { get; protected set; }

        private IEnumerable<SampleData> setData;
        /// <summary>
        /// The collection of <see cref="SampleData"/> in this <see cref="SampleSet"/>.
        /// </summary>
        public IEnumerable<SampleData> SetData { get => setData; protected set { setData = value; ShuffledData = value.Shuffled(); } }

        /// <summary>
        /// A <see cref="bool"/> value indicating whether 
        /// </summary>
        public bool IsShuffled { get; }

        /// <summary>
        /// Creates a new <see cref="SampleSet"/> from a collection of <see cref="Learning.SampleData"/> objects.
        /// </summary>
        /// <param name="data">A collection of <see cref="Learning.SampleData"/> objects to add to the <see cref="SampleSet"/>.</param>
        /// <param name="shuffle">A <see cref="bool"/> value indicating whether the data in <see cref="ShuffledData"/> should be shuffled. Defaults to true.</param>
        public SampleSet(IEnumerable<SampleData> data, bool shuffle = true)
        {
            IsShuffled = shuffle;
            SetData = data;
        }

        /// <summary>
        /// Creates a new <see cref="SampleSet"/> from a collection of <see cref="Learning.SampleData"/> objects and related <see cref="int"/> values indicating how common that data should be in the <see cref="SampleSet"/>.
        /// </summary>
        /// <param name="data">A collection of <see cref="Tuple{T1, T2}"/> values containing the <see cref="Learning.SampleData"/> to add and an <see cref="int"/> indicating how many times to include it.</param>
        /// <param name="shuffle">A <see cref="bool"/> value indicating whether the data in <see cref="ShuffledData"/> should be shuffled. Defaults to true.</param>
        public SampleSet(IEnumerable<Tuple<SampleData, int>> data, bool shuffle = true)
        {
            IsShuffled = shuffle;
            var dataList = new List<SampleData>();
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

        /// <summary>
        /// Gets a collection of <see cref="Learning.SampleData"/> objects from the <see cref="SampleSet"/>'s <see cref="ShuffledData"/> of a certain size and continuing from the last time the method was called. If the end of the <see cref="ShuffledData"/> collection is reached, the method will loop back to the beginning of the collection.
        /// </summary>
        /// <param name="size">The size of the sample to return.</param>
        public IEnumerable<SampleData> GetSample(int size)
        {
            int count = ShuffledData.Count();
            if (count - index > size)
            {
                var sample = ShuffledData.Skip(index).Take(size);
                index += size;
                return sample;
            }
            else if(count - index == size)
            {
                var sample = ShuffledData.Skip(index);
                index = 0;
                return sample;
            }
            else
            {
                int overflow = size - (count - index);
                var part = ShuffledData.Skip(index);
                index = 0;
                return part.Concat(GetSample(overflow));
            }
        }

        /// <inheritdoc/>
        public static bool operator ==(SampleSet a, SampleSet b)
        {
            return a.SetData.SequenceEqual(b.SetData);
        }

        /// <inheritdoc/>
        public static bool operator !=(SampleSet a, SampleSet b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SampleSet set
                && this == set;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
