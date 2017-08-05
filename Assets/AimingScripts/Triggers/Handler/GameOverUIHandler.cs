using UnityEngine;

public class GameOverUIHandler : MonoBehaviour, IGameOverEvent
{
    private TweenUIPosition tween;
    
    private void Start()
    {
        tween = GetComponent<TweenUIPosition>();
    }

    public void OnGameOver(GameObject player)
    {
        tween.Play();
    }
}
