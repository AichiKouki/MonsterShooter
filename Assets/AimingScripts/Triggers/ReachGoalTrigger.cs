using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReachGoalTrigger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roots;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var player = other.gameObject;
        ExecuteEvents.Execute<IReachGoalEvent>(player, null, (x, y) => x.OnReached(player));
        roots.ForEach(go => ExecuteEventsHelper.ExecuteInChildren<IReachGoalEvent>(go, null, (x, y) => x.OnReached(player)));
    }
}