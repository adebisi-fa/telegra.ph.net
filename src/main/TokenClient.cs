using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegraph.Net.Models;

namespace Telegraph.Net
{
    internal class TokenClient : ITokenClient
    {
        private string _accessToken;
        private readonly TelegraphClient _client;

        public TokenClient(string accessToken, TelegraphClient client)
        {
            _accessToken = accessToken;
            _client = client;
        }

        /// <summary>
        /// Use this method to revoke access_token and generate a new one, for example, if the user would like to reset all connected sessions, or you have reasons to believe the token was compromised. On success, returns an Account object with new access_token and auth_url fields.
        /// </summary>
        /// <returns>Returns an Account object with new access_token and auth_url fields.</returns>
        public async Task<Account> RevokeAccessToken()
        {
            return (await _client.PostAsync<RevokeAccessTokenRequest, Account>(
                "revokeAccessToken",
                new RevokeAccessTokenRequest { AccessToken = _accessToken }
            )).Result;
        }

        /// <summary>
        /// Use this method to get information about a Telegraph account. Returns an Account object on success.
        /// </summary>
        /// <param name="flaggedFields">
        /// List of account fields to return.
        /// This can be specified as a flagged enum, viz: AccountFields.ShortName | AccountFields.AuthorName | AccountFields.PageCount.
        /// </param>
        /// <returns>Returns an Account object on success.</returns>
        public async Task<Account> GetAccountInformation(AccountFields flaggedFields = AccountFields.ShortName | AccountFields.AuthorName | AccountFields.AuthorUrl)
        {
            Func<AccountFields, AccountFields, bool> isIncluded =
                (combinedFields, fieldToTest) => (combinedFields & fieldToTest) == fieldToTest;

            List<string> fields = new List<string>();
            var dict = new Dictionary<AccountFields, string>() {
                {AccountFields.AuthorName, "author_name"},
                {AccountFields.AuthorUrl, "author_url"},
                {AccountFields.AuthorizationUrl, "auth_url"},
                {AccountFields.PageCount, "page_count"},
                {AccountFields.ShortName, "short_name"}
            };

            foreach (var entry in dict)
                if (isIncluded(flaggedFields, entry.Key))
                    fields.Add(entry.Value);

            return await GetAccountInformationByString(fields.ToArray());
        }

        /// <summary>
        /// Use this method to get information about a Telegraph account. Returns an Account object on success.
        /// </summary>
        /// <param name="fields">
        /// List of account fields to return. Available fields: short_name, author_name, author_url, auth_url, page_count.  
        /// </param>
        /// <returns>Returns an Account object on success.</returns>
        public async Task<Account> GetAccountInformationByString(string[] fields = null) =>
            (await _client.PostAsync<GetAccountInfoRequest, Account>(
                "getAccountInfo",
                new GetAccountInfoRequest() { AccessToken = _accessToken, Fields = fields }
            )).Result;

        /// <summary>
        /// Use this method to update information about a Telegraph account. Pass only the parameters that you want to edit. On success, returns an Account object with the default fields.
        /// </summary>
        /// <param name="shortName">New account name.</param>
        /// <param name="authorName">New default author name used when creating new articles.</param>
        /// <param name="authorUrl">New default profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <returns>An Account object with the default fields.</returns>
        public async Task<Account> EditAccountInformation(string shortName, string authorName, string authorUrl) =>
            (await _client.PostAsync<EditAccountInfoRequest, Account>("editAccountInfo", new EditAccountInfoRequest()
            {
                AccessToken = _accessToken,
                AuthorUrl = authorUrl,
                ShortName = shortName,
                AuthorName = authorName
            })).Result;

        /// <summary>
        /// Use this method to create a new Telegraph page.
        /// </summary>
        /// <param name="title">Access token of the Telegraph account.</param>
        /// <param name="content">Content of the page.</param>
        /// <param name="authorName">Author name, displayed below the article's title.</param>
        /// <param name="authorUrl">Profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <param name="returnContent">If true, a content field will be returned in the Page object.</param>
        /// <returns> On success, returns a Page object.</returns>
        public async Task<Page> CreatePage(string title, NodeElement[] content, string authorName = null,
            string authorUrl = null, bool returnContent = false)
        {
            return (
                await _client.PostAsync<PageRequest, Page>(
                    "createPage",
                    new PageRequest
                    {
                        AccessToken = _accessToken,
                        AuthorName = authorName,
                        AuthorUrl = authorUrl,
                        Content = content.ToRequestObjects(),
                        ReturnContent = returnContent,
                        Title = title
                    }
                )
            ).Result;
        }

        /// <summary>
        /// Use this method to edit an existing Telegraph page.
        /// </summary>
        /// <param name="title">Access token of the Telegraph account.</param>
        /// <param name="content">Content of the page.</param>
        /// <param name="authorName">Author name, displayed below the article's title.</param>
        /// <param name="authorUrl">Profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <param name="returnContent">If true, a content field will be returned in the Page object.</param>
        /// <returns>On success, returns a Page object.</returns>
        public async Task<Page> EditPage(string path, string title, NodeElement[] content, string authorName = null, string authorUrl = null, bool returnContent = false)
        {
            return (
                await _client.PostAsync<PageRequest, Page>(
                    $"editPage/{path}",
                    new PageRequest
                    {
                        AccessToken = _accessToken,
                        AuthorName = authorName,
                        AuthorUrl = authorUrl,
                        Content = content,
                        ReturnContent = returnContent,
                        Title = title
                    }
                )
            ).Result;
        }

        /// <summary>
        /// Use this method to get a list of pages belonging to a Telegraph account.
        /// </summary>
        /// <param name="offset">Sequential number of the first page to be returned. (default = 0)</param>
        /// <param name="limit">Limits the number of pages to be retrieved. (0 - 200, default = 50)</param>
        /// <returns>Returns a PageList object, sorted by most recently created pages first.</returns>
        public async Task<PageList> GetPageList(int offset, int limit)
        {
            return (
                await _client.PostAsync<GetPageListRequest, PageList>(
                    "getPageList",
                    new GetPageListRequest
                    {
                        AccessToken = _accessToken,
                        Limit = limit,
                        Offset = offset
                    }
                )
            ).Result;
        }
    }
}
