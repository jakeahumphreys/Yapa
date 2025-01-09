using Yapa.Modules.NoteTaking;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Test.Modules.NoteTaking.Helpers;

public sealed class FakeNoteRepository : INoteRepository
{
    private List<NoteRecord> _notes = new List<NoteRecord>();
    
    public async Task<NoteRecord> GetById(Guid id)
    {
        return _notes.SingleOrDefault(n => n.Id == id);
    }

    public async Task<IList<NoteRecord>> GetAll()
    {
        return _notes;
    }

    public async Task<IList<NoteRecord>> GetByCollection(Guid collectionId)
    {
        return _notes.Where(x => x.CollectionRecord.Id == collectionId).ToList();
    }

    public async Task Add(NoteRecord noteRecord)
    {
        _notes.Add(noteRecord);
    }

    public async Task Update(NoteRecord noteRecord)
    {
        var noteToUpdate = _notes.SingleOrDefault(n => n.Id == noteRecord.Id);
        noteToUpdate = noteRecord;
    }

    public async Task Archive(NoteRecord noteRecord)
    {
        var noteToArchive = _notes.SingleOrDefault(n => n.Id == noteRecord.Id);
        noteToArchive.IsArchived = true;
    }

    public async Task<IList<NoteRecord>> GetArchivedNotes()
    {
        return _notes.Where(x => x.IsArchived).ToList();
    }
}