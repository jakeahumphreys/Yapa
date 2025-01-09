using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Test.Modules.NoteTaking;

public sealed class FakeNoteRepository : INoteRepository
{
    private List<Note> _notes = new List<Note>();
    
    public async Task<Note> GetById(Guid id)
    {
        return _notes.SingleOrDefault(n => n.Id == id);
    }

    public async Task<IList<Note>> GetAll()
    {
        return _notes;
    }

    public async Task<IList<Note>> GetByCollection(Guid collectionId)
    {
        return _notes.Where(x => x.Collection.Id == collectionId).ToList();
    }

    public async Task Add(Note note)
    {
        _notes.Add(note);
    }

    public async Task Update(Note note)
    {
        var noteToUpdate = _notes.SingleOrDefault(n => n.Id == note.Id);
        noteToUpdate = note;
    }

    public async Task Archive(Note note)
    {
        var noteToArchive = _notes.SingleOrDefault(n => n.Id == note.Id);
        noteToArchive.IsArchived = true;
    }

    public async Task<IList<Note>> GetArchivedNotes()
    {
        return _notes.Where(x => x.IsArchived).ToList();
    }
}