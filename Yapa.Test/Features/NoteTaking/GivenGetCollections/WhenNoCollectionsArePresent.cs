using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenGetCollections;

[TestFixture]
[Parallelizable]
public sealed class WhenNoCollectionsArePresent
{
    private Result<List<CollectionDto>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var repository = new FakeCollectionRepository();
        
        var subject = new CollectionService(repository);
        _result = await subject.GetAll();

    }

    [Test]
    public void ThenAnEmptyListIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.HasError, Is.False);
            Assert.That(_result.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(_result.Content, Is.Empty);
        }));
    }
}