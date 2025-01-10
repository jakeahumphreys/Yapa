using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenGetCollections;

[TestFixture]
[Parallelizable]
public sealed class WhenCollectionsArePresent
{
    private Result<List<CollectionDto>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var repository = new FakeCollectionRepository();

        await repository.Add(new CollectionDto
        {
            Id = Guid.Parse("270c3970-d4b1-48b6-b4d4-cec5ee13e30a"),
            Name = "Test Collection 1",
            IsArchived = false
        });
        
        await repository.Add(new CollectionDto
        {
            Id = Guid.Parse("c2a3fa7b-1298-46db-b94b-e0bb659c00c3"),
            Name = "Test Collection 2",
            IsArchived = true
        });
        
        var subject = new CollectionService(repository);
        _result = await subject.GetAll();

    }

    [Test]
    public void ThenTheExpectedNumberOfCollectionsAreReturned()
    {
        Assert.That(_result.Content, Has.Count.EqualTo(2));
    }
    
    [Test]
    public void ThenTheExpectedCollectionsAreReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result.Content.First().Id, Is.EqualTo(Guid.Parse("270c3970-d4b1-48b6-b4d4-cec5ee13e30a")));
            Assert.That(_result.Content.First().Name, Is.EqualTo("Test Collection 1"));
            Assert.That(_result.Content.First().IsArchived, Is.False);
            Assert.That(_result.Content.Last().Id, Is.EqualTo(Guid.Parse("c2a3fa7b-1298-46db-b94b-e0bb659c00c3")));
            Assert.That(_result.Content.Last().Name, Is.EqualTo("Test Collection 2"));
            Assert.That(_result.Content.Last().IsArchived, Is.True);
        });
    }
}