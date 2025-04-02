using System.Text.Json;
using MauiPlate.Models;
using Microsoft.Extensions.Logging;

namespace MauiPlate.Data
{
    public class SeedDataService(
        ProjectRepository projectRepository,
        TaskRepository taskRepository,
        TagRepository tagRepository,
        CategoryRepository categoryRepository,
        ILogger<SeedDataService> logger)
    {
        private readonly string _seedDataFilePath = "SeedData.json";

        public async Task LoadSeedDataAsync()
        {
            ClearTables();

            await using Stream templateStream = await FileSystem.OpenAppPackageFileAsync(_seedDataFilePath);

            ProjectsJson? payload = null;
            try
            {
                payload = JsonSerializer.Deserialize(templateStream, JsonContext.Default.ProjectsJson);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error deserializing seed data");
            }

            try
            {
                if (payload is not null)
                {
                    foreach (var project in payload.Projects)
                    {
                        if (project is null)
                        {
                            continue;
                        }

                        if (project.Category is not null)
                        {
                            await categoryRepository.SaveItemAsync(project.Category);
                            project.CategoryID = project.Category.Id;
                        }

                        await projectRepository.SaveItemAsync(project);

                        if (project?.Tasks is not null)
                        {
                            foreach (var task in project.Tasks)
                            {
                                task.ProjectID = project.ID;
                                await taskRepository.SaveItemAsync(task);
                            }
                        }

                        if (project?.Tags is not null)
                        {
                            foreach (var tag in project.Tags)
                            {
                                await tagRepository.SaveItemAsync(tag, project.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error saving seed data");
                throw;
            }
        }

        private async void ClearTables()
        {
            try
            {
                await Task.WhenAll(
                    projectRepository.DropTableAsync(),
                    taskRepository.DropTableAsync(),
                    tagRepository.DropTableAsync(),
                    categoryRepository.DropTableAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}