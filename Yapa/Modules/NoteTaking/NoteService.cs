using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Modules.NoteTaking;

public interface INoteService
{
    Task<Note> GetNoteById(Guid id);
    Task<IList<Note>> GetAllNotes();
    Task<IList<Note>> GetNotesByCollection(Guid collectionId);
    Task<IList<Note>> GetArchivedNotes();
    Task CreateNote(Note note);
    Task<Note> UpdateNote(Note note);
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

    public async Task<Note> GetNoteById(Guid id)
    {
        return await _noteRepository.GetById(id);
    }

    public async Task<IList<Note>> GetAllNotes()
    {
        return await _noteRepository.GetAll();
    }

    public async Task<IList<Note>> GetNotesByCollection(Guid collectionId)
    {
        return await _noteRepository.GetByCollection(collectionId);
    }

    public async Task<IList<Note>> GetArchivedNotes()
    {
        return await _noteRepository.GetArchivedNotes();
    }

    public async Task CreateNote(Note note)
    {
        if (note == null) throw new ArgumentNullException(nameof(note));

        note.CreatedOn = _timeProvider.GetUtcNow().DateTime;
        note.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        await _noteRepository.Add(note);
    }

    public async Task<Note> UpdateNote(Note note)
    {
        if (note == null) throw new ArgumentNullException(nameof(note));

        note.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        await _noteRepository.Update(note);

        return note;
    }

    public async Task ArchiveNote(Guid noteId)
    {
        var note = await _noteRepository.GetById(noteId);
        if (note == null) throw new KeyNotFoundException($"No note found with ID {noteId}");

        await _noteRepository.Archive(note);
    }
}
