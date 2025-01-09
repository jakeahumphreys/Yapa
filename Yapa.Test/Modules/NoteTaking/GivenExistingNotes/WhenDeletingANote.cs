using Microsoft.Extensions.Time.Testing;
using Moq;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;
using Yapa.Test.Modules.NoteTaking.Helpers;

namespace Yapa.Test.Modules.NoteTaking.GivenExistingNotes;

[TestFixture]
[Parallelizable]
public sealed class WhenDeletingANote
{
    private FakeNoteRepository _noteRepository;
    private FakeTimeProvider _timeProvider;
    private Guid _noteId;
    private NoteRecord _result;

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
            Title = "Test Title",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        };

        await _noteRepository.Add(note);
        
        var subject = new NoteService(_noteRepository, _timeProvider);

        await _noteRepository.Archive(note);

        _result = await _noteRepository.GetById(note.Id);
    }

    [Test]
    public void ThenTheNoteIsArchived()
    {
        Assert.That(_result.IsArchived, Is.True);
    }
}