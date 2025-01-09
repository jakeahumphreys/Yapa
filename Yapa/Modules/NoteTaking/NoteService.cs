using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Modules.NoteTaking;

public interface INoteService
{
    Task<NoteRecord> GetNoteById(Guid id);
    Task<IList<NoteRecord>> GetAllNotes();
    Task<IList<NoteRecord>> GetNotesByCollection(Guid collectionId);
    Task<IList<NoteRecord>> GetArchivedNotes();
    Task CreateNote(NoteRecord noteRecord);
    Task<NoteRecord> UpdateNote(NoteRecord noteRecord);
    Task ArchiveNote(Guid noteId);
}

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly TimeProvider _timeProvider;

    public NoteService(INoteRepository noteRepository, TimeProvider timeProvider)
    {
        _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        _timeProvider = timeProvider;
    }

    public async Task<NoteRecord> GetNoteById(Guid id)
    {
        return await _noteRepository.GetById(id);
    }

    public async Task<IList<NoteRecord>> GetAllNotes()
    {
        return await _noteRepository.GetAll();
    }

    public async Task<IList<NoteRecord>> GetNotesByCollection(Guid collectionId)
    {
        return await _noteRepository.GetByCollection(collectionId);
    }

    public async Task<IList<NoteRecord>> GetArchivedNotes()
    {
        return await _noteRepository.GetArchivedNotes();
    }

    public async Task CreateNote(NoteRecord noteRecord)
    {
        if (noteRecord == null) throw new ArgumentNullException(nameof(noteRecord));

        noteRecord.CreatedOn = _timeProvider.GetUtcNow().DateTime;
        noteRecord.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        await _noteRepository.Add(noteRecord);
    }

    public async Task<NoteRecord> UpdateNote(NoteRecord noteRecord)
    {
        if (noteRecord == null) throw new ArgumentNullException(nameof(noteRecord));

        noteRecord.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        await _noteRepository.Update(noteRecord);

        return noteRecord;
    }

    public async Task ArchiveNote(Guid noteId)
    {
        var note = await _noteRepository.GetById(noteId);
        if (note == null) throw new KeyNotFoundException($"No note found with ID {noteId}");

        await _noteRepository.Archive(note);
    }
}
