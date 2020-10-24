using BassClefStudio.NeuralNet.Core.Learning;
using BassClefStudio.Serialization;
using BassClefStudio.Serialization.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.IO
{
    internal class SampleSetToJsonConverter : IToJsonConverter<NodeSet>
    {
        public IToJsonConverter<Node> SampleDataConverter { get; set; }

        /// <inheritdoc/>
        public bool CanConvert(NodeSet item) => true;

        /// <inheritdoc/>
        public JToken Convert(NodeSet item)
        {
            return new JObject(
                new JProperty("Type", "SampleSet"),
                new JProperty("IsShuffled", item.IsShuffled),
                new JProperty("Data", item.SetData.Select(d => SampleDataConverter.GetTo(d))));
        }
    }

    internal class SampleSetFromJsonConverter : IFromJsonConverter<NodeSet>
    {
        public IFromJsonConverter<Node> SampleDataConverter { get; set; }

        public bool CanConvert(JToken item) => item.IsJsonType("SampleSet");

        public NodeSet Convert(JToken item)
        {
            bool isShuffled = item["IsShuffled"].Value<bool>();
            Node[] data = item["Data"].Select(d => SampleDataConverter.GetTo(d)).ToArray();
            return new NodeSet(data, isShuffled);
        }
    }
}
