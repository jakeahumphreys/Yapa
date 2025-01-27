﻿using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenANoteUpdate;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteToUpdateDoesNotExist
{
    private FakeTimeProvider _timeProvider;
    private int _noteId;
    private Result<NoteDto> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var noteRepository = new FakeNoteRepository();
        var collectionRepository = new FakeCollectionRepository();
        _noteId = 1;
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);

        var note = new NoteDto
        {
            Id = _noteId,
            Content = "Test Content",
            Title = "Updated Note",
            IsArchived = true,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        };
        
        var subject = new NoteService(noteRepository, collectionRepository, _timeProvider);

        _result = await subject.UpdateNote(note);
    }

    [Test]
    public void ThenAnErrorIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.HasError, Is.True);
            Assert.That(_result.ErrorMessage, Is.EqualTo("A note with id 1 was not found"));
        }));
    }
}