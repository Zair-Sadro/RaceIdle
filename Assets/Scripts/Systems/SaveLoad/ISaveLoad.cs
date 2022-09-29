public interface ISaveLoad<T>
{
    public T GetData();
    public void Initialize(T data);
}
