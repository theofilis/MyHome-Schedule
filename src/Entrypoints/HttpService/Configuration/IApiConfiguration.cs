namespace MyHome.Dashboard.Configuration
{
    public interface IApiConfiguration
    {
        string BaseUrl { get; set; }
        string Name { get; set; }
        string Version { get; set; }
    }
}