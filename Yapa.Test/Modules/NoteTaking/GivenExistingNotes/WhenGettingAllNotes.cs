using Microsoft.Extensions.Time.Testing;
using Moq;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;
using Yapa.Test.Modules.NoteTaking.Helpers;

namespace Yapa.Test.Modules.NoteTaking.GivenExistingNotes;

[TestFixture]
[Parallelizable]
public sealed class WhenGettingAllNotes
{
    private FakeNoteRepository _noteRepository;
    private FakeTimeProvider _timeProvider;
    private IList<NoteRecord> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        
       await _noteRepository.Add(new NoteRecord
       {
            Id = Guid.Parse("7e93eba1-8c6b-4534-bbcc-227462a6d3df"),
            CollectionRecord = null,
            Content = "Test Content",
            Title = "Note 1",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
       });

        await _noteRepository.Add(new NoteRecord
        {
            Id = Guid.Parse("15b1f591-bf92-4a8f-bbce-0f336d5b2492"),
            CollectionRecord = null,
            Content = "Test Content",
            Title = "Note 2",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        });
        
        var subject = new NoteService(_noteRepository, _timeProvider);
        _result = await subject.GetAllNotes();
    }
    
    [Test]
    public void ThenTheExpectedNumberOfNotesIsReturned()
    {
        Assert.That(_result, Has.Count.EqualTo(2));
    }

    [Test]
    public void ThenTheFirstNoteIsReturned()
    {
        var note = _result.First();
        
        Assert.That(note.Content, Is.EqualTo("Test Content"));
        Assert.That(note.Title, Is.EqualTo("Note 1"));
        Assert.That(note.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(note.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(note.IsArchived, Is.False);
        Assert.That(note.Id, Is.EqualTo(Guid.Parse("7e93eba1-8c6b-4534-bbcc-227462a6d3df")));
        Assert.That(note.CollectionRecord, Is.Null);
    }
    
    [Test]
    public void ThenTheSecondNoteIsReturned()
    {
        var note = _result.Last();
        
        Assert.That(note.Content, Is.EqualTo("Test Content"));
        Assert.That(note.Title, Is.EqualTo("Note 2"));
        Assert.That(note.CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(note.ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
        Assert.That(note.IsArchived, Is.False);
        Assert.That(note.Id, Is.EqualTo(Guid.Parse("15b1f591-bf92-4a8f-bbce-0f336d5b2492")));
        Assert.That(note.CollectionRecord, Is.Null);
    }
}