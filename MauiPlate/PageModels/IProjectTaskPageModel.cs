using CommunityToolkit.Mvvm.Input;
using MauiPlate.Models;

namespace MauiPlate.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}