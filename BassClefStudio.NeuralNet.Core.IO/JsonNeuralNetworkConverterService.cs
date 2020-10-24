using Autofac;
using BassClefStudio.NeuralNet.Core.Networks;
using BassClefStudio.Serialization;
using BassClefStudio.Serialization.DI;
using BassClefStudio.Serialization.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.IO
{
    /// <summary>
    /// A <see cref="ConverterService{TItem, TInput, TOutput}"/> for serializing and deserializing <see cref="NeuralNetwork"/> configurations.
    /// </summary>
    public class JsonNeuralNetworkConverterService : ConverterService<NeuralNetwork, JToken, JToken>
    {
        /// <summary>
        /// Creates a new <see cref="JsonNeuralNetworkConverterService"/>.
        /// </summary>
        public JsonNeuralNetworkConverterService() : base(typeof(JsonNeuralNetworkConverterService).GetTypeInfo().Assembly)
        { }

        /// <inheritdoc/>
        public override NeuralNetwork ReadItem(JToken input)
        {
            var service = ConverterContainer.Resolve<IFromJsonConverter<NeuralNetwork>>();
            return service.GetTo(input);
        }

        /// <inheritdoc/>
        public override JToken WriteItem(NeuralNetwork item)
        {
            var service = ConverterContainer.Resolve<IToJsonConverter<NeuralNetwork>>();
            return service.GetTo(item);
        }
    }
}
