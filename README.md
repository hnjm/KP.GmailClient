[![NuGet version (KP.GmailClient)](https://img.shields.io/nuget/v/KP.GmailClient.svg?style=flat-square)](https://www.nuget.org/packages/KP.GmailClient/)
[![Build status](https://ci.appveyor.com/api/projects/status/tqv09fs3fo9a37t0?svg=true)](https://ci.appveyor.com/project/KP/gmail-api)

# KP.GmailClient
This is an alternative client for the auto generated [Google.Apis.Gmail.v1](https://www.nuget.org/packages/Google.Apis.Gmail.v1/) Client Library.

- It's easy to use
- Has added extension methods to make common tasks even more easier
- Supports Async
- Cross platform (.NET Standard library)
- Did I mention easy?

## Prerequisites
1. Create a new project in the [Google Cloud Platform Console](https://console.cloud.google.com/home/dashboard)
2. Enable the Gmail API
3. Create a [service account](https://console.cloud.google.com/iam-admin/serviceaccounts/) for the project
4. Create and download a new key as JSON file
5. (only for G Suite users) Go to the G Suite Admin console and select the scopes ([more on that here](https://developers.google.com/identity/protocols/OAuth2ServiceAccount#delegatingauthority))

## Setup
``` csharp
// Either use from config
const GmailScopes scopes = GmailScopes.Readonly | GmailScopes.Compose;
string privateKey = SettingsManager.GetPrivateKey();
string tokenUri = SettingsManager.GetTokenUri();
string clientEmail = SettingsManager.GetClientEmail();
string emailAddress = SettingsManager.GetEmailAddress();
var accountCredential = new ServiceAccountCredential
{
    PrivateKey = privateKey,
    TokenUri = tokenUri,
    ClientEmail = clientEmail
};
var client = new GmailClient(accountCredential, emailAddress, scopes);

// Or use from downloaded JSON file directly
const string path = "C:\\Users\\Me\\Documents\\Gmail-Project.json";
var initializer = GmailClientInitializer.Initialize(path, scopes);
client = new GmailClient(initializer, emailAddress);
```

## Usage examples
``` csharp
// Send a plain text email
Message sentMessage = await client.Messages.SendAsync(emailAddress, "The subject", "Plain text body");

// Send a HTML email
sentMessage = await client.Messages.SendAsync(emailAddress, "The subject", "<h1>HTML body</h1>", isBodyHtml: true);

// Get the users profile
Profile profile = await client.GetProfileAsync();

// Get inbox messages
IList<Message> messages = await client.Messages.ListAsync();

// Get starred messages
IList<Message> starredMessages = await client.Messages.ListByLabelAsync(Label.Starred);

// List all labels
IList<Label> labels = await client.Labels.ListAsync();

// List all drafts
IList<Draft> drafts = await client.Drafts.ListAsync();
```
