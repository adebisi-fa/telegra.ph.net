using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegraph.Net.Models;

namespace Telegraph.Net
{
    public class TelegraphClient
    {
        private readonly HttpClient _client;

        public TelegraphClient()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _client = new HttpClient { BaseAddress = new Uri("https://api.telegra.ph/") };
        }

        /// <summary>
        /// Use this method to create a new Telegraph account. Most users only need one account, but this can be useful for channel administrators who would like to keep individual author names and profile links for each of their channels. 
        /// </summary>
        /// <param name="shortName">Required. Account name, helps users with several accounts remember which they are currently using. Displayed to the user above the "Edit/Publish" button on Telegra.ph, other users don't see this name.</param>
        /// <param name="authorName">Default author name used when creating new articles.</param>
        /// <param name="authorUrl">Default profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <returns>On success, returns an Account object with the regular fields and an additional access_token field.</returns>
        public async Task<Account> CreateAccountAsync(string shortName, string authorName = null, string authorUrl = null)
        {
            return (
                await PostAsync<CreateAccountRequest, Account>(
                    "createAccount",
                    new CreateAccountRequest() { AuthorName = authorName, ShortName = shortName, AuthorUrl = authorUrl }
                )
            ).Result;
        }

        /// <summary>
        /// Use this method to get a Telegraph page. 
        /// </summary>
        /// <param name="path">Required. Path to the Telegraph page (in the format Title-12-31, i.e. everything that comes after http://telegra.ph/).</param>
        /// <param name="returnContent">If true, content field will be returned in Page object.</param>
        /// <returns>Returns a Page object on success.</returns>
        public async Task<Page> GetPageAsync(string path, bool returnContent = false)
        {
            return (
                await PostAsync<GetPageRequest, Page>(
                    "getPage",
                    new GetPageRequest { Path = path, ReturnContent = returnContent }
                )
            ).Result;
        }


        /// <summary>
        /// Use this method to get the number of views for a Telegraph article. 
        /// </summary>
        /// <param name="path">Required. Path to the Telegraph page (in the format Title-12-31, where 12 is the month and 31 the day the article was first published).</param>
        /// <param name="year">Required if month is passed. If passed, the number of page views for the requested year will be returned.</param>
        /// <param name="month">Required if day is passed. If passed, the number of page views for the requested month will be returned.</param>
        /// <param name="day">Required if hour is passed. If passed, the number of page views for the requested day will be returned.</param>
        /// <param name="hour">If passed, the number of page views for the requested hour will be returned.</param>
        /// <returns>Returns a PageViews object on success. By default, the total number of page views will be returned.</returns>
        public async Task<PageViews> GetViewsAsync(string path, int? year = null, int? month = null, int? day = null, int? hour = null)
        {
            // Do some validations!

            if (hour.HasValue && !day.HasValue)
                throw new InvalidOperationException("Day must be specified.");

            if (day.HasValue && !month.HasValue)
                throw new InvalidOperationException("Month must be specified.");

            if (month.HasValue && !year.HasValue)
                throw new InvalidOperationException("Year must be specified.");

            return (
                await PostAsync<GetViewsRequest, PageViews>(
                    "getViews",
                    new GetViewsRequest
                    {
                        Day = day,
                        Hour = hour,
                        Month = month,
                        Path = path,
                        Year = year
                    }
                )
            ).Result;
        }

        public ITokenClient GetTokenClient(string accessToken) => new TokenClient(accessToken, this);

        internal async Task<TelegraphResponse<TR>> PostAsync<T, TR>(string methodName, T requestObject)
        {
            var requestBody = JsonConvert.SerializeObject(
                requestObject,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                }
            );

            var content = new FormUrlEncodedContent(
                JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody).Select(e => new KeyValuePair<string, string>(e.Key, e.Value.ToString()))
            );

            var responseBody =
                    await (
                        await _client.PostAsync(
                            methodName,
                            content
                        )
                    ).Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<TelegraphResponse<TR>>(responseBody);

            if (!response.Ok)
                throw new TelegraphApiException(response.Error);

            return response;
        }
    }

    public class TelegraphApiException : Exception
    {
        public TelegraphApiException(string message) : base(message) { }
    }
}
