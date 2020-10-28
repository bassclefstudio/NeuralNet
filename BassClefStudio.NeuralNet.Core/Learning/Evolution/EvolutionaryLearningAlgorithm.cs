using BassClefStudio.NET.Core;
using BassClefStudio.NeuralNet.Core.Networks;
using Medallion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BassClefStudio.NeuralNet.Core.Learning.Evolution
{
    /// <summary>
    /// Represents a service that teaches a <see cref="NeuralNetwork"/> controlling the behavior of an <see cref="IAgent"/> by evaluating and comparing its fitness in a given <see cref="IAgentEnvironment"/> to other <see cref="IAgent"/>s and past versions of itself.
    /// </summary>
    public class EvolutionaryLearningAlgorithm : IAgentLearningAlgorithm
    {
        /// <summary>
        /// The number of <see cref="IAgentEnvironment"/>s competing in each execution of <see cref="Teach(IAgentEnvironment)"/>.
        /// </summary>
        public int Environments { get; }

        /// <summary>
        /// The maximum number of times to call <see cref="IAgent.Execute"/> before evaluating <see cref="IAgent.GetFitness"/> and adjusting network parameters.
        /// </summary>
        public int NumberOfExecutes { get; }

        /// <summary>
        /// The upper bound of the random changes ('mutations') to the weights and biases of <see cref="IAgent"/>s' <see cref="NeuralNetwork"/>s.
        /// </summary>
        public double MutationLevel { get; set; }

        /// <summary>
        /// Creates a new <see cref="EvolutionaryLearningAlgorithm"/>.
        /// </summary>
        /// <param name="environments">The number of <see cref="IAgentEnvironment"/>s competing in each execution of <see cref="Teach(IAgentEnvironment)"/>.</param>
        /// <param name="numberOfExecutes">The maximum number of times to call <see cref="IAgent.Execute"/> before evaluating <see cref="IAgent.GetFitness"/> and adjusting network parameters.</param>
        /// <param name="mutationLevel">The upper bound of the random changes ('mutations') to the weights and biases of <see cref="IAgent"/>s' <see cref="NeuralNetwork"/>s.</param>
        public EvolutionaryLearningAlgorithm(int environments, int numberOfExecutes, double mutationLevel)
        {
            Environments = environments;
            NumberOfExecutes = numberOfExecutes;
            MutationLevel = mutationLevel;
        }

        /// <inheritdoc/>
        public double Teach(IAgentEnvironment environment)
        {
            //// Create the specified number of environments.
            IAgentEnvironment[] eList = Enumerable.Range(0, Environments).Select(i => environment.Copy()).ToArray();
            Parallel.ForEach(eList, e =>
            {
                Mutate(e);
                RunEnvironment(e);
            });

            var fitnesses = eList.Select(e => e.Agents.Select(a => a.GetFitness())).Transpose()
                .Select(t => t.ToList()).ToList();

            //// Find the best network for each type of agent in the environment, and swap networks.
            for (int i = 0; i < fitnesses.Count; i++)
            {
                int iMaxIndex = fitnesses[i].IndexOf(fitnesses[i].Max());
                environment.Agents.ElementAt(i).Network =
                    eList[iMaxIndex].Agents.ElementAt(i).Network;
            }

            return fitnesses.Average(f => f.Max());
        }

        private void Mutate(IAgentEnvironment environment)
        {
            foreach (var neuron in environment.Agents.SelectMany(
                agent => agent.Network.Layers.SelectMany(layer => layer.Neurons)))
            {
                neuron.Bias += Rand.Current.NextDouble(-MutationLevel, MutationLevel);
                foreach (var synapse in neuron.Synapses)
                {
                    synapse.Weight += Rand.Current.NextDouble(-MutationLevel, MutationLevel);
                }
            }
        }

        private void RunEnvironment(IAgentEnvironment environment)
        {
            environment.Setup();
            for (int i = 0; i < NumberOfExecutes; i++)
            {
                foreach(var agent in environment.Agents)
                {
                    agent.Execute();
                }
            }
        }
    }
}
