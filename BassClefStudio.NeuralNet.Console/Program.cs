using BassClefStudio.NeuralNet.Core;
using BassClefStudio.NeuralNet.Core.IO;
using BassClefStudio.NeuralNet.Core.Learning;
using BassClefStudio.NeuralNet.Core.Learning.Backpropagation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;

namespace BassClefStudio.NeuralNet.Client
{
    class Program
    {
        static SampleSet SampleSet;
        static NeuralNetwork NeuralNetwork;

        const double learningThreshold = 0.01;
        const double lowValue = 0.2;
        const double highValue = 0.8;
        const int trialsPerLearn = 200;
        const int maxTrials = 2500000;

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

            double cost = 1;
            while (cost > learningThreshold)
            {
                NeuralNetwork = new NeuralNetwork(new int[] { 3, 6, 6, 2 }, -2, 2);

                ILearningAlgorithm learningAlgorithm = new BackpropagationLearningAlgorithm(0.5);
                cost = 1;
                Console.ForegroundColor = ConsoleColor.Gray;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                int trials = 0;
                while (cost > learningThreshold && trials < maxTrials)
                {
                    cost = learningAlgorithm.Teach(NeuralNetwork, SampleSet, trialsPerLearn);
                    trials += trialsPerLearn;

                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{cost:F4}: ");
                    GetProgress(cost, trials);
                    Console.WriteLine();
                }

                watch.Stop();
                if (trials < maxTrials)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Learning complete! Time elapsed - {watch.Elapsed.TotalSeconds} sec.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Learning failed! Time elapsed - {watch.Elapsed.TotalSeconds} sec. Restarting...");
                    Console.ReadLine();
                    Console.Clear();
                }
                
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
            }

            PrintMap();

            Console.WriteLine();

            EvaluateSamples();

            Console.ReadLine();
        }

        static int last = 0;
        public static void GetProgress(double cost, int trials)
        {
            double percent = 1 - cost + learningThreshold;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{trials} trials, {percent * 100:F1}% ");
            int left = Console.CursorLeft;
            int size = Console.WindowWidth - left;
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
