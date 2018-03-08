﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using KP.GmailClient.Common;
using KP.GmailClient.Models;
using KP.GmailClient.Services;
using KP.GmailClient.Services.Extensions;
using Xunit;

namespace KP.GmailClient.Tests.IntegrationTests.MessageServiceTests
{
    public class MessageSendTests : IDisposable
    {
        private readonly CleanupHelper<Message, Message> _helper;
        private readonly MessageService _service;
        private readonly GmailProxy _proxy;

        public MessageSendTests()
        {
            _proxy = SettingsManager.GetGmailProxy();
            _service = new MessageService(_proxy);
            _helper = CleanupHelpers.GetMessageServiceCleanupHelper(_service);
        }

        [Fact]
        public async Task CanSend()
        {
            // Arrange
            var labels = new List<string> { Label.Inbox, Label.Sent, Label.Unread };
            string to = SettingsManager.GetEmailAddress();

            // Act
            Message sentMessage = await _service.SendAsync(to, "The subject", "The body");

            // Assert
            _helper.Add(sentMessage);
            sentMessage.LabelIds.Should().BeEquivalentTo(labels);
        }

        public void Dispose()
        {
            _helper?.Cleanup();
            _proxy?.Dispose();
        }
    }
}
