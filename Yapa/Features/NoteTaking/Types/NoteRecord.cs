using System;
using FluentNHibernate.Mapping;

namespace Yapa.Features.NoteTaking.Types;

public class NoteRecord
{
    public virtual Guid Id { get; set; }
    public virtual CollectionRecord CollectionRecord { get; set; }
    public virtual string Title { get; set; }
    public virtual string Content { get; set; }
    public virtual DateTime CreatedOn { get; set; }
    public virtual DateTime ModifiedOn { get; set; }
    public virtual bool IsArchived { get; set; }
}

public class NoteDto
{
    public Guid Id { get; set; }
    public virtual string Title { get; set; }
    public virtual string Content { get; set; }
    public virtual DateTime CreatedOn { get; set; }
    public virtual DateTime ModifiedOn { get; set; }
    public virtual bool IsArchived { get; set; }
}

public class NoteMap : ClassMap<NoteRecord>
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

        References(x => x.CollectionRecord)
            .Column("CollectionId")
            .LazyLoad();
    }
}