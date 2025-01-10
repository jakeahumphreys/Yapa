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
    private FakeNoteRepository _noteRepository;
    private FakeTimeProvider _timeProvider;
    private Result<IList<NoteRecord>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        
        var subject = new NoteService(_noteRepository, _timeProvider);
        _result = await subject.GetAllNotes();
    }

    [Test]
    public void ThenAnErrorIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.HasError, Is.True);
            Assert.That(_result.ErrorMessage, Is.EqualTo("No notes found"));
        }));
      
    }
}