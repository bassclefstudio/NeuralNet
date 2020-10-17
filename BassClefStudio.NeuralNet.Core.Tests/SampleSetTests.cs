using BassClefStudio.NeuralNet.Core.Learning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Tests
{
    [TestClass]
    public class SampleSetTests
    {
        [TestMethod]
        public void CreateSampleSet()
        {
            var set = new SampleSet(new SampleData[]
            {
                new SampleData(new double[]{ 0,0 }, new double[]{ 1,0 }),
                new SampleData(new double[]{ 1,0 }, new double[]{ 1,0 }),
                new SampleData(new double[]{ 0,1 }, new double[]{ 1,0 }),
                new SampleData(new double[]{ 1,1 }, new double[]{ 0,1 })
            });

            Console.WriteLine(string.Join("\r\n", set.SampleData.Select(d => string.Join(",", d.Input))));

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("---");
                Console.WriteLine(string.Join("\r\n", set.GetSample(3).Select(d => string.Join(",", d.Input))));
            }

            Console.WriteLine("---");
            Console.WriteLine(string.Join("\r\n", set.GetSample(6).Select(d => string.Join(",", d.Input))));

        }
    }
}
