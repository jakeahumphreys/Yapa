using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenAddNote;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteIsAdded
{
    private FakeTimeProvider _timeProvider;
    private Result<List<NoteDto>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var noteRepository = new FakeNoteRepository();
        var collectionRepository = new FakeCollectionRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        var subject = new NoteService(noteRepository, collectionRepository, _timeProvider);

        await subject.CreateNote(new NoteDto
        {
            Id = 1,
            Content = "Test Content",
            Title = "Test Note",
            IsArchived = false,
        });
        
        _result = await subject.GetAllNotes();
    }
    
    [Test]
    public void ThenTheNoteIsAddedToTheRepository()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result.Content, Has.Count.EqualTo(1));
            
            Assert.That(_result.Content.First().Content, Is.EqualTo("Test Content"));
            Assert.That(_result.Content.First().Title, Is.EqualTo("Test Note"));
            Assert.That(_result.Content.First().CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.First().ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.First().IsArchived, Is.False);
            Assert.That(_result.Content.First().Id, Is.Not.EqualTo(0));
       });
    }
}