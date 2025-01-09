using Microsoft.Extensions.Time.Testing;
using Moq;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Test.Modules.NoteTaking.GivenExistingNotes;

[TestFixture]
[Parallelizable]
public sealed class WhenGettingById
{
    private FakeTimeProvider _timeProvider;
    private Guid _noteId;
    private Note _result;
    private FakeNoteRepository _noteRepository;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _noteId = Guid.Parse("a83d6396-9a27-4ce9-a488-a3cc8c181ab5");
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);


        await _noteRepository.Add(new Note
        {
            Id = _noteId,
            Collection = null,
            Content = "Test Content",
            Title = "Test Title",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        });
        
        var subject = new NoteService(_noteRepository, _timeProvider);
        _result = await subject.GetNoteById(_noteId);
    }
    
    [Test]
    public void ThenANoteIsReturned()
    {
        Assert.That(_result, Is.Not.Null);
    }

    [Test]
    public void ThenTheExpectedNoteIsReturned()
    {
        Assert.That(_result.Content, Is.EqualTo("Test Content"));
        Assert.That(_result.Title, Is.EqualTo("Test Title"));
        Assert.That(_result.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(_result.IsArchived, Is.False);
        Assert.That(_result.Id, Is.EqualTo(_noteId));
        Assert.That(_result.Collection, Is.Null);
    }
}