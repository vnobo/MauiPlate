using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiPlate.Models;

namespace MauiPlate.PageModels
{
    public partial class TaskDetailPageModel(
        ProjectRepository projectRepository,
        TaskRepository taskRepository,
        ModalErrorHandler errorHandler)
        : ObservableObject, IQueryAttributable
    {
        public const string ProjectQueryKey = "project";
        private ProjectTask? _task;
        private bool _canDelete;

        [ObservableProperty]
        public partial string Title { get; set; }= string.Empty;

        [ObservableProperty]
        public partial bool IsCompleted { get; set; }
        [ObservableProperty]
        public partial List<Project> Projects { get; set; } = [];

        [ObservableProperty]
        public partial Project? Project { get; set; }

        [ObservableProperty]
        public partial int SelectedProjectIndex { get; set; } = -1;


        [ObservableProperty]
        public partial bool IsExistingProject { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            LoadTaskAsync(query).FireAndForgetSafeAsync(errorHandler);
        }

        private async Task LoadTaskAsync(IDictionary<string, object> query)
        {
            if (query.TryGetValue(ProjectQueryKey, out var project))
                Project = (Project)project;

            var taskId = 0;

            if (query.TryGetValue("id", out object? value))
            {
                taskId = Convert.ToInt32(value);
                _task = await taskRepository.GetAsync(taskId);

                if (_task is null)
                {
                    errorHandler.HandleError(new Exception($"Task Id {taskId} isn't valid."));
                    return;
                }

                Project = await projectRepository.GetAsync(_task.ProjectID);
            }
            else
            {
                _task = new ProjectTask();
            }

            // If the project is new, we don't need to load the project dropdown
            if (Project?.ID == 0)
            {
                IsExistingProject = false;
            }
            else
            {
                Projects = await projectRepository.ListAsync();
                IsExistingProject = true;
            }

            if (Project is not null)
                SelectedProjectIndex = Projects.FindIndex(p => p.ID ==Project.ID);
            else if (_task?.ProjectID > 0)
                SelectedProjectIndex = Projects.FindIndex(p => p.ID == _task.ProjectID);

            if (taskId > 0)
            {
                if (_task is null)
                {
                    errorHandler.HandleError(new Exception($"Task with id {taskId} could not be found."));
                    return;
                }

                Title = _task.Title;
                IsCompleted = _task.IsCompleted;
                CanDelete = true;
            }
            else
            {
                _task = new ProjectTask()
                {
                    ProjectID = Project?.ID ?? 0
                };
            }
        }

        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                _canDelete = value;
                DeleteCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (_task is null)
            {
                errorHandler.HandleError(
                    new Exception("Task or project is null. The task could not be saved."));

                return;
            }

            _task.Title = Title;

            int projectId = Project?.ID ?? 0;

            if (Projects.Count > SelectedProjectIndex && SelectedProjectIndex >= 0)
                _task.ProjectID = projectId = Projects[SelectedProjectIndex].ID;

            _task.IsCompleted = IsCompleted;

            if (Project?.ID == projectId && !Project.Tasks.Contains(_task))
               Project.Tasks.Add(_task);

            if (_task.ProjectID > 0)
                taskRepository.SaveItemAsync(_task).FireAndForgetSafeAsync(errorHandler);

            await Shell.Current.GoToAsync("..?refresh=true");

            if (_task.ID > 0)
                await AppShell.DisplayToastAsync("Task saved");
        }

        [RelayCommand(CanExecute = nameof(CanDelete))]
        private async Task Delete()
        {
            if (_task is null || Project is null)
            {
                errorHandler.HandleError(
                    new Exception("Task is null. The task could not be deleted."));

                return;
            }

            if (Project.Tasks.Contains(_task))
                Project.Tasks.Remove(_task);

            if (_task.ID > 0)
                await taskRepository.DeleteItemAsync(_task);

            await Shell.Current.GoToAsync("..?refresh=true");
            await AppShell.DisplayToastAsync("Task deleted");
        }
    }
}