using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenAddNote;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteIsAddedToAnInvalidCollection
{
    private FakeTimeProvider _timeProvider;
    private Result<NoteDto> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var noteRepository = new FakeNoteRepository();
        var collectionRepository = new FakeCollectionRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        var subject = new NoteService(noteRepository, collectionRepository, _timeProvider);

        collectionRepository.Add(new CollectionDto
        {
            Id = 1,
            IsArchived = false,
            Name = "Test Collection",
            Notes = new List<NoteDto>()
        });
        
        _result = await subject.CreateNote(new NoteDto
        {
            Id = 1,
            Content = "Test Content",
            Title = "Test Note",
            IsArchived = false,
            CollectionRecordId = 2
        });
    }
    
    [Test]
    public void ThenTheNoteIsAddedToTheRepository()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result.HasError, Is.True);
            Assert.That(_result.ErrorMessage, Is.EqualTo("A collection with id 2 does not exist"));
       });
    }
}