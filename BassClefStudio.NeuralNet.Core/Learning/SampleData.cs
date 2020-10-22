using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents an input/expected output pair that can be used for learning and testing purposes.
    /// </summary>
    public class SampleData
    {
        /// <summary>
        /// The <see cref="double"/> array to input in the <see cref="NeuralNetwork"/>.
        /// </summary>
        public double[] Input { get; set; }

        /// <summary>
        /// The expected <see cref="double"/> array from the <see cref="NeuralNetwork"/>.
        /// </summary>
        public double[] ExpectedOutput { get; set; }

        /// <summary>
        /// Creates a new <see cref="SampleData"/> object from an input and expected output.
        /// </summary>
        /// <param name="input">The input to the <see cref="NeuralNetwork"/>.</param>
        /// <param name="expectedOutput">The expected output from the <see cref="NeuralNetwork"/>.</param>
        public SampleData(double[] input, double[] expectedOutput)
        {
            Input = input;
            ExpectedOutput = expectedOutput;
        }

        /// <summary>
        /// Calculates the cost function, a measure of how much the given <paramref name="output"/> differs from the <see cref="ExpectedOutput"/>.
        /// </summary>
        /// <param name="output">The recieved output from the <see cref="NeuralNetwork"/>.</param>
        public double GetCost(double[] output)
        {
            if(output.Length != ExpectedOutput.Length)
            {
                throw new ArgumentException("The given output has a different length to the expected output.");
            }
            else
            {
                double cost = 0;
                for (int i = 0; i < output.Length; i++)
                {
                    cost += Math.Pow(output[i] - ExpectedOutput[i], 2);
                }

                return cost;
            }
        }

        /// <inheritdoc/>
        public static bool operator ==(SampleData a, SampleData b)
        {
            return a.Input.SequenceEqual(b.Input) && a.ExpectedOutput.SequenceEqual(b.ExpectedOutput);
        }

        /// <inheritdoc/>
        public static bool operator !=(SampleData a, SampleData b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SampleData data
                && this == data;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
