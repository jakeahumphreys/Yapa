using System;
using System.Collections.Generic;
using System.Threading;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;

namespace Yapa.Modules.NoteTaking.Types;

public class Collection
{
    public virtual Guid Id { get; set; }
    public virtual string Name { get; set; }
    public virtual List<Note> Notes { get; set; } = [];
    public virtual bool IsArchived { get; set; }
}

public class CollectionMap : ClassMap<Collection>
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