namespace MP.View.Interfaces
{
    public interface IObservable<T>
    {
        void Register(T instance);
        void Remove(T instance);
    }

    public interface IObserver
    {
        void Update(bool state);
    }
}

