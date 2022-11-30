namespace Capstone3.Models
{
    public class Apps
    {
        public IList<AppData> apps { get; set; } = new List<AppData>();
        public string Query { get; set; }
    }
}
