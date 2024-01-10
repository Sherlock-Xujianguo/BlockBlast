using UnityEngine;
using UnityEngine.Events;

public class FishMonoClass : MonoBehaviour
{
    protected void RegisterMessage<T>(string key, UnityAction<T> action)
    {
        FishMessage.GetInstnace.Register(key, action);
    }

    protected void UnregisterMessage<T>(string key, UnityAction<T> action)
    {
        FishMessage.GetInstnace.Unregister(key, action);
    }

    protected void SendMessage<T>(string key, T data)
    {
        FishMessage.GetInstnace.Send(key, data);
    }
}