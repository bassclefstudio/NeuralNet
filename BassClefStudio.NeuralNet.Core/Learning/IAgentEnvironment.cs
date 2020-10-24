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
    }
}
