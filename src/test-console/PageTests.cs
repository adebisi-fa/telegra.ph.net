using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegraph.Net;
using Telegraph.Net.Models;
using Xunit;

namespace TestConsole
{
    public class PageTests
    {
        private TelegraphClient _client;
        private ITokenClient _tokenClient;

        public PageTests()
        {
            _client = new TelegraphClient();
            _tokenClient = _client.GetTokenClient("b968da509bb76866c35425099bc0989a5ec3b32997d55286c657e6994bbb");
        }

        [Fact]
        public void ShouldBeAbleToCreatePage()
        {
            var page = _tokenClient.CreatePage(
                title: "Sample Page",
                authorName: "Anonymous",
                returnContent: true,
                content: new List<NodeElement> {
                    new NodeElement("p", null, "Hello, world!")
                }.ToArray()
            ).Result;

            Assert.True(page.Path.Contains("Sample-Page-"));
            Assert.True(page.Url.StartsWith("http://telegra.ph/Sample-Page-"));
            Assert.Equal(1, page.Content.Count);
            Assert.Equal(1, page.Content[0].Children.Count);
            Assert.Equal("Hello, world!", page.Content[0].Children[0]);
        }
    }
}
