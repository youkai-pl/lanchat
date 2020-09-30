namespace Lanchat.Core.Models
{
    public class DataWrapper<T>
    {
        public string DataType => typeof(T).Name;
        public T Data { get; set; }

        public DataWrapper(T data)
        {
            Data = data;
        }
    }
}