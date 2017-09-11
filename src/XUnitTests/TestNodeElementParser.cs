using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegraph.Net.Models;

namespace TestConsole
{

    public class TestData
    {
        public NodeElement[] Content { get; set; }
    }

    public class TestNodeElementParser
    {
        public static void Run()
        {
            // Parse TestData.json into NodeElement[], nodes.Content.
            var nodes = JsonConvert.DeserializeObject<TestData>(
                new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(TestNodeElementParser), "TestData.json")).ReadToEnd()
            );

            // Serialize NodeElement[] to proper request object, ~.ToRequestObject()
            Console.WriteLine(
                JsonConvert.SerializeObject(
                    nodes.Content.ToRequestObjects(),
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                )
            );
        }
    }
}
