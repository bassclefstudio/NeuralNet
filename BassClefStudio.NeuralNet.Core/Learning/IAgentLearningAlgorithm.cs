using BassClefStudio.NeuralNet.Core.Networks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents a learning model that changes the structure of <see cref="NeuralNetwork"/>s in <see cref="IAgent"/>s that interact with an environment to achieve certain states.
    /// </summary>
    public interface IAgentLearningAlgorithm
    {
        /// <summary>
        /// Uses the <see cref="IAgent"/>s produced by an <see cref="IAgentEnvironment"/> to teach a <see cref="NeuralNetwork"/> with the goal of increasing the <see cref="IAgent.GetFitness"/> of resulting <see cref="IAgent"/>s.
        /// </summary>
        /// <param name="environment">The <see cref="IAgentEnvironment"/> containing the <see cref="IAgent"/>s to train.</param>
        double Teach(IAgentEnvironment environment);
    }
}
