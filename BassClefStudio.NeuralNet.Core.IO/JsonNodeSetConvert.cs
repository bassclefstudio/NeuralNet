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
    internal class NodeSetToJsonConverter : IToJsonConverter<NodeSet>
    {
        public IToJsonConverter<Node> NodeConverter { get; set; }

        /// <inheritdoc/>
        public bool CanConvert(NodeSet item) => true;

        /// <inheritdoc/>
        public JToken Convert(NodeSet item)
        {
            return new JObject(
                new JProperty("Type", "NodeSet"),
                new JProperty("IsShuffled", item.IsShuffled),
                new JProperty("Data", item.SetData.Select(d => NodeConverter.GetTo(d))));
        }
    }

    internal class NodeSetFromJsonConverter : IFromJsonConverter<NodeSet>
    {
        public IFromJsonConverter<Node> NodeConverter { get; set; }

        public bool CanConvert(JToken item) => item.IsJsonType("NodeSet");

        public NodeSet Convert(JToken item)
        {
            bool isShuffled = item["IsShuffled"].Value<bool>();
            Node[] data = item["Data"].Select(d => NodeConverter.GetTo(d)).ToArray();
            return new NodeSet(data, isShuffled);
        }
    }
}
