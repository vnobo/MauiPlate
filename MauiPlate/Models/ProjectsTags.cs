using SQLite;

namespace MauiPlate.Models;

public class ProjectsTags
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int ProjectID { get; set; }
    public int TagID { get; set; }
}