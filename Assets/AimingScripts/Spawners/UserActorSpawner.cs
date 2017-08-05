using UnityEngine;

public class UserActorSpawner : Spawner
{
    [SerializeField]
    private FollowTarget mainCamera;
    
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main.GetComponent<FollowTarget>();
        
        var go = Spawn();
        go.AddComponent<ThirdPersonUserController>();
        go.AddComponent<GameOverActorHandler>();
        go.tag = "Player";
        mainCamera.Target = go.transform;
    }
}