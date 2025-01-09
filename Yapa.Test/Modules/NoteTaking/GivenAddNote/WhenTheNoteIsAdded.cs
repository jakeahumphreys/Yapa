﻿using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;
using Yapa.Test.Modules.NoteTaking.Helpers;

namespace Yapa.Test.Modules.NoteTaking.GivenAddNote;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteIsAdded
{
    private FakeTimeProvider _timeProvider;
    private FakeNoteRepository _noteRepository;
    private Result<NoteRecord> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        var subject = new NoteService(_noteRepository, _timeProvider);

        var noteId = Guid.Parse("60d0c577-95aa-4255-b652-a40b3c686716");
        await subject.CreateNote(new NoteRecord
        {
            Id = noteId,
            CollectionRecord = null,
            Content = "Test Content",
            Title = "Test Note",
            IsArchived = false,
        });
        
        _result = await subject.GetNoteById(noteId);
    }
    
    [Test]
    public void ThenTheNoteIsAddedToTheRepository()
    {
     
        Assert.That(_result.Content.Content, Is.EqualTo("Test Content"));
        Assert.That(_result.Content.Title, Is.EqualTo("Test Note"));
        Assert.That(_result.Content.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.Content.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.Content.IsArchived, Is.False);
        Assert.That(_result.Content.Id, Is.EqualTo(Guid.Parse("60d0c577-95aa-4255-b652-a40b3c686716")));
        Assert.That(_result.Content.CollectionRecord, Is.Null);
    }
}