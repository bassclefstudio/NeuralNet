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
        public void IterateNodeSet()
        {
            var a = new Node(new double[] { 0, 0 }, new double[] { 1, 0 });
            var b = new Node(new double[] { 1, 0 }, new double[] { 1, 0 });
            var c = new Node(new double[] { 0, 1 }, new double[] { 1, 0 });
            var d = new Node(new double[] { 1, 1 }, new double[] { 0, 1 });
            var set = new NodeSet(new Node[] { a, b, c, d });

            int repeat = 3;
            var data = set.GetData().Take(4 * repeat).ToArray();
            Console.WriteLine(string.Join("\r\n", data.Select(d => string.Join(",", d.Input))));
            Assert.AreEqual(repeat, data.Count(n => n == a), "Unexpected number of items of node A.");
            Assert.AreEqual(repeat, data.Count(n => n == b), "Unexpected number of items of node B.");
            Assert.AreEqual(repeat, data.Count(n => n == c), "Unexpected number of items of node C.");
            Assert.AreEqual(repeat, data.Count(n => n == d), "Unexpected number of items of node D.");
        }
    }
}
