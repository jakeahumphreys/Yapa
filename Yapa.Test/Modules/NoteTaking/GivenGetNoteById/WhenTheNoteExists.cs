﻿using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;
using Yapa.Test.Modules.NoteTaking.Helpers;

namespace Yapa.Test.Modules.NoteTaking.GivenGetNoteById;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteExists
{
    private FakeTimeProvider _timeProvider;
    private Guid _noteId;
    private FakeNoteRepository _noteRepository;
    private Result<NoteRecord> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _noteId = Guid.Parse("a83d6396-9a27-4ce9-a488-a3cc8c181ab5");
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);


        await _noteRepository.Add(new NoteRecord
        {
            Id = _noteId,
            CollectionRecord = null,
            Content = "Test Content",
            Title = "Test Title",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        });
        
        var subject = new NoteService(_noteRepository, _timeProvider);
        _result = await subject.GetNoteById(_noteId);
    }
    
    [Test]
    public void ThenANoteIsReturned()
    {
        Assert.That(_result, Is.Not.Null);
    }

    [Test]
    public void ThenTheExpectedNoteIsReturned()
    {
        Assert.That(_result.Content.Content, Is.EqualTo("Test Content"));
        Assert.That(_result.Content.Title, Is.EqualTo("Test Title"));
        Assert.That(_result.Content.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.Content.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.Content.IsArchived, Is.False);
        Assert.That(_result.Content.Id, Is.EqualTo(_noteId));
        Assert.That(_result.Content.CollectionRecord, Is.Null);
    }
}