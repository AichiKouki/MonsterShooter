using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, 10f);
    [SerializeField]
    private float smoothTime = 0.25f;
    private Vector3 currentVelocity;
    
    //public Transform Target { get; set; }
	public Transform Target;
    
    private void LateUpdate()
    {
        var targetPosition = Target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
