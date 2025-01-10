using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenANoteUpdate;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteToUpdateDoesNotExist
{
    private FakeNoteRepository _noteRepository;
    private FakeTimeProvider _timeProvider;
    private Guid _noteId;
    private Result<NoteRecord> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _noteId = Guid.Parse("a83d6396-9a27-4ce9-a488-a3cc8c181ab5");
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);

        var note = new NoteRecord
        {
            Id = _noteId,
            CollectionRecord = null,
            Content = "Test Content",
            Title = "Updated Note",
            IsArchived = true,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        };
        
        var subject = new NoteService(_noteRepository, _timeProvider);

        _result = await subject.UpdateNote(note);
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