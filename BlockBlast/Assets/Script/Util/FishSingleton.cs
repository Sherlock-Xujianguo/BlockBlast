
public interface IInitable
{
    void Init();
}

public class FishSingleton<T> : FishSimpleClass where T : new()
{
    private static T instance;


    public static T GetInstnace
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                (instance as IInitable)?.Init();
            }
            return instance;
        }
    }
}
