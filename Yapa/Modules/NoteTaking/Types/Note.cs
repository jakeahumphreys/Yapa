using System;
using FluentNHibernate.Mapping;

namespace Yapa.Modules.NoteTaking.Types;

public class Note
{
    public virtual Guid Id { get; set; }
    public virtual Collection Collection { get; set; }
    public virtual string Title { get; set; }
    public virtual string Content { get; set; }
    public virtual DateTime CreatedOn { get; set; }
    public virtual DateTime ModifiedOn { get; set; }
    public virtual bool IsArchived { get; set; }
}

public class NoteMap : ClassMap<Note>
{
    public NoteMap()
    {
        Table("Notes");
        
        Id(x => x.Id).GeneratedBy.GuidComb();
        Map(x => x.Title).Not.Nullable();
        Map(x => x.Content).Not.Nullable();
        Map(x => x.CreatedOn).Not.Nullable();
        Map(x => x.ModifiedOn).Not.Nullable();
        Map(x => x.IsArchived).Not.Nullable();

        References(x => x.Collection)
            .Column("CollectionId")
            .LazyLoad();
    }
}