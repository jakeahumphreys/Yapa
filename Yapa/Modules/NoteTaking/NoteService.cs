using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapa.Common.Types;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Modules.NoteTaking;

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
}
