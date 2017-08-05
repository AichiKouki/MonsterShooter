using UnityEngine;

public class GameOverActorHandler : MonoBehaviour, IGameOverEvent
{
    private Animator Animator { get; set; }
    private MovingActor Actor { get; set; }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Actor = GetComponent<MovingActor>();
    }

    public void OnGameOver(GameObject player)
    {
        if (player != gameObject)
            return;
        Actor.Enabled = false;
        Animator.SetTrigger("Dead");
    }
}
