using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Features.NoteTaking;

public interface INoteRepository
{
    Task<NoteDto> GetById(int id);
    Task<List<NoteDto>> GetAll();
    Task<List<NoteDto>> GetNotesForCollection(int collectionId);
    Task<NoteDto> Add(NoteDto note);
    Task<NoteDto> Update(NoteDto noteRecord);
    Task<List<NoteDto>> GetArchivedNotes();
}

public class NoteRepository : INoteRepository
{
    private readonly ISessionFactory _sessionFactory;

    public NoteRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task<NoteDto> GetById(int id)
    {
        using var session = _sessionFactory.OpenSession();
        var result = await session.GetAsync<NoteRecord>(id);
        return new NoteDto
        {
            Id = result.Id,
            Content = result.Content,
            Title = result.Title,
            IsArchived = result.IsArchived,
            CreatedOn = result.CreatedOn,
            ModifiedOn = result.ModifiedOn
        };
    }

    public async Task<List<NoteDto>> GetAll()
    {
        using var session = _sessionFactory.OpenSession();
        var results = await session.Query<NoteRecord>().Select(x => new NoteDto
        {
            Id = x.Id,
            Content = x.Content,
            Title = x.Title,
            IsArchived = x.IsArchived,
            CreatedOn = x.CreatedOn
        }).ToListAsync();
        
        return results;
    }

    public async Task<List<NoteDto>> GetNotesForCollection(int collectionId)
    {
        using var session = _sessionFactory.OpenSession();
        var results = await session.QueryOver<NoteRecord>()
            .Where(note => note.CollectionRecord.Id == collectionId).ListAsync();

        var dtoResults = results
            .Select(x => new NoteDto
            {
                Id = x.Id,
                Content = x.Content,
                Title = x.Title,
                IsArchived = x.IsArchived,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn,
                CollectionRecordId = x.CollectionRecord.Id
            }).ToList();
        
        return dtoResults;
    }

    public async Task<NoteDto> Add(NoteDto note)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var record = new NoteRecord
        {
            Title = note.Title,
            Content = note.Content,
            IsArchived = note.IsArchived,
            CreatedOn = note.CreatedOn,
            ModifiedOn = note.ModifiedOn,
            CollectionRecord = new CollectionRecord
            {
                Id = note.CollectionRecordId
            }
        };

        await session.SaveAsync(record);
        await transaction.CommitAsync();

        note.Id = record.Id;
        
        return note;
    }

    public async Task<NoteDto> Update(NoteDto note)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        var record = await session.GetAsync<NoteRecord>(note.Id);
        
        record.Content = note.Content;
        record.IsArchived = note.IsArchived;
        record.ModifiedOn = note.ModifiedOn;

        await session.UpdateAsync(record);
        await transaction.CommitAsync();
        return note;
    }

    public async Task<List<NoteDto>> GetArchivedNotes()
    {
        using var session = _sessionFactory.OpenSession();
        
        var results = await session.Query<NoteRecord>()
            .Where(note => note.IsArchived)
            .Select(x => new NoteDto
            {
                Id = x.Id,
                Content = x.Content,
                Title = x.Title,
                IsArchived = x.IsArchived,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
            }).ToListAsync();

        return results;
    }
}

