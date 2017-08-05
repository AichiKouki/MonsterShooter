using UnityEngine;

public class ReachGoalUIHandler : MonoBehaviour, IReachGoalEvent
{
    private TweenUIPosition tween;
    
    private void Start()
    {
        tween = GetComponent<TweenUIPosition>();
    }

    public void OnReached(GameObject player)
    {
        tween.Play();
    }
}
