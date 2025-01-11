using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenGetAllNotes;

[TestFixture]
[Parallelizable]
public sealed class WhenThereAreNoNotesPresent
{
    private FakeTimeProvider _timeProvider;
    private Result<List<NoteDto>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var noteRepository = new FakeNoteRepository();
        var collectionRepository = new FakeCollectionRepository();
        
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        
        var subject = new NoteService(noteRepository,collectionRepository, _timeProvider);
        _result = await subject.GetAllNotes();
    }

    [Test]
    public void ThenAnEmptyListIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.HasError, Is.False);
            Assert.That(_result.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(_result.Content, Is.Empty);
        }));
      
    }
}