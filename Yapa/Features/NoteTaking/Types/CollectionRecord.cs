using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Yapa.Features.NoteTaking.Types;

public class CollectionRecord
{
    public virtual Guid Id { get; set; }
    public virtual string Name { get; set; }
    public virtual IList<NoteRecord> Notes { get; set; }
    public virtual bool IsArchived { get; set; }
}

public class CollectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsArchived { get; set; }
    public List<NoteDto> Notes { get; set; } = new List<NoteDto>();
}

public class CollectionMap : ClassMap<CollectionRecord>
{
    public CollectionMap()
    {
        Table("Collections");
        
        Id(x => x.Id).GeneratedBy.GuidComb();
        Map(x => x.Name).Not.Nullable();
        Map(x => x.IsArchived).Not.Nullable();

        HasMany(x => x.Notes)
            .KeyColumn("CollectionId")
            .Inverse()
            .Cascade.All();
    }
}