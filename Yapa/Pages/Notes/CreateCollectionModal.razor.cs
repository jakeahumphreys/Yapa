using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Yapa.Features.NoteTaking;

namespace Yapa.Pages.Notes;

public partial class CreateCollectionModal : ComponentBase
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public EventCallback<bool> OnDialogClose { get; set; }
    
    [Inject] private CollectionService CollectionService { get; set; }
    
    private string _collectionName;
    private string _errorMessage;

    private async Task AddCollection()
    {
        var result = await CollectionService.AddCollection(_collectionName);
        
        if(result.HasError)
            _errorMessage = result.ErrorMessage;
        else
            MudDialog.Close(DialogResult.Ok(true));
    }

    private void CloseDialog(bool result)
    {
        OnDialogClose.InvokeAsync(result);
        MudDialog.Close(DialogResult.Ok(result));
    }
}