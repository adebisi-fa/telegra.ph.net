namespace Telegraph.Net.Models
{
    public class TelegraphResponse<T>
    {
        public bool Ok { get; set; }
        public T Result { get; set; }
        public string Error { get; set; }
    }
}