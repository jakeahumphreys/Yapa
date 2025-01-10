using Yapa.Common.Types;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;
using Yapa.Test.Features.NoteTaking.Helpers;

namespace Yapa.Test.Features.NoteTaking.GivenAddCollection;

[TestFixture]
[Parallelizable]
public sealed class WhenTheCollectionHasNoName
{
    private Result<CollectionDto> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var repository = new FakeCollectionRepository();
        var subject = new CollectionService(repository);
        _result = await subject.AddCollection("");
    }
    [Test]
    public void ThenAnErrorIsReturned()
    {
        Assert.Multiple((() =>
        {
            Assert.That(_result.ErrorMessage, Is.EqualTo("Collection name cannot be empty"));
            Assert.That(_result.HasError, Is.True);
        }));
    }
}