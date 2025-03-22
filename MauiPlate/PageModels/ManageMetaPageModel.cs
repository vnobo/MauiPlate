using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiPlate.Data;
using MauiPlate.Models;
using MauiPlate.Services;

namespace MauiPlate.PageModels
{
    public partial class ManageMetaPageModel(
        CategoryRepository categoryRepository,
        TagRepository tagRepository,
        SeedDataService seedDataService)
        : ObservableObject
    {
        [ObservableProperty]
        public partial ObservableCollection<Category> Categories { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<Tag> Tags { get; set; }= [];

        private async Task LoadData()
        {
            var categoriesList = await categoryRepository.ListAsync();
            Categories = new ObservableCollection<Category>(categoriesList);
            var tagsList = await tagRepository.ListAsync();
            Tags = new ObservableCollection<Tag>(tagsList);
        }

        [RelayCommand]
        private Task Appearing()
            => LoadData();

        [RelayCommand]
        private async Task SaveCategories()
        {
            foreach (var category in Categories)
            {
                await categoryRepository.SaveItemAsync(category);
            }

            await AppShell.DisplayToastAsync("Categories saved");
        }

        [RelayCommand]
        private async Task DeleteCategory(Category category)
        {
            Categories.Remove(category);
            await categoryRepository.DeleteItemAsync(category);
            await AppShell.DisplayToastAsync("Category deleted");
        }

        [RelayCommand]
        private async Task AddCategory()
        {
            var category = new Category();
            Categories.Add(category);
            await categoryRepository.SaveItemAsync(category);
            await AppShell.DisplayToastAsync("Category added");
        }

        [RelayCommand]
        private async Task SaveTags()
        {
            foreach (var tag in Tags)
            {
                await tagRepository.SaveItemAsync(tag);
            }

            await AppShell.DisplayToastAsync("Tags saved");
        }

        [RelayCommand]
        private async Task DeleteTag(Tag tag)
        {
            Tags.Remove(tag);
            await tagRepository.DeleteItemAsync(tag);
            await AppShell.DisplayToastAsync("Tag deleted");
        }

        [RelayCommand]
        private async Task AddTag()
        {
            var tag = new Tag();
            Tags.Add(tag);
            await tagRepository.SaveItemAsync(tag);
            await AppShell.DisplayToastAsync("Tag added");
        }

        [RelayCommand]
        private async Task Reset()
        {
            Preferences.Default.Remove("is_seeded");
            await seedDataService.LoadSeedDataAsync();
            Preferences.Default.Set("is_seeded", true);
            await Shell.Current.GoToAsync("//main");
        }
    }
}
