﻿using BassClefStudio.NeuralNet.Core.Networks;
using BassClefStudio.Serialization.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassClefStudio.NeuralNet.Core.IO
{
    internal class NeuralNetworkToJsonConverter : IToJsonConverter<NeuralNetwork>
    {
        public bool CanConvert(NeuralNetwork item) => true;

        public JToken Convert(NeuralNetwork item)
        {
            return new JObject(
                new JProperty("Type", "NeuralNetwork"),
                new JProperty("Layers", item.LayerSizes),
                new JProperty("Biases", item.Biases.Select(l => new JArray(l))),
                new JProperty("Weights", item.Weights.Select(l => new JArray(l.Select(n => new JArray(n))))));
        }
    }

    internal class NeuralNetworkFromJsonConverter : IFromJsonConverter<NeuralNetwork>
    {
        public bool CanConvert(JToken json) => json.IsJsonType("NeuralNetwork");

        public NeuralNetwork Convert(JToken json)
        {
            int[] layers = json["Layers"].Select(j => j.Value<int>()).ToArray();
            double[][] biases = json["Biases"].Select(j => j.Select(b => b.Value<double>()).ToArray()).ToArray();
            double[][][] weights = json["Weights"].Select(j => j.Select(l => l.Select(w => w.Value<double>()).ToArray()).ToArray()).ToArray();

            return new NeuralNetwork(layers, biases, weights);
        }
    }
}
