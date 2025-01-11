using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Test.Features.NoteTaking.Helpers;

public sealed class FakeCollectionRepository : ICollectionRepository
{
    private List<CollectionDto> _collections = new List<CollectionDto>();
    
    public async Task<List<CollectionDto>> GetAll()
    {
        return await Task.FromResult(_collections);
    }

    public async Task<CollectionDto> Add(CollectionDto record)
    {
        _collections.Add(record);
        return await Task.FromResult(record);
    }

    public async Task<CollectionDto> GetById(Guid id)
    {
        var result = _collections.FirstOrDefault(x => x.Id == id)!;
        return await Task.FromResult(result);
    }
}