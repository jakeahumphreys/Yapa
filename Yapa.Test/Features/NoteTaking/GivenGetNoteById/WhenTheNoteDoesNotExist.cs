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
    private Guid _noteId;
    private FakeNoteRepository _noteRepository;
    private Result<NoteDto> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _noteId = Guid.Parse("a83d6396-9a27-4ce9-a488-a3cc8c181ab5");
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        
        var subject = new NoteService(_noteRepository, _timeProvider);
        _result = await subject.GetNoteById(_noteId);
    }

    [Test]
    public void ThenAnErrorIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.HasError, Is.True);
            Assert.That(_result.ErrorMessage, Is.EqualTo("A note with id a83d6396-9a27-4ce9-a488-a3cc8c181ab5 was not found"));
        }));
    }
}