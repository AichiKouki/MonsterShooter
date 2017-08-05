using UnityEngine;

public class ThirdPersonUserController : MonoBehaviour
{
    [SerializeField]
    private MovingActor actor;

    private bool Jump { get; set; }
    
    private void Start()
    {
        if (actor == null)
            actor = GetComponent<MovingActor>();
    }

    private void Update()
    {
        if (!Jump)
            Jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var move = v * Vector3.back + h * Vector3.left;
        actor.Move(move, Jump);
        Jump = false;
    }
}
