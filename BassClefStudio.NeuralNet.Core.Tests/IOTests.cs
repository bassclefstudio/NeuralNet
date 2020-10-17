using BassClefStudio.NeuralNet.Core.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                Console.WriteLine(json.ToString());
                var deserialized = service.ReadItem(json);
                Assert.IsTrue(deserialized.Weights[1][0].Length == 3);
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
    }
}
