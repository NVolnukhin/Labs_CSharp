public interface IDataWriter<T>
{
    void WriteData(string filePath, IEnumerable<T> data);
}