using UnityEngine;
using UnityEngine.EventSystems;

public static class ExecuteEventsHelper
{
    public static GameObject ExecuteInChildren<T>(GameObject root, BaseEventData eventData, ExecuteEvents.EventFunction<T> callbackFunction)
        where T : IEventSystemHandler
    {
        foreach (Transform child in root.transform)
        {
            var gameObject = child.gameObject;
            if (!ExecuteEvents.CanHandleEvent<T>(gameObject))
                continue;
            if (ExecuteEvents.Execute<T>(gameObject, eventData, callbackFunction))
                return gameObject;
        }
        return null;
    }
}