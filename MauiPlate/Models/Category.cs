using System.Text.Json.Serialization;

namespace MauiPlate.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Color { get; set; } = "#FF0000";

        [JsonIgnore]
        public Brush ColorBrush => new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb(Color));

        public override string ToString() => $"{Title}";
    }
}