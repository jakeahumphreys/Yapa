using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Modules.NoteTaking;

public interface INoteRepository
{
    Task<NoteRecord> GetById(Guid id);
    Task<IList<NoteRecord>> GetAll();
    Task<IList<NoteRecord>> GetByCollection(Guid collectionId);
    Task<NoteRecord> Add(NoteRecord noteRecord);
    Task<NoteRecord> Update(NoteRecord noteRecord);
    Task<IList<NoteRecord>> GetArchivedNotes();
}

public class NoteRepository : INoteRepository
{
    private readonly ISessionFactory _sessionFactory;

    public NoteRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async Task<NoteRecord> GetById(Guid id)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.GetAsync<NoteRecord>(id);
    }

    public async Task<IList<NoteRecord>> GetAll()
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<NoteRecord>().ToListAsync();
    }

    public async Task<IList<NoteRecord>> GetByCollection(Guid collectionId)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<NoteRecord>()
            .Where(note => note.CollectionRecord.Id == collectionId)
            .ToListAsync();
    }

    public async Task<NoteRecord> Add(NoteRecord noteRecord)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.SaveAsync(noteRecord);
        await transaction.CommitAsync();
        return noteRecord;
    }

    public async Task<NoteRecord> Update(NoteRecord noteRecord)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.UpdateAsync(noteRecord);
        await transaction.CommitAsync();
        return noteRecord;
    }

    public async Task<IList<NoteRecord>> GetArchivedNotes()
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<NoteRecord>()
            .Where(note => note.IsArchived)
            .ToListAsync();
    }
}

