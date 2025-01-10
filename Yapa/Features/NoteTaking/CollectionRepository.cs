using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Features.NoteTaking;

public interface ICollectionRepository
{
    public Task<List<CollectionDto>> GetAll();
    public Task<CollectionDto> Add(CollectionDto collection);
}

public sealed class CollectionRepository : ICollectionRepository
{
    private readonly ISessionFactory _sessionFactory;

    public CollectionRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task<List<CollectionDto>> GetAll()
    {
        using var session = _sessionFactory.OpenSession();
        var result = await session.QueryOver<CollectionRecord>().ListAsync();

        var dtoResult = result.Select(x => new CollectionDto
        {
            Id = x.Id,
            Name = x.Name,
            IsArchived = x.IsArchived,
            Notes = x.Notes?.Select(y => new NoteDto
            {
                Id = y.Id,
                Content = y.Content
            }).ToList() ?? new List<NoteDto>(),
        }).ToList();
        
        return dtoResult;
    }

    public async Task<CollectionDto> Add(CollectionDto collection)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var record = new CollectionRecord
        {
            Id = collection.Id,
            Name = collection.Name,
            IsArchived = collection.IsArchived,
            Notes = collection.Notes.Select(x => new NoteRecord
            {
                Id = x.Id,
                Content = x.Content,
                IsArchived = x.IsArchived,
                Title = x.Title,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
            }).ToList()
        };

        await session.SaveAsync(record);
        await transaction.CommitAsync();
        return collection;
    }
}