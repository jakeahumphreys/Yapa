using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenGetAllNotes;

[TestFixture]
[Parallelizable]
public sealed class WhenThereAreNotesPresent
{
    private FakeNoteRepository _noteRepository;
    private FakeTimeProvider _timeProvider;
    private Result<List<NoteDto>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new FakeNoteRepository();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        
       await _noteRepository.Add(new NoteDto
       {
            Id = Guid.Parse("7e93eba1-8c6b-4534-bbcc-227462a6d3df"),
            Content = "Test Content",
            Title = "Note 1",
            IsArchived = false,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
       });

        await _noteRepository.Add(new NoteDto
        {
            Id = Guid.Parse("15b1f591-bf92-4a8f-bbce-0f336d5b2492"),
            Content = "Test Content",
            Title = "Note 2",
            IsArchived = true,
            ModifiedOn = _timeProvider.GetUtcNow().DateTime,
            CreatedOn = _timeProvider.GetUtcNow().DateTime,
        });
        
        var subject = new NoteService(_noteRepository, _timeProvider);
        _result = await subject.GetAllNotes();
    }

    [Test]
    public void ThenTheExpectedNotesAreReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.Content, Has.Count.EqualTo(2));
            
            Assert.That(_result.Content.First().Content, Is.EqualTo("Test Content"));
            Assert.That(_result.Content.First().Title, Is.EqualTo("Note 1"));
            Assert.That(_result.Content.First().CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.First().ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.First().IsArchived, Is.False);
            Assert.That(_result.Content.First().Id, Is.EqualTo(Guid.Parse("7e93eba1-8c6b-4534-bbcc-227462a6d3df")));
            
            Assert.That(_result.Content.Last().Content, Is.EqualTo("Test Content"));
            Assert.That(_result.Content.Last().Title, Is.EqualTo("Note 2"));
            Assert.That(_result.Content.Last().CreatedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.Last().ModifiedOn, Is.EqualTo(_timeProvider.GetUtcNow().DateTime));
            Assert.That(_result.Content.Last().IsArchived, Is.True);
            Assert.That(_result.Content.Last().Id, Is.EqualTo(Guid.Parse("15b1f591-bf92-4a8f-bbce-0f336d5b2492")));
        }));
      
    }
}