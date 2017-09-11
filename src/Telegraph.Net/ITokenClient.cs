using System.Threading.Tasks;
using Telegraph.Net.Models;

namespace Telegraph.Net
{
    public interface ITokenClient
    {
        /// <summary>
        /// Use this method to revoke access_token and generate a new one, for example, if the user would like to reset all connected sessions, or you have reasons to believe the token was compromised. On success, returns an Account object with new access_token and auth_url fields.
        /// </summary>
        /// <returns>Returns an Account object with new access_token and auth_url fields.</returns>
        Task<Account> RevokeAccessTokenAsync();

        /// <summary>
        /// Use this method to get information about a Telegraph account. Returns an Account object on success.
        /// </summary>
        /// <param name="flaggedFields">
        /// List of account fields to return.
        /// This can be specified as a flagged enum, viz: AccountFields.ShortName | AccountFields.AuthorName | AccountFields.PageCount.
        /// </param>
        /// <returns>Returns an Account object on success.</returns>
        Task<Account> GetAccountInformationAsync(AccountFields flaggedFields = AccountFields.ShortName | AccountFields.AuthorName | AccountFields.AuthorUrl);

        /// <summary>
        /// Use this method to get information about a Telegraph account. Returns an Account object on success.
        /// </summary>
        /// <param name="fields">
        /// List of account fields to return. Available fields: short_name, author_name, author_url, auth_url, page_count.  
        /// </param>
        /// <returns>Returns an Account object on success.</returns>
        Task<Account> GetAccountInformationByStringAsync(string[] fields = null);

        /// <summary>
        /// Use this method to update information about a Telegraph account. Pass only the parameters that you want to edit. On success, returns an Account object with the default fields.
        /// </summary>
        /// <param name="shortName">New account name.</param>
        /// <param name="authorName">New default author name used when creating new articles.</param>
        /// <param name="authorUrl">New default profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <returns>An Account object with the default fields.</returns>
        Task<Account> EditAccountInformationAsync(string shortName, string authorName, string authorUrl);

        /// <summary>
        /// Use this method to create a new Telegraph page.
        /// </summary>
        /// <param name="title">Access token of the Telegraph account.</param>
        /// <param name="content">Content of the page.</param>
        /// <param name="authorName">Author name, displayed below the article's title.</param>
        /// <param name="authorUrl">Profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <param name="returnContent">If true, a content field will be returned in the Page object.</param>
        /// <returns> On success, returns a Page object.</returns>
        Task<Page> CreatePageAsync(string title, NodeElement[] content, string authorName = null,
            string authorUrl = null, bool returnContent = false);

        /// <summary>
        /// Use this method to edit an existing Telegraph page.
        /// </summary>
        /// <param name="title">Access token of the Telegraph account.</param>
        /// <param name="content">Content of the page.</param>
        /// <param name="authorName">Author name, displayed below the article's title.</param>
        /// <param name="authorUrl">Profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.</param>
        /// <param name="returnContent">If true, a content field will be returned in the Page object.</param>
        /// <returns>On success, returns a Page object.</returns>
        Task<Page> EditPageAsync(string path, string title, NodeElement[] content, string authorName = null, string authorUrl = null, bool returnContent = false);

        /// <summary>
        /// Use this method to get a list of pages belonging to a Telegraph account.
        /// </summary>
        /// <param name="offset">Sequential number of the first page to be returned. (default = 0)</param>
        /// <param name="limit">Limits the number of pages to be retrieved. (0 - 200, default = 50)</param>
        /// <returns>Returns a PageList object, sorted by most recently created pages first.</returns>
        Task<PageList> GetPageListAsync(int offset, int limit);
    }
}