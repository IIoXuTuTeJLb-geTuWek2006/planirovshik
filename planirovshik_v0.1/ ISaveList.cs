namespace planirovshik_v0._1
{
    public interface ISaveList<T>
    {
        T Load(string path);
        void Save(T data, string path);
    }
}
