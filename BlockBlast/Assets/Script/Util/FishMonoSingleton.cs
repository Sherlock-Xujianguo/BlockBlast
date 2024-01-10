using UnityEngine;


public class FishMonoSingleton<T> : FishMonoClass where T : Component
{
    private static T _instance;

    public static T GetInstance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// 继承Mono单例的类如果写了Awake方法，需要在Awake方法最开始的地方调用一次base.Awake()，来给_instance赋值
    /// </summary>
    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name, new[] { typeof(T) });
                DontDestroyOnLoad(obj);
                _instance = obj.GetComponent<T>();
                (_instance as IInitable)?.Init();
            }
            else
            {
                Debug.LogWarning("Instance is already exist!");
            }
        }
        _instance = this as T;
        DontDestroyOnLoad(this);
    }

}