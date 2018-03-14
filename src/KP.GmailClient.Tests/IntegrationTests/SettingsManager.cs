﻿using System;
using System.IO;
using System.Text;
using KP.GmailClient.Common;
using KP.GmailClient.Models;
using Microsoft.Extensions.Configuration;

namespace KP.GmailClient.Tests.IntegrationTests
{
    internal class SettingsManager
    {
        private static readonly IConfigurationRoot ConfigurationRoot;
        private const string SettingsPrefix = "KP_GmailClient_";
        public static GmailProxy GmailProxy { get; }

        static SettingsManager()
        {
            ConfigurationRoot = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", false)
                 .AddJsonFile("appsettings.Private.json", true)
                 .Build();

            GmailProxy = GetGmailProxy();
        }

        public static string GetPrivateKey()
        {
            string base64String = GetSetting($"{SettingsPrefix}PrivateKey");
            byte[] bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string GetTokenUri()
        {
            return GetSetting($"{SettingsPrefix}TokenUri");
        }

        public static string GetClientEmail()
        {
            return GetSetting($"{SettingsPrefix}ClientEmail");
        }

        public static string GetEmailAddress()
        {
            return GetSetting($"{SettingsPrefix}EmailAddress");
        }

        private static GmailProxy GetGmailProxy()
        {
            string privateKey = GetPrivateKey();
            string tokenUri = GetTokenUri();
            string clientEmail = GetClientEmail();
            string emailAddress = GetEmailAddress();
            var accountCredential = new ServiceAccountCredential
            {
                PrivateKey = privateKey,
                TokenUri = tokenUri,
                ClientEmail = clientEmail
            };

            //TODO: get GmailClient.ConvertToScopes using reflection in ReflectionHelper
            string scope = GmailHelper.GetGmailScopesField("ModifyScope");
            return new GmailProxy(new AuthorizationDelegatingHandler(accountCredential, emailAddress, scope));
        }

        private static string GetSetting(string key)
        {
            // Environment variables are used on Travis CI and AppVeyor
            string value = Environment.GetEnvironmentVariable(key) ?? ConfigurationRoot[key];
            if (value == null)
            {
                throw new Exception($"Key '{key}' has not been set in neither the environment variables or config file.");
            }

            return value;
        }
    }
}
