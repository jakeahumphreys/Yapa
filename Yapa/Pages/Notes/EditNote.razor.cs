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
        if(int.TryParse(NoteId, out var noteInt) && noteInt != 0)
        {
            var noteResult = await NoteService.GetNoteById(noteInt);
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
                Id = 0,
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
            if (note.Id == 0)
            {
                note.CollectionRecordId = int.Parse(CollectionId);
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