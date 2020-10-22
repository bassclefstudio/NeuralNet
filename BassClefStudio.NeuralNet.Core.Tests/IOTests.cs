using BassClefStudio.NeuralNet.Core.IO;
using BassClefStudio.NeuralNet.Core.Learning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace BassClefStudio.NeuralNet.Core.Tests
{
    [TestClass]
    public class IOTests
    {
        [TestMethod]
        public void CreateNetwork()
        {
            var network = new NeuralNetwork(new int[] { 2, 3, 2 });
            using (var service = new JsonNeuralNetworkConverterService())
            {
                var json = service.WriteItem(network);
                Console.WriteLine(json.ToString(Formatting.None));
                Assert.AreEqual(network, service.ReadItem(json), "The serialized network was not deserialized into an equivalent network to the original.");
            }
        }

        [TestMethod]
        public void CreateWrongSize1()
        {
            Assert.ThrowsException<NetworkCreationException>(
                () => new NeuralNetwork(
                    new int[] { 2, 3, 2 },
                    new double[][]
                    {
                        new double[] { 1, 1 },
                        new double[] { 1, 1, 1 },
                        new double[] { 1, 1 }
                    },
                    new double[][][]
                    {
                        new double[][] 
                        {
                            new double[] { 1, 1 },
                            new double[] { 1, 1 },
                            new double[] { 1, 1 }
                        },
                        new double[][]
                        {
                            new double[] { 1, 1, 1 },
                            //// ERROR here!
                            new double[] { 1, 1 }
                        }
                    }));
        }

        [TestMethod]
        public void CreateWrongSize2()
        {
            Assert.ThrowsException<NetworkCreationException>(
                () => new NeuralNetwork(
                    new int[] { 2, 3, 2 },
                    new double[][]
                    {
                        new double[] { 1, 1 },
                        new double[] { 1, 1, 1 },
                        new double[] { 1, 1 }
                    },
                    new double[][][]
                    {
                        new double[][]
                        {
                            new double[] { 1, 1 },
                            new double[] { 1, 1 },
                            new double[] { 1, 1 }
                        },
                        new double[][]
                        {
                            new double[] { 1, 1, 1 },
                            //// ERROR here!
                            //new double[] { 1, 1, 1 }
                        }
                    }));
        }

        [TestMethod]
        public void CreateWrongSize3()
        {
            Assert.ThrowsException<NetworkCreationException>(
                () => new NeuralNetwork(
                    new int[] { 2, 3, 2 },
                    new double[][]
                    {
                        new double[] { 1, 1 },
                        //// ERROR here!
                        new double[] { 1, 1, 1, 1 },
                        new double[] { 1, 1 }
                    },
                    new double[][][]
                    {
                        new double[][]
                        {
                            new double[] { 1, 1 },
                            new double[] { 1, 1 },
                            new double[] { 1, 1 }
                        },
                        new double[][]
                        {
                            new double[] { 1, 1, 1 },
                            new double[] { 1, 1, 1 }
                        }
                    }));
        }

        [TestMethod]
        public void CreateSampleSet()
        {
            SampleSet set = new SampleSet(
                new SampleData[]
                {
                    //// Random data set.
                    new SampleData(new double[]{ 0, 1 }, new double[]{ 1 }),
                    new SampleData(new double[]{ 0.5, 0.75 }, new double[]{ 0.5 }),
                    new SampleData(new double[]{ 10, -14 }, new double[]{ 0.6 }),
                    new SampleData(new double[]{ 4.6, 3 }, new double[]{ 0.7 }),
                    new SampleData(new double[]{ 33.4, 0.1 }, new double[]{ 0 })
                });
            using (var service = new JsonSampleSetConverterService())
            {
                var json = service.WriteItem(set);
                Console.WriteLine(json.ToString(Formatting.None));
                Assert.AreEqual(set, service.ReadItem(json), "The serialized set was not deserialized into an equivalent set to the original.");
            }
        }
    }
}
