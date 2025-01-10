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
        _noteRepository = noteRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result<NoteDto>> GetNoteById(Guid id)
    {
        var noteById = await _noteRepository.GetById(id);
        
        if(noteById == null)
            return Result<NoteDto>.Failure($"A note with id {id} was not found");
        
        return Result<NoteDto>.Success(noteById);
    }

    public async Task<Result<List<NoteDto>>> GetAllNotes()
    {
        var allNotes = await _noteRepository.GetAll();
        
        return Result<List<NoteDto>>.Success(allNotes);
    }

    public async Task<List<NoteDto>> GetNotesForCollection(Guid collectionId)
    {
        return await _noteRepository.GetNotesForCollection(collectionId);
    }

    public async Task<List<NoteDto>> GetArchivedNotes()
    {
        return await _noteRepository.GetArchivedNotes();
    }

    public async Task<NoteDto> CreateNote(NoteDto noteDto)
    {
        noteDto.Id = Guid.NewGuid();
        noteDto.CreatedOn = _timeProvider.GetUtcNow().DateTime;
        noteDto.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        await _noteRepository.Add(noteDto);
        return noteDto;
    }

    public async Task<Result<NoteDto>> UpdateNote(NoteDto noteDto)
    {
        var existingNote = await _noteRepository.GetById(noteDto.Id);
        
        if(existingNote == null)
            return Result<NoteDto>.Failure($"A note with id {noteDto.Id} was not found");
        
        noteDto.ModifiedOn = _timeProvider.GetUtcNow().DateTime;
        var updatedNote = await _noteRepository.Update(noteDto);

        return Result<NoteDto>.Success(noteDto);
    }
}
