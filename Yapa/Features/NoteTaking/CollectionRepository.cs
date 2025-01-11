using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Features.NoteTaking;

public interface ICollectionRepository
{
    public Task<List<CollectionDto>> GetAll();
    public Task<CollectionDto> Add(CollectionDto collection);
    public Task<CollectionDto> GetById(int id);
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
        using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

        var record = new CollectionRecord
        {
            Id = collection.Id,
            Name = collection.Name,
            IsArchived = collection.IsArchived
        };

        await session.SaveAsync(record);
        await transaction.CommitAsync();

        collection.Id = record.Id;
        
        return collection;
    }
    
    public async Task <CollectionDto> GetById(int id)
    {
        using var session = _sessionFactory.OpenSession();
        var result = await session.GetAsync<CollectionRecord>(id);

        return new CollectionDto
        {
            Id = result.Id,
            Name = result.Name,
            IsArchived = result.IsArchived,
            Notes = result.Notes.Select(x => new NoteDto
            {
                Id = x.Id,
                Content = x.Content,
                IsArchived = x.IsArchived,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn,
                Title = x.Title
            }).ToList()
        };
    }
}