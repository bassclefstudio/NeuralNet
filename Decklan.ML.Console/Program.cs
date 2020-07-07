using Decklan.ML.Core;
using Decklan.ML.Core.Learning;
using Decklan.ML.Core.Learning.Backpropagation;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Decklan.ML.Client
{
    class Program
    {
        static SampleData[] SampleData;
        static NeuralNetwork NeuralNetwork;

        static void Main(string[] args)
        {
            SampleData = new SampleData[]
            {
                new SampleData(new double[]{ 0,0,0 }, new double[]{ 0,0,0 }),
                new SampleData(new double[]{ 0,0,0 }, new double[]{ 0,0,0 }),
                new SampleData(new double[]{ 1,1,0 }, new double[]{ 0,1,0 }),
                new SampleData(new double[]{ 1,0,0 }, new double[]{ 1,0,0 }),
                new SampleData(new double[]{ 1,0,1 }, new double[]{ 0,1,0 }),
                new SampleData(new double[]{ 0,1,0 }, new double[]{ 1,0,0 }),
                new SampleData(new double[]{ 0,1,1 }, new double[]{ 0,1,0 }),
                new SampleData(new double[]{ 0,0,1 }, new double[]{ 1,0,0 }),
                new SampleData(new double[]{ 1,1,1 }, new double[]{ 0,0,1 }),
                new SampleData(new double[]{ 1,1,1 }, new double[]{ 0,0,1 })
            };

            NeuralNetwork = new NeuralNetwork(new int[] { 3, 6, 8, 3 });

            ILearningAlgorithm learningAlgorithm = new BackpropagationLearningAlgorithm(1);
            double cost = 1;
            while (cost > 0.01)
            {
                for (int j = 0; j < 100; j++)
                {
                    cost = learningAlgorithm.Teach(NeuralNetwork, SampleData);
                }
                Console.WriteLine(cost);
            }

            Console.WriteLine();

            PrintMap();

            Console.WriteLine();

            EvaluateSamples();

            Console.ReadLine();
        }

        public static void EvaluateSamples()
        {
            foreach (var sample in SampleData)
            {
                var output = Evaluate(sample.Input);
                Console.WriteLine($"Cost: {sample.GetCost(output):F4}");
            }
        }

        public static double[] Evaluate(double[] inputs)
        {
            var output = NeuralNetwork.FeedForward(inputs);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"[{string.Join(",", inputs)}] => [");
            foreach (var o in output)
            {
                if (output.Max() == o)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write($"{o:F3},");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("]");
            //Console.WriteLine($"[{string.Join(",", inputs)}] => [{string.Join(",", output.Select(o => o.ToString("F3")))}]");
            return output;
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
