using BassClefStudio.NeuralNet.Core.Learning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Tests
{
    [TestClass]
    public class NodeSetTests
    {
        [TestMethod]
        public void IterateSampleSet()
        {
            var set = new NodeSet(new Node[]
            {
                new Node(new double[]{ 0,0 }, new double[]{ 1,0 }),
                new Node(new double[]{ 1,0 }, new double[]{ 1,0 }),
                new Node(new double[]{ 0,1 }, new double[]{ 1,0 }),
                new Node(new double[]{ 1,1 }, new double[]{ 0,1 })
            });

            Console.WriteLine(string.Join("\r\n", set.ShuffledData.Select(d => string.Join(",", d.Input))));

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("---");
                Console.WriteLine(string.Join("\r\n", set.GetData().Take(3).Select(d => string.Join(",", d.Input))));
            }

            Console.WriteLine("---");
            Console.WriteLine(string.Join("\r\n", set.GetData().Take(6).Select(d => string.Join(",", d.Input))));

        }
    }
}
