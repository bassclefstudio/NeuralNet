using BassClefStudio.NET.Core;
using BassClefStudio.NeuralNet.Core;
using BassClefStudio.NeuralNet.Core.IO;
using BassClefStudio.NeuralNet.Core.Learning;
using BassClefStudio.NeuralNet.Core.Learning.Backpropagation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        static object l = new object();
        static async Task Main(string[] args)
        {
            //// Test sample data - counting how many of the given data values were high (1) in binary in the output.
            SampleSet = new SampleSet(new Tuple<SampleData, int>[]
            {
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,0,0 }, new double[]{ 1,0,0,0,0,0,0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,0,1 }, new double[]{ 0,1,0,0,0,0,0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,1,0 }, new double[]{ 0,0,1,0,0,0,0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 0,1,1 }, new double[]{ 0,0,0,1,0,0,0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,0,0 }, new double[]{ 0,0,0,0,1,0,0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,0,1 }, new double[]{ 0,0,0,0,0,1,0,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,1,0 }, new double[]{ 0,0,0,0,0,0,1,0 }), 1),
                new Tuple<SampleData, int>(new SampleData(new double[]{ 1,1,1 }, new double[]{ 0,0,0,0,0,0,0,1 }), 1),
            });

            double minCost = 1;
            while (minCost > learningThreshold)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                List<Task<Tuple<double, NeuralNetwork>>> tasks = new List<Task<Tuple<double, NeuralNetwork>>>();
                List<Progress<double>> progresses = new List<Progress<double>>();
                List<double> currentProgress = new List<double>();
                for (int i = 0; i < 4; i++)
                {
                    var p = new Progress<double>();
                    progresses.Add(p);
                    currentProgress.Add(1);
                    int index = progresses.Count - 1;
                    tasks.Add(AttemptTeachNeuralNetworkAsync(SampleSet, p));
                    p.ProgressChanged += P_ProgressChanged;
                }

                void P_ProgressChanged(object sender, double e)
                {
                    currentProgress[progresses.IndexOf((Progress<double>)sender)] = e; WriteProgresses();
                }

                void WriteProgresses()
                {
                    lock (l)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        int line = 0;
                        foreach (var p in currentProgress.ToArray())
                        {
                            Console.SetCursorPosition(0, line);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{p:F4}: ");
                            GetProgress(p);
                            line++;
                        }
                        Console.WriteLine();
                    }
                }

                var result = await Task.WhenAny(tasks.ToArray());
                watch.Stop();
                foreach (var p in progresses)
                {
                    p.ProgressChanged -= P_ProgressChanged;
                }
                //var success = result.FirstOrDefault(r => r.Item1 < learningThreshold);
                var success = await result;

                if (success != null)
                {
                    Console.Clear();
                    WriteProgresses();
                    minCost = success.Item1;
                    NeuralNetwork = success.Item2;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Learning complete!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Cost - {success.Item1}");
                    Console.WriteLine($"Time elapsed - {watch.Elapsed.TotalSeconds} sec.");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    WriteProgresses();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Learning failed!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Min Cost - {currentProgress.Min()}");
                    Console.WriteLine($"Time elapsed - {watch.Elapsed.TotalSeconds} sec.");
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

        public static async Task<Tuple<double, NeuralNetwork>> AttemptTeachNeuralNetworkAsync(SampleSet set, IProgress<double> progress)
        {
            return await Task.Run(() => AttemptTeachNeuralNetwork(set, progress));
        }
        private static Tuple<double, NeuralNetwork> AttemptTeachNeuralNetwork(SampleSet set, IProgress<double> progress)
        {
            NeuralNetwork network = new NeuralNetwork(new int[] { 3, 6, 6, 8 }, -2, 2);
            ILearningAlgorithm learningAlgorithm = new BackpropagationLearningAlgorithm(0.5);
            double cost = 1;
            while (cost > learningThreshold)
            {
                cost = learningAlgorithm.Teach(network, set, trialsPerLearn);
                progress.Report(cost);
            }
            return new Tuple<double, NeuralNetwork>(cost, network);
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
