using Microsoft.Extensions.Time.Testing;
using Yapa.Common.Types;
using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;
using Yapa.Test.Modules.NoteTaking.Helpers;

namespace Yapa.Test.Modules.NoteTaking.GivenGetCollections;

[TestFixture]
[Parallelizable]
public sealed class WhenNoCollectionsArePresent
{
    private Result<List<CollectionRecord>> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var repository = new FakeCollectionRepository();
        
        var subject = new CollectionService(repository, new FakeTimeProvider());
        _result = await subject.GetAll();

    }

    [Test]
    public void ThenAnErrorIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.HasError, Is.True);
            Assert.That(_result.ErrorMessage, Is.EqualTo("No collection records found"));
        }));
    }
}