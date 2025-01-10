using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Pages.Notes;

public partial class Index : ComponentBase
{
    [Inject]
    public CollectionService CollectionService { get; set; }
    
    [Inject]
    public IDialogService DialogService { get; set; }
    
    private List<CollectionDto> Collections { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var storedCollections = await CollectionService.GetAll();
        
        Collections = storedCollections.Content;
        await base.OnInitializedAsync();
    }

    private void OpenCreateCollectionModal()
    {
        var parameters = new DialogParameters();
        var dialog = DialogService.Show<CreateCollectionModal>();
        dialog.Result.ContinueWith(OnDialogClose);
    }

    private async Task OnDialogClose(Task<DialogResult> task)
    {
        if (task.Result?.Data != null && (bool)task.Result.Data)
        {
            var storedCollections = await CollectionService.GetAll();
            Collections = storedCollections.Content;
            
            StateHasChanged();
        }
    }

    private async Task HandleClick()
    {
        await Task.CompletedTask;
    }
}