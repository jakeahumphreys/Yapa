using System;

namespace Yapa.Modules.NoteTaking.Types;

public sealed class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public bool IsArchived { get; set; }
}