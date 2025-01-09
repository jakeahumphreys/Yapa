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
    Task<Note> GetById(Guid id);
    Task<IList<Note>> GetAll();
    Task<IList<Note>> GetByCollection(Guid collectionId);
    Task Add(Note note);
    Task Update(Note note);
    Task Archive(Note note);
    Task<IList<Note>> GetArchivedNotes();
}

public class NoteRepository : INoteRepository
{
    private readonly ISessionFactory _sessionFactory;

    public NoteRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async Task<Note> GetById(Guid id)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.GetAsync<Note>(id);
    }

    public async Task<IList<Note>> GetAll()
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<Note>().ToListAsync();
    }

    public async Task<IList<Note>> GetByCollection(Guid collectionId)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<Note>()
            .Where(note => note.Collection.Id == collectionId)
            .ToListAsync();
    }

    public async Task Add(Note note)
    {
        if (note == null) throw new ArgumentNullException(nameof(note));

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.SaveAsync(note);
        await transaction.CommitAsync();
    }

    public async Task Update(Note note)
    {
        if (note == null) throw new ArgumentNullException(nameof(note));

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.UpdateAsync(note);
        await transaction.CommitAsync();
    }

    public async Task Archive(Note note)
    {
        if (note == null) throw new ArgumentNullException(nameof(note));

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        note.IsArchived = true; // Mark as archived
        await session.UpdateAsync(note);
        await transaction.CommitAsync();
    }

    public async Task<IList<Note>> GetArchivedNotes()
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<Note>()
            .Where(note => note.IsArchived)
            .ToListAsync();
    }
}

