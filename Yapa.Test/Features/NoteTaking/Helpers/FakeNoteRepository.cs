using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Test.Features.NoteTaking.Helpers;

public sealed class FakeNoteRepository : INoteRepository
{
    private List<NoteRecord> _notes = new List<NoteRecord>();
    
    public async Task<NoteRecord> GetById(Guid id)
    {
        return (await Task.FromResult(_notes.FirstOrDefault(x => x.Id == id)))!;
    }

    public async Task<IList<NoteRecord>> GetAll()
    {
        return await Task.FromResult(_notes);
    }

    public async Task<IList<NoteRecord>> GetByCollection(Guid collectionId)
    {
        return await Task.FromResult(_notes.Where(x => x.CollectionRecord.Id == collectionId).ToList());
    }

    public async Task<NoteRecord> Add(NoteRecord noteRecord)
    {
        _notes.Add(noteRecord);
        return await Task.FromResult(noteRecord);
    }

    public async Task<NoteRecord> Update(NoteRecord noteRecord)
    {
        _notes.FirstOrDefault(n => n.Id == noteRecord.Id);
        return await Task.FromResult(noteRecord);
    }

    public async Task<IList<NoteRecord>> GetArchivedNotes()
    {
        return await Task.FromResult(_notes.Where(x => x.IsArchived).ToList());
    }
}