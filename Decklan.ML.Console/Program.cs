using Decklan.ML.Core;
using Decklan.ML.Core.Learning;
using Decklan.ML.Core.Learning.Backpropagation;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;

namespace Decklan.ML.Client
{
    class Program
    {
        static SampleSet SampleSet;
        static NeuralNetwork NeuralNetwork;

        const double learningThreshold = 0.01;
        const double lowValue = 0.2;
        const double highValue = 0.8;

        static void Main(string[] args)
        {
            SampleSet = new SampleSet(new Tuple<SampleData, int>[]
            {
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,0,0 }, new double[]{ 0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,0,0 }, new double[]{ 0,1 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,1,0 }, new double[]{ 0,1 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,0,1 }, new double[]{ 0,1 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,1,0 }, new double[]{ 1,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,0,1 }, new double[]{ 1,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,1,1 }, new double[]{ 1,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,1,1 }, new double[]{ 1,1 }), 1)
            });

            NeuralNetwork = new NeuralNetwork(new int[] { 3, 6, 6, 2 });

            ILearningAlgorithm learningAlgorithm = new BackpropagationLearningAlgorithm(0.25);
            double cost = 1;
            Console.ForegroundColor = ConsoleColor.Gray;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (cost > learningThreshold)
            {
                cost = learningAlgorithm.Teach(NeuralNetwork, SampleSet, 200);
                
                Console.SetCursorPosition(0,0);
                Console.Write($"{cost:F4} ");
                GetProgress(cost);
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Learning complete! Time elapsed - {watch.Elapsed.TotalSeconds} sec.");
            watch.Stop();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

            PrintMap();

            Console.WriteLine();

            EvaluateSamples();

            Console.ReadLine();
        }

        public static void GetProgress(double cost)
        {
            double percent = 1 - cost + learningThreshold;
            Console.Write($"({percent * 100:F1}%) [");
            int left = Console.CursorLeft;
            int size = Console.WindowWidth - left - 1;
            Console.Write("".PadRight((int)(percent * size), '#').PadRight(size, '-'));
            Console.Write("]");
        }

        public static void EvaluateSamples()
        {
            foreach (var sample in SampleSet.SampleData)
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
            for (int l = 0; l < NeuralNetwork.Layers.Length; l++)
            {
                Console.Write($"Layer {l}: ");
                for (int i = 0; i < NeuralNetwork.Layers[l]; i++)
                {
                    Console.Write($"B{NeuralNetwork.Biases[l][i]:F2}: ");
                    if (l > 0)
                    {
                        Console.Write($"[{string.Join(",", NeuralNetwork.Weights[l - 1][i].Select(w => w.ToString("F2")))}] ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
