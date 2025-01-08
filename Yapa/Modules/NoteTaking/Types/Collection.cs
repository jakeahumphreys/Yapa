using System;
using System.Collections.Generic;

namespace Yapa.Modules.NoteTaking.Types;

public sealed class Collection
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    private List<Note> Notes { get; set; } = [];
    public bool IsArchived { get; set; }
}