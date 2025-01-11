using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenANoteUpdate;

[TestFixture]
[Parallelizable]
public sealed class WhenTheNoteToUpdateExists
{
    private FakeNoteRepository _noteRepository;
    private FakeTimeProvider _timeProvider;
    private int _noteId;
    private Result<NoteDto> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _noteId = 1;
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);

        var note = new NoteDto
        {
            Id = _noteId,
            Content = "Test Content",
            Title = "Test Title",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        };

        await _noteRepository.Add(note);
        
        var subject = new NoteService(_noteRepository, _timeProvider);

        note.IsArchived = true;
        note.Title = "Updated Note";

        _result = await subject.UpdateNote(note);
    }

    [Test]
    public void ThenTheExpectedNoteIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.Content.Content, Is.EqualTo("Test Content"));
            Assert.That(_result.Content.Title, Is.EqualTo("Updated Note"));
            Assert.That(_result.Content.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.IsArchived, Is.True);
            Assert.That(_result.Content.Id, Is.EqualTo(_noteId));
        }));
    }
}