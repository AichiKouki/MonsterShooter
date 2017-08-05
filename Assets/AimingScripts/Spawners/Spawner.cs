using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObject;
    [SerializeField]
    private Vector3 initializeAngle;
    
    private void Start()
    {
        Spawn();
    }

    public GameObject Spawn()
    {
        var rotation = Quaternion.Euler(initializeAngle.x, initializeAngle.y, initializeAngle.z);
        return Instantiate(spawnObject, transform.position, rotation, transform);
    }
}