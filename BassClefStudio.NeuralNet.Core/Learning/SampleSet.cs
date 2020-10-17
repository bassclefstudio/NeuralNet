using Medallion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents a collection of <see cref="SampleData"/> objects for learning and testing purposes.
    /// </summary>
    public class SampleSet
    {
        /// <summary>
        /// Represents the shuffled collection of data in the <see cref="SampleSet"/>.
        /// </summary>
        public List<SampleData> SampleData { get; }

        /// <summary>
        /// Creates a new <see cref="SampleSet"/> from a collection of <see cref="Learning.SampleData"/> objects.
        /// </summary>
        /// <param name="data">A collection of <see cref="Learning.SampleData"/> objects to add to the <see cref="SampleSet"/>.</param>
        /// <param name="shuffle">A <see cref="bool"/> value indicating whether the data in <see cref="SampleData"/> should be shuffled. Defaults to true.</param>
        public SampleSet(IEnumerable<SampleData> data, bool shuffle = true)
        {
            SampleData = new List<SampleData>(data);
            if(shuffle)
            {
                SampleData.Shuffle();
            }    
        }

        /// <summary>
        /// Creates a new <see cref="SampleSet"/> from a collection of <see cref="Learning.SampleData"/> objects and related <see cref="int"/> values indicating how common that data should be in the <see cref="SampleSet"/>.
        /// </summary>
        /// <param name="data">A collection of <see cref="Tuple{SampleData, int}"/> values containing the <see cref="Learning.SampleData"/> to add and an <see cref="int"/> indicating how many times to include it.</param>
        /// <param name="shuffle">A <see cref="bool"/> value indicating whether the data in <see cref="SampleData"/> should be shuffled. Defaults to true.</param>
        public SampleSet(IEnumerable<Tuple<SampleData, int>> data, bool shuffle = true)
        {
            SampleData = new List<SampleData>();

            foreach (var d in data)
            {
                for (int i = 0; i < d.Item2; i++)
                {
                    SampleData.Add(d.Item1);
                }
            }

            if (shuffle)
            {
                SampleData.Shuffle();
            }
        }

        private int index = 0;

        /// <summary>
        /// Gets a collection of <see cref="Learning.SampleData"/> objects from the <see cref="SampleSet"/>'s <see cref="SampleData"/> of a certain size and continuing from the last time the method was called. If the end of the <see cref="SampleData"/> collection is reached, the method will loop back to the beginning of the collection.
        /// </summary>
        /// <param name="size">The size of the sample to return.</param>
        public IEnumerable<SampleData> GetSample(int size)
        {
            int count = SampleData.Count;
            if (count - index > size)
            {
                var sample = SampleData.Skip(index).Take(size);
                index += size;
                return sample;
            }
            else if(count - index == size)
            {
                var sample = SampleData.Skip(index);
                index = 0;
                return sample;
            }
            else
            {
                int overflow = size - (count - index);
                var part = SampleData.Skip(index);
                index = 0;
                return part.Concat(GetSample(overflow));
            }
        }
    }
}
