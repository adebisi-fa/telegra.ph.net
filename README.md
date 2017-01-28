# Telegra.ph .Net
A .Net wrapper for the [Telegra.ph API](http://telegra.ph/api)

# Installation
```
Install-Package Telegraph.Net
```

# Usage

```c#
var client = new TelegraphClient();
```

## Access-Token based Methods
To use Telegra.ph API methods that require access-token, call the **.GetTokenClient(...)** method of the **TelegraphClient** class, passing the access-token of the context account, viz:

```c#
  ITokenClient tokenClient = client.GetTokenClient("access-token-from-a-save-location");

  // Revoke an access token
  Account account = await tokenClient.RevokeAccessTokenAsync();

  // Get account information
  Account account = await tokenClient.GetAccountInformationAsync(
    AccountFields.ShortName | AccountFields.AuthorUrl | AccountFields.AuthorUrl
  );

  // Edit account information
  Account updatedAccount = await tokenClient.EditAccountInformationAsync(
    "new-short-name", 
    "new-author-name", 
    "new-author-url"
  );
  
  // Create a new Page
  Page newPage = await tokenClient.CreatePageAsync(
    "page-title", 
    content /* NodeElement[] */, 
    returnContent: true
  );
  
  // Edit a page
  Page editedPage = await tokenClient.EditPageAsync(
    "path-of-page-to-edit",
    "new-page-title",
    updateContent /* NodeElement[] */
  );
  
  // Get first 50 pages created by the account with the context access-token
  PageList pageList = await tokenClient.GetPageListAsync(offset: 0, limit: 50);
```

## Non Token-Based Methods
Methods that doesn't require access-token can be called directly on the **TelegraphClient** class, viz

```c#
// Retrieve the total number of views for a page
int views = await client.GetViewsAsync("page-path", year: 2016, month: 12);

// Get a page information
Page page = await client.GetPage("page-path", returnContent: true);

// Create a new Telegra.ph Account
Account newAccount = await client.CreateAccount("Sandbox", "Anonymous", "http://sandbox.net");
```

# Working with NodeElements

According to [Telegraph API page](http://telegra.ph/api#NodeElement): *A **Node** represents a DOM Node. It can be a String which represents a DOM text node or a NodeElement object.*  

To simplify working with this concept, all **Node**s is a **NodeElement** in this library.  That is, a string is a NodeElement.  A text content like "Hello World!" can thus be expressed as:

```c#
NodeElement nodeElement = new NodeElement {
  Tag = "_text",
  Attributes = new Dictionary<string, string> {
    { "value", "Hello World!" }
  }
};
```
or preferably, simply as:
```c#
NodeElement nodeElement = "Hello World!";
```
taking advantage of implicit operator overloading for string<-->NodeElement.

Thus, the following json content:
```json
[
    {
        "tag": "p",
        "children": [
            "Hello, world!",
            {
                "tag": "p",
                "children": ["This is the second line to first paragraph."]
            }
        ]
    }
]
```

could be constructed as:
```c#
var nodes = new List<NodeElement>();
nodes.Add(
  new NodeElement("p", 
      null /* no attribute */, 
      "Hello World!",
      new NodeElement("p", 
          null /* again no attribute */,
          "This is the second line to first paragraph."
      )
  )
);
...
NodeElement[] nodesArray = nodes.ToArray();
```

# Test Coverage

This library is completely covered via Test (all passing)!  
