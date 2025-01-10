using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Test.Modules.NoteTaking.Helpers;

public sealed class FakeCollectionRepository : ICollectionRepository
{
    private List<CollectionRecord> _collections = new List<CollectionRecord>();
    
    public async Task<List<CollectionRecord>> GetAll()
    {
        return await Task.FromResult(_collections);
    }

    public async Task<CollectionRecord> Add(CollectionRecord record)
    {
        _collections.Add(record);
        return await Task.FromResult(record);
    }
}