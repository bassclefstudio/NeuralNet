using BassClefStudio.NET.Core;
using BassClefStudio.NeuralNet.Core;
using BassClefStudio.NeuralNet.Core.IO;
using BassClefStudio.NeuralNet.Core.Learning;
using BassClefStudio.NeuralNet.Core.Learning.Backpropagation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BassClefStudio.NeuralNet.Client
{
    class Program
    {
        static NodeSet SampleSet;
        static NeuralNetwork NeuralNetwork;

        const double learningThreshold = 0.01;
        const double lowValue = 0.2;
        const double highValue = 0.8;
        const int trialsPerLearn = 200;
        const int maxTrials = 2500000;

        static object l = new object();
        static async Task Main(string[] args)
        {
            //// Test sample data - counting how many of the given data values were high (1) in binary in the output.
            SampleSet = new NodeSet(new Tuple<Node, int>[]
            {
                new Tuple<Node, int>(new Node(new double[]{ 0,0,0 }, new double[]{ 1,0,0,0,0,0,0,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 0,0,1 }, new double[]{ 0,1,0,0,0,0,0,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 0,1,0 }, new double[]{ 0,0,1,0,0,0,0,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 0,1,1 }, new double[]{ 0,0,0,1,0,0,0,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 1,0,0 }, new double[]{ 0,0,0,0,1,0,0,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 1,0,1 }, new double[]{ 0,0,0,0,0,1,0,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 1,1,0 }, new double[]{ 0,0,0,0,0,0,1,0 }), 1),
                new Tuple<Node, int>(new Node(new double[]{ 1,1,1 }, new double[]{ 0,0,0,0,0,0,0,1 }), 1),
            });

            double cost = 1;
            while (cost > learningThreshold)
            {
                NeuralNetwork = new NeuralNetwork(new int[] { 3, 6, 6, 8 }, -2, 2);
                INodeLearningAlgorithm learningAlgorithm = new BackpropagationLearningAlgorithm(0.5);
                cost = 1;
                int trials = 0;
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (cost > learningThreshold && trials < maxTrials)
                {
                    cost = learningAlgorithm.Teach(NeuralNetwork, SampleSet, trialsPerLearn);
                    WriteProgress(0);
                    trials += trialsPerLearn;
                }

                void WriteProgress(int indexOf)
                {
                    lock (l)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(0, indexOf);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"{cost:F4}: ");
                        GetProgress(cost);
                        Console.WriteLine();
                    }
                }

                if (cost <= learningThreshold)
                {
                    Console.Clear();
                    WriteProgress(0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Learning complete!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Cost - {cost}");
                    Console.WriteLine($"Time elapsed - {stopwatch.Elapsed.TotalSeconds} sec.");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    WriteProgress(0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Learning failed!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Cost - {cost}");
                    Console.WriteLine($"Time elapsed - {stopwatch.Elapsed.TotalSeconds} sec.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"Press [ENTER] to restart.");
                    Console.ReadLine();
                    Console.Clear();
                }

                Console.ForegroundColor = ConsoleColor.Gray;
            }

            PrintMap();
            Console.WriteLine();
            EvaluateSamples();
            Console.ReadLine();
        }

        static int last = 0;
        public static void GetProgress(double cost, int? trials = null)
        {
            double percent = 1 - cost + learningThreshold;
            if (percent > 1) percent = 1;
            if (percent < 0) percent = 0;
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (trials.HasValue)
            {
                Console.Write($"{trials.Value} trials, {percent * 100:F1}% ");
            }
            else
            {
                Console.Write($"{percent * 100:F1}% ");

            }
            int left = Console.CursorLeft;
            int size = Console.BufferWidth - left;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[".PadRight((int)(percent * (size - 1)), '#').PadRight(size - 1, '-') + "]");
            if (last > size)
            {
                Console.Write("".PadRight(last - size, ' '));
            }
            last = size;
        }

        public static void EvaluateSamples()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Tests:");
            foreach (var sample in SampleSet.SetData)
            {
                var output = Evaluate(sample.Input);
                Console.WriteLine($"Cost: {sample.GetCost(output):F4}");
            }
        }

        public static double[] Evaluate(double[] inputs)
        {
            WriteArray(inputs, "F1");
            Console.Write(" => ");
            var output = NeuralNetwork.FeedForward(inputs);
            WriteArray(output, "F3");
            Console.WriteLine();
            return output;
        }

        public static void WriteArray(double[] array, string format)
        {
            Console.Write("[");
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > highValue)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (array[i] < lowValue)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write($"{array[i].ToString(format)}");
                Console.ForegroundColor = ConsoleColor.Gray;
                if (i < array.Length - 1)
                {
                    Console.Write(",");
                }
            }
            Console.Write("]");
        }

        public static void PrintMap()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("JSON:");
            Console.ForegroundColor = ConsoleColor.Gray;
            using (var convert = new JsonNeuralNetworkConverterService())
            {
                Console.WriteLine(convert.WriteItem(NeuralNetwork).ToString(Formatting.None));
            }
        }
    }
}
