using Microsoft.Extensions.Time.Testing;
using Moq;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Test.Modules.NoteTaking;

[TestFixture]
[Parallelizable]
public sealed class GivenANoteToAdd
{
    private Mock<INoteRepository> _noteRepository;
    private DateTime _date;
    private FakeTimeProvider _timeProvider;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _noteRepository = new Mock<INoteRepository>();
        _timeProvider = new FakeTimeProvider(DateTime.UtcNow);
        var subject = new NoteService(_noteRepository.Object, _timeProvider);
        _date = new DateTime(2020, 1, 1);

        await subject.CreateNote(new Note
        {
            Id = Guid.Parse("60d0c577-95aa-4255-b652-a40b3c686716"),
            Collection = null,
            Content = "Test Content",
            Title = "Test Note",
            IsArchived = false,
        });
    }
    
    [Test]
    public void ThenTheRepositoryIsCalled()
    {
        _noteRepository.Verify(x => x.Add(It.Is<Note>(y => y.Id == Guid.Parse("60d0c577-95aa-4255-b652-a40b3c686716") && 
                                                           y.Collection == null &&
                                                           y.CreatedOn == _timeProvider.GetUtcNow() &&
                                                           y.Content == "Test Content" &&
                                                           y.Title == "Test Note" &&
                                                           y.IsArchived == false &&
                                                           y.ModifiedOn == _timeProvider.GetUtcNow())), Times.Once);
    }
}