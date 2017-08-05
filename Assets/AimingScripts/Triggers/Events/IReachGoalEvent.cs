using UnityEngine;
using UnityEngine.EventSystems;

public interface IReachGoalEvent : IEventSystemHandler
{
    void OnReached(GameObject player);
}