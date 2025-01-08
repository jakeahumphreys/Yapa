using FluentNHibernate.Mapping;

namespace Yapa.Data;

public class SchemaVersion
{
    public virtual int Id { get; set; }
    public virtual int VersionNumber { get; set; }
}

public class SchemaVersionMap : ClassMap<SchemaVersion>
{
    public SchemaVersionMap()
    {
        Table("SchemaVersion");
        Id(x => x.Id).GeneratedBy.Identity();
        Map(x => x.VersionNumber).Not.Nullable();
    }
}
