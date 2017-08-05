using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roots;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var player = other.gameObject;
        ExecuteEvents.Execute<IGameOverEvent>(player, null, (x, y) => x.OnGameOver(player));
        roots.ForEach(go => ExecuteEventsHelper.ExecuteInChildren<IGameOverEvent>(go, null, (x, y) => x.OnGameOver(player)));
    }
}