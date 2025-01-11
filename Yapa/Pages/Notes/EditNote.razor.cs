using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Yapa.Features.NoteTaking;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Pages.Notes;

public partial class EditNote : ComponentBase
{
    [Parameter] public string NoteId { get; set; }
    [Parameter] public string CollectionId { get; set; }
    [Inject] private NoteService NoteService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    
    private NoteDto note;
    private CancellationTokenSource _debounceCts;

    private DateTime LastSavedTime;
    private string CurrentContent;

    protected override async Task OnInitializedAsync()
    {
        if(Guid.TryParse(NoteId, out var noteIdGuid) && noteIdGuid != Guid.Empty)
        {
            var noteResult = await NoteService.GetNoteById(noteIdGuid);
            if (noteResult.HasError)
            {
                //handle error
            }

            note = noteResult.Content;
        }
        else
        {
            note = new NoteDto
            {
                Id = Guid.Empty,
                Content = string.Empty,
                CreatedOn = DateTime.UtcNow,
                Title = string.Empty,
                ModifiedOn = DateTime.UtcNow
            };
        }
    }

    private async Task OnInputChanged(string value)
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _debounceCts = new CancellationTokenSource();

        try
        {
            var inputValue = value;
            
            await Task.Delay(1500, _debounceCts.Token);

            if (!string.IsNullOrEmpty(inputValue))
            {
                CurrentContent = inputValue;
                
                await Save(inputValue);
                
                LastSavedTime = DateTime.Now;
                StateHasChanged();
            }
        }
        catch (TaskCanceledException e)
        {
            //swallow for now
        }
    }

    private async Task Save(string inputValue)
    {
        if (note != null)
        {
            if (note.Id == Guid.Empty)
            {
                note.Title = inputValue.Length > 10 ? inputValue.Substring(0, 10) : inputValue;
                note.Content = inputValue;
                var createdNote = await NoteService.CreateNote(note);
                note.Id = createdNote.Content.Id;
            }
            else
            {
                note.Content = inputValue;
                await NoteService.UpdateNote(note);
            }
            
            Snackbar.Add($"Note Saved", Severity.Success);
        }
    }
}