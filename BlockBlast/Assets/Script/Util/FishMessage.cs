using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IFishMessageData
{

}

public class FishMessageData<T> : IFishMessageData
{
    public UnityAction<T> MessageEvents;

    public FishMessageData(UnityAction<T> action)
    {
        MessageEvents += action;
    }
}

public class FishMessage : FishSingleton<FishMessage>
{
    private Dictionary<string, IFishMessageData> dictionaryMessage;

    public FishMessage()
    {
        InitData();
    }

    private void InitData()
    {
        dictionaryMessage = new Dictionary<string, IFishMessageData>();
    }

    public void Register<T>(string key, UnityAction<T> action)
    {
        if (dictionaryMessage.TryGetValue(key, out var previousAction))
        {
            if (previousAction is FishMessageData<T> messageData)
            {
                messageData.MessageEvents += action;
            }
        }
        else
        {
            dictionaryMessage.Add(key, new FishMessageData<T>(action));
        }
    }

    public void Unregister<T>(string key, UnityAction<T> action)
    {
        if (dictionaryMessage.TryGetValue(key, out var previousAction))
        {
            if (previousAction is FishMessageData<T> messageData)
            {
                messageData.MessageEvents -= action;
            }
        }
    }

    public void Send<T>(string key, T data)
    {
        if (dictionaryMessage.TryGetValue(key, out var previousAction))
        {
            (previousAction as FishMessageData<T>)?.MessageEvents.Invoke(data);
        }
    }

    public void Clear()
    {
        dictionaryMessage.Clear();
    }
}
