#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiPlate.Data;
using MauiPlate.Models;
using MauiPlate.Services;

namespace MauiPlate.PageModels
{
    public partial class ProjectListPageModel(ProjectRepository projectRepository) : ObservableObject
    {
        [ObservableProperty] public partial List<Project> Projects { get; set; } = [];

        [RelayCommand]
        private async Task Appearing()
        {
            Projects = await projectRepository.ListAsync();
        }

        [RelayCommand]
        Task NavigateToProject(Project project)
            => Shell.Current.GoToAsync($"project?id={project.ID}");

        [RelayCommand]
        async Task AddProject()
        {
            await Shell.Current.GoToAsync($"project");
        }
    }
}