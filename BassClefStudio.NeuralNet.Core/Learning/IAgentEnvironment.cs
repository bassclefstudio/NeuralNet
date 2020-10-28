using BassClefStudio.NeuralNet.Core.Networks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents an environment with one or more <see cref="IAgent"/>s that interact with each other and given inputs, used for learning with an <see cref="IAgentLearningAlgorithm"/>.
    /// </summary>
    public interface IAgentEnvironment
    {
        /// <summary>
        /// The <see cref="IAgent"/>s inhabiting the <see cref="IAgentEnvironment"/>.
        /// </summary>
        IEnumerable<IAgent> Agents { get; }

        /// <summary>
        /// Sets up the <see cref="IAgentEnvironment"/> to its initial state, while presrerving the <see cref="NeuralNetwork"/>s that control the behaviors of <see cref="IAgent"/>s in the <see cref="IAgentEnvironment"/>. This allows for learning that can adapt the <see cref="Agents"/>' behavior over multiple runs of the situation.
        /// </summary>
        void Setup();

        /// <summary>
        /// Creates a shallow copy of this <see cref="IAgentEnvironment"/> in its current state.
        /// </summary>
        IAgentEnvironment Copy();
    }
}
