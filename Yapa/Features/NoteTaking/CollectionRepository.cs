using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Features.NoteTaking;

public interface ICollectionRepository
{
    public Task<List<CollectionRecord>> GetAll();
    public Task<CollectionRecord> Add(CollectionRecord record);
}

public sealed class CollectionRepository : ICollectionRepository
{
    private readonly ISessionFactory _sessionFactory;

    public CollectionRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task<List<CollectionRecord>> GetAll()
    {
        using var session = _sessionFactory.OpenSession();
        var result = await session.QueryOver<CollectionRecord>().ListAsync();
        return result.ToList();
    }

    public async Task<CollectionRecord> Add(CollectionRecord record)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.SaveAsync(record);
        await transaction.CommitAsync();
        return record;
    }
}