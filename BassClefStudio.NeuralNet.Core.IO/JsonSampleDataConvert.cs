using BassClefStudio.NeuralNet.Core.Learning;
using BassClefStudio.Serialization.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.IO
{
    internal class SampleDataToJsonConverter : IToJsonConverter<Node>
    {
        public bool CanConvert(Node item) => true;

        public JToken Convert(Node item)
        {
            return new JObject(
                new JProperty("Type", "SampleData"),
                new JProperty("Inputs", item.Input),
                new JProperty("Outputs", item.ExpectedOutput));
        }
    }

    internal class SampleDataFromJsonConverter : IFromJsonConverter<Node>
    {
        public bool CanConvert(JToken item) => item.IsJsonType("SampleData");

        public Node Convert(JToken item)
        {
            double[] inputs = item["Inputs"].Select(v => v.Value<double>()).ToArray();
            double[] outputs = item["Outputs"].Select(v => v.Value<double>()).ToArray();
            return new Node(inputs, outputs);
        }
    }
}
