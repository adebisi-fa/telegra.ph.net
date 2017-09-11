using Newtonsoft.Json;
using System.Collections.Generic;
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
            var page = _tokenClient.CreatePageAsync(
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

        [Fact]
        public void ShouldBeAbleToGetAPage()
        {
            var page = _tokenClient.CreatePageAsync(
               title: "Sample Page",
               authorName: "Anonymous",
               returnContent: true,
               content: new List<NodeElement> {
                    new NodeElement("p", null, "Hello, world!")
               }.ToArray()
           ).Result;

            var pageRetrieved = _client.GetPageAsync(page.Path).Result;
            Assert.Null(pageRetrieved.Content);

            pageRetrieved = _client.GetPageAsync(page.Path, true).Result;
            Assert.NotNull(pageRetrieved.Content);
        }

        [Fact]
        public void ShouldBeAbleToEditAPage()
        {
            var page = _tokenClient.CreatePageAsync(
                title: "Sample Page",
                authorName: "Anonymous",
                returnContent: true,
                content: new List<NodeElement> {
                    new NodeElement("p", null, "Hello, world!")
                }.ToArray()
            ).Result;

            // Add a nested 'p' tag first paragraph.
            page.Content[0].Children.Add(new NodeElement("p", null, "This is the second line to first paragraph."));


            // Update the page and get the modified copy from the server
            var modifiedPage = _tokenClient.EditPageAsync(
                page.Path,
                page.Title,
                page.Content.ToArray(),
                page.AuthorName,
                page.AuthorUrl,
                returnContent: true
            ).Result;

            dynamic expectedStructure = JsonConvert.DeserializeObject(@"
[
    {
        ""tag"": ""p"",
        ""children"": [
            ""Hello, world!"",
            {
                ""tag"": ""p"",
                ""children"": [""This is the second line to first paragraph.""]
            }
        ]
    }
]");
            Assert.Equal(
                JsonConvert.SerializeObject(expectedStructure),
                JsonConvert.SerializeObject(modifiedPage.Content.ToRequestObjects(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
        }

        [Fact]
        public void ShouldBeAbleToGetPageViews()
        {
            var response = _client.GetViewsAsync("Sample-Page-12-15", 2017, 09).Result;
            Assert.True(response.Views >= 0);
        }

        [Fact]
        public void ShouldBeAbleToGetPageList()
        {
            var response = _tokenClient.GetPageListAsync(0, 40).Result;

            // Was 1227 as at when this code was written.
            // Should be more than now!
            Assert.True(response.TotalCount >= 1227);
            Assert.Equal(40, response.Pages.Count);
        }
    }
}
