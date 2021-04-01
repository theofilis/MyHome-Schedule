namespace MyHome.Dashboard.Configuration
{
    public class ApiConfiguration : IApiConfiguration
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string BaseUrl { get; set; }
    }
}
