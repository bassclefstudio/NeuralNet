using Autofac;
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
    public class JsonNeuralNetworkConverterService : ConverterService<NeuralNetwork, JToken, JToken>
    {
        public JsonNeuralNetworkConverterService(params Assembly[] assemblies) : base(assemblies)
        { }

        public override NeuralNetwork ReadItem(JToken input)
        {
            var service = ConverterContainer.Resolve<IFromJsonConverter<NeuralNetwork>>();
            return service.GetTo(input);
        }

        public override JToken WriteItem(NeuralNetwork item)
        {
            var service = ConverterContainer.Resolve<IToJsonConverter<NeuralNetwork>>();
            return service.GetTo(item);
        }
    }
}
