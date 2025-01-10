using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Features.NoteTaking;

public class NoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly TimeProvider _timeProvider;

    public NoteService(INoteRepository noteRepository, TimeProvider timeProvider)
    {
        _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        _timeProvider = timeProvider;
    }

    public async Task<Result<NoteRecord>> GetNoteById(Guid id)
    {
        var noteById = await _noteRepository.GetById(id);
        
        if(noteById == null)
            return Result<NoteRecord>.Failure($"A note with id {id} was not found");
        
        return Result<NoteRecord>.Success(noteById);
    }

    public async Task<Result<IList<NoteRecord>>> GetAllNotes()
    {
        var allNotes = await _noteRepository.GetAll();
        
        if(allNotes.Count == 0)
            return Result<IList<NoteRecord>>.Failure($"No notes found");
        
        return Result<IList<NoteRecord>>.Success(allNotes);
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
        noteRecord.CreatedOn = _timeProvider.GetUtcNow().DateTime;
        noteRecord.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        await _noteRepository.Add(noteRecord);
    }

    public async Task<Result<NoteRecord>> UpdateNote(NoteRecord noteRecord)
    {
        var existingNote = await _noteRepository.GetById(noteRecord.Id);
        
        if(existingNote == null)
            return Result<NoteRecord>.Failure($"A note with id {noteRecord.Id} was not found");
        
        noteRecord.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        var updatedNote = await _noteRepository.Update(noteRecord);

        return Result<NoteRecord>.Success(updatedNote);
    }
}
