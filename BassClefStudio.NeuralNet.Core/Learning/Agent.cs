using BassClefStudio.NeuralNet.Core.Networks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// </summary>
    public abstract class Agent
    {
        /// <summary>
        /// Represents the current state of the environment as a <see cref="Node"/> object that can be passed as inputs to a <see cref="NeuralNetwork"/>.
        /// </summary>
        public Node Environment { get; private set; }

        /// <summary>
        /// Represents the <see cref="NeuralNetwork"/> that is the 'brain' of the <see cref="Agent"/> and manipulates the environment.
        /// </summary>
        public NeuralNetwork Network { get; }

        /// <summary>
        /// Executes the actions of the <see cref="Agent"/> using the current <see cref="Network"/> to interpret the given <see cref="Environment"/>.
        /// </summary>
        public void Execute()
        {
            Environment = CreateEnvironment(Network.FeedForward(Environment.Input));
        }

        /// <summary>
        /// Updates the <see cref="Environment"/> given the most recent output from the <see cref="Network"/>.
        /// </summary>
        /// <param name="output">An array of <see cref="double"/> outputs from the <see cref="Network"/>.</param>
        protected abstract Node CreateEnvironment(double[] output);
    }
}
