namespace ElasticSearch.Api.Options;
#nullable disable

public class ElasticConfig
{
    public List<HostConfig> Hosts { get; set; }
}

public class HostConfig
{
    public string Url { get; set; }
    public string Alias { get; set; }
    public List<IndexConfig> Indexes { get; set; }
}

public class IndexConfig
{
    public string Index { get; set; }
    public string Alias { get; set; }
    public bool EnableVerboseLogging { get; set; }
}
