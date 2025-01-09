using Microsoft.Extensions.Time.Testing;
using Moq;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Test.Modules.NoteTaking;

[TestFixture]
[Parallelizable]
public sealed class GivenANoteToAdd
{
    private FakeTimeProvider _timeProvider;
    private FakeNoteRepository _noteRepository;
    private Note _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        var subject = new NoteService(_noteRepository, _timeProvider);

        var noteId = Guid.Parse("60d0c577-95aa-4255-b652-a40b3c686716");
        await subject.CreateNote(new Note
        {
            Id = noteId,
            Collection = null,
            Content = "Test Content",
            Title = "Test Note",
            IsArchived = false,
        });
        
        _result = await subject.GetNoteById(noteId);
    }
    
    [Test]
    public void ThenTheNoteIsAddedToTheRepository()
    {
     
        Assert.That(_result.Content, Is.EqualTo("Test Content"));
        Assert.That(_result.Title, Is.EqualTo("Test Note"));
        Assert.That(_result.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.IsArchived, Is.False);
        Assert.That(_result.Id, Is.EqualTo(Guid.Parse("60d0c577-95aa-4255-b652-a40b3c686716")));
        Assert.That(_result.Collection, Is.Null);
    }
}