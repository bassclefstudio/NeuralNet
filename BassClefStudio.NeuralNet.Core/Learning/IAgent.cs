using BassClefStudio.NeuralNet.Core.Networks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.Learning
{
    /// <summary>
    /// Represents an object or character controlled by a <see cref="NeuralNetwork"/> that can have <see cref="Node"/> inputs generated based on the current state of an environment.
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// Represents the current state of the environment as a <see cref="Node"/> object that can be passed as inputs to a <see cref="NeuralNetwork"/>.
        /// </summary>
        Node Inputs { get; }

        /// <summary>
        /// Represents the <see cref="NeuralNetwork"/> that is the 'brain' of the <see cref="IAgent"/> and manipulates the environment.
        /// </summary>
        NeuralNetwork Network { get; set; }

        /// <summary>
        /// Executes the actions of the <see cref="IAgent"/> using the current <see cref="Network"/> to interpret the given <see cref="Inputs"/>.
        /// </summary>
        void Execute();

        /// <summary>
        /// Returns a <see cref="double"/> value indicating the current success (or 'fitness') of the <see cref="IAgent"/> based on the current state of the environment.
        /// </summary>
        double GetFitness();
    }

    /// <summary>
    /// An abstract implementation of <see cref="IAgent"/> where <see cref="Inputs"/> are generated using the current state of the environment and the most recent outputs of the <see cref="Network"/>.
    /// </summary>
    public abstract class BaseAgent : IAgent
    {
        /// <inheritdoc/>
        public Node Inputs { get; private set; }

        /// <inheritdoc/>
        public NeuralNetwork Network { get; set; }

        /// <inheritdoc/>
        public void Execute()
        {
            Inputs = CreateInputs(Network.FeedForward(Inputs.Input));
        }

        /// <summary>
        /// Updates the <see cref="Inputs"/> given the most recent output from the <see cref="Network"/>.
        /// </summary>
        /// <param name="output">An array of <see cref="double"/> outputs from the <see cref="Network"/>.</param>
        protected abstract Node CreateInputs(double[] output);

        /// <inheritdoc/>
        public abstract double GetFitness();
    }
}
