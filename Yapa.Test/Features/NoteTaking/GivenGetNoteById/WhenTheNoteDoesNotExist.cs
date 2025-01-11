using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenGetNoteById;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteDoesNotExist
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
        
        var subject = new NoteService(noteRepository,collectionRepository, _timeProvider);
        _result = await subject.GetNoteById(_noteId);
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