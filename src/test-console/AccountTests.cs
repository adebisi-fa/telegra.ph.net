using Telegraph.Net;
using Telegraph.Net.Models;
using Xunit;

namespace TestConsole
{
    public class AccountTests
    {
        TelegraphClient _client;

        public AccountTests()
        {
            _client = new TelegraphClient();
        }

        [Fact]
        public void ShouldBeAbleToCreateNewAccount()
        {
            var result = _client.CreateAccountAsync("Sandbox", "Anonymous").Result;
            Assert.Equal("Anonymous", result.AuthorName);
            Assert.Equal("Sandbox", result.ShortName);
        }

        [Fact]
        public void ShouldBeAbleToGetAccountInformation()
        {
            var tokenClient = _client.GetTokenClient("b968da509bb76866c35425099bc0989a5ec3b32997d55286c657e6994bbb");
            var acctInfo = tokenClient.GetAccountInformation(AccountFields.ShortName | AccountFields.PageCount).Result;
            Assert.Equal("Sandbox",acctInfo.ShortName);
            Assert.True(acctInfo.PageCount > 1211);
            Assert.Null(acctInfo.AuthorName);
        }

        [Fact]
        public void ShouldBeAbleToRevokeAccessToken()
        {
            // Create a new account
            var result = _client.CreateAccountAsync("Sandbox", "Anonymous").Result;

            var tokenClient = _client.GetTokenClient(result.AccessToken);

            // Revoke the access token of the new account
            var acctInfo = tokenClient.RevokeAccessToken().Result;

            // Check for new access_token and new authorization url
            Assert.NotNull(acctInfo.AccessToken);
            Assert.NotNull(acctInfo.AuthorizationUrl);
        }

        [Fact]
        public void ShouldBeAbleToEditAccountInformation()
        {
            // Create a new account
            var result = _client.CreateAccountAsync("Sandbox", "Anonymous").Result;

            var tokenClient = _client.GetTokenClient(result.AccessToken);

            // Update account information
            var acctInfo =
                tokenClient.EditAccountInformation("Sandbox-Edited", "Anonymous-Edited", "http://google.com").Result;

            // Get account information
            var updateAcctInfo = tokenClient.GetAccountInformation().Result;

            // Ensure updated and retrieved account information match
            Assert.Equal(updateAcctInfo.AuthorName, acctInfo.AuthorName);
            Assert.Equal(updateAcctInfo.ShortName, acctInfo.ShortName);
            Assert.Equal(updateAcctInfo.AuthorUrl, acctInfo.AuthorUrl);
        }
    }
}
