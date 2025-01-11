using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Test.Features.NoteTaking.Helpers;

public sealed class FakeNoteRepository : INoteRepository
{
    private List<NoteDto> _notes = new List<NoteDto>();
    
    public async Task<NoteDto> GetById(int id)
    {
        return (await Task.FromResult(_notes.FirstOrDefault(x => x.Id == id)))!;
    }

    public async Task<List<NoteDto>> GetAll()
    {
        return await Task.FromResult(_notes);
    }

    public async Task<List<NoteDto>> GetNotesForCollection(int collectionId)
    {
        return await Task.FromResult(_notes.Where(x => x.CollectionRecordId == collectionId).ToList());
    }

    public async Task<NoteDto> Add(NoteDto note)
    {
        _notes.Add(note);
        return await Task.FromResult(note);
    }

    public async Task<NoteDto> Update(NoteDto note)
    {
        _notes.FirstOrDefault(n => n.Id == note.Id);
        return await Task.FromResult(note);
    }

    public async Task<List<NoteDto>> GetArchivedNotes()
    {
        return await Task.FromResult(_notes.Where(x => x.IsArchived).ToList());
    }
}