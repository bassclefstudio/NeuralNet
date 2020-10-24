using Autofac;
using BassClefStudio.NeuralNet.Core.Learning;
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
    /// A <see cref="ConverterService{TItem, TInput, TOutput}"/> for serializing and deserializing neural network <see cref="NodeSet"/>s.
    /// </summary>
    public class JsonSampleSetConverterService : ConverterService<NodeSet, JToken, JToken>
    {
        /// <summary>
        /// Creates a new <see cref="JsonNeuralNetworkConverterService"/>.
        /// </summary>
        public JsonSampleSetConverterService() : base(typeof(JsonSampleSetConverterService).GetTypeInfo().Assembly)
        { }

        /// <inheritdoc/>
        public override NodeSet ReadItem(JToken input)
        {
            var service = ConverterContainer.Resolve<IFromJsonConverter<NodeSet>>();
            return service.GetTo(input);
        }

        /// <inheritdoc/>
        public override JToken WriteItem(NodeSet item)
        {
            var service = ConverterContainer.Resolve<IToJsonConverter<NodeSet>>();
            return service.GetTo(item);
        }
    }
}
