using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Pages.Notes;

public partial class Index : ComponentBase
{
    [Inject]
    public CollectionService CollectionService { get; set; }
    
    private List<CollectionRecord> Collections { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var storedCollections = await CollectionService.GetAll();
        
        Collections = storedCollections.Content;
        await base.OnInitializedAsync();
    }
}