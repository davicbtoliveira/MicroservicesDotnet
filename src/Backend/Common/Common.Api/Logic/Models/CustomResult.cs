namespace Common.Api.Logic.Models
{
    public class CustomResult
    {
        public bool success { get; set; }
        public object? data { get; set; }
        public IEnumerable<string>? errors { get; set; }
    }

    public class CustomResult<T>
    {
        public bool success { get; set; }
        public T? data { get; set; }
        public IEnumerable<string>? errors { get; set; }
    }

    public class CustomProxyResult<T>
    {
        public bool success { get; set; }
        public T? data { get; set; }
        public IEnumerable<string>? errors { get; set; }
    }
}
