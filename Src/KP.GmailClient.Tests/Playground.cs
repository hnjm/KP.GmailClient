﻿using System.Collections.Generic;
using System.Threading.Tasks;
using KP.GmailClient.Models;
using KP.GmailClient.Services.Extensions;
using KP.GmailClient.Tests.IntegrationTests;
using Xunit;

namespace KP.GmailClient.Tests
{
    public class Playground
    {
        public async Task Play()
        {
            // ----------------------
            // --- PREREQUISITES ---
            // ----------------------

#if DOCS
            1. Create a new project in the Google Developer Console -> https://console.developers.google.com/project
            2. Create a service account for the project -> https://console.developers.google.com/iam-admin/serviceaccounts
            3. Create and download a new key as JSON file.
            4. (only for Google Apps users) Go to the Google Apps Admin console and select the scopes, more on that here https://developers.google.com/identity/protocols/OAuth2ServiceAccount#delegatingauthority
#endif

            // ----------------------
            // --- SETUP ---
            // ----------------------

            string privateKey = SettingsManager.GetPrivateKey();
            string tokenUri = SettingsManager.GetTokenUri();
            string clientEmail = SettingsManager.GetClientEmail();
            string emailAddress = SettingsManager.GetEmailAddress();
            ServiceAccountCredential accountCredential = new ServiceAccountCredential
            {
                PrivateKey = privateKey,
                TokenUri = tokenUri,
                ClientEmail = clientEmail
            };
            var client = new GmailClient(accountCredential, emailAddress, GmailScopes.Readonly | GmailScopes.Compose);

            // ----------------------
            // --- USAGE EXAMPLES ---
            // ----------------------

            // Send a plain text email
            Message sentMessage = await client.Messages.SendAsync("you@gmail.com", "The subject", "Plain text body");

            // Send a HTML email
            sentMessage = await client.Messages.SendAsync("you@gmail.com", "The subject", "<h1>HTML body</h1>", isBodyHtml: true);

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

        }
    }
}