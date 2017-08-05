using UnityEngine;
using UnityEngine.EventSystems;

public interface IGameOverEvent : IEventSystemHandler
{
    void OnGameOver(GameObject player);
}