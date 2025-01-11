using System;
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
    [Inject] NavigationManager NavigationManager { get; set; }
    
    private List<NoteDto> Notes { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var noteResults = await NoteService.GetNotesForCollection(Guid.Parse(CollectionId));
        Notes = noteResults.Content;
        
        await base.OnInitializedAsync();
    }


    private void CreateNoteNote(string collectionId)
    {
        NavigationManager.NavigateTo($"/Note/EditNote/{collectionId}/new");
    }
}