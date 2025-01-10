﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Pages.Notes;

public partial class Collection : ComponentBase
{
    [Parameter] public string CollectionId { get; set; } 
    [Inject] NoteService NoteService { get; set; }
    
    private List<NoteDto> Notes { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var noteResults = await NoteService.GetNotesForCollection(Guid.Parse(CollectionId));
        Notes = noteResults;
        
        await base.OnInitializedAsync();
    }
}