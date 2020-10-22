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
    internal class SampleSetToJsonConverter : IToJsonConverter<SampleSet>
    {
        public IToJsonConverter<SampleData> SampleDataConverter { get; set; }

        /// <inheritdoc/>
        public bool CanConvert(SampleSet item) => true;

        /// <inheritdoc/>
        public JToken Convert(SampleSet item)
        {
            return new JObject(
                new JProperty("Type", "SampleSet"),
                new JProperty("IsShuffled", item.IsShuffled),
                new JProperty("Data", item.SetData.Select(d => SampleDataConverter.GetTo(d))));
        }
    }

    internal class SampleSetFromJsonConverter : IFromJsonConverter<SampleSet>
    {
        public IFromJsonConverter<SampleData> SampleDataConverter { get; set; }

        public bool CanConvert(JToken item) => item.IsJsonType("SampleSet");

        public SampleSet Convert(JToken item)
        {
            bool isShuffled = item["IsShuffled"].Value<bool>();
            SampleData[] data = item["Data"].Select(d => SampleDataConverter.GetTo(d)).ToArray();
            return new SampleSet(data, isShuffled);
        }
    }
}
