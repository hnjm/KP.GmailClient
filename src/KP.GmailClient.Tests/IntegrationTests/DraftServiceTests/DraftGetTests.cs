﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using KP.GmailClient.Models;
using KP.GmailClient.Services;
using Xunit;

namespace KP.GmailClient.Tests.IntegrationTests.DraftServiceTests
{
    public class DraftGetTests : IDisposable
    {
        private readonly DraftService _service;
        private readonly CleanupHelper<Draft, Draft> _helper;

        public DraftGetTests()
        {
            _service = new DraftService(SettingsManager.GmailProxy);
            _helper = CleanupHelpers.GetDraftServiceCleanupHelper(_service);
        }

        [Fact]
        public async Task CanGet()
        {
            // Arrange
            Draft draft = Samples.DraftSample;
            Draft createdDraft = await _helper.CreateAsync(draft);

            // Act
            Draft getDraft = await _service.GetAsync(createdDraft.Id);

            // Assert
            getDraft.Id.Should().Be(createdDraft.Id);
        }

        public void Dispose()
        {
            _helper?.Cleanup();
        }
    }
}
