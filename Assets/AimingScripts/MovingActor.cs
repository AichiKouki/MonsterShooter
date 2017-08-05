using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovingActor : MonoBehaviour
{
    [SerializeField]
    private float movingTurnSpeed = 360;
    [SerializeField]
    private float stationaryTurnSpeed = 180;
    [SerializeField]
    private float groundCheckDistance = 0.25f;
    [SerializeField]
    private float moveSpeedMultiplier = 0.1f;
    [SerializeField]
    private float jumpPower = 6;
    [SerializeField]
    private float animSpeedMultiplier = 1f;
    [Range(1f, 5f)]
    [SerializeField]
    private float gravityMultiplier = 2f;
    
    private Animator Animator { get; set; }
    private Rigidbody Rigidbody { get; set; }
    private float OrigGroundCheckDistance { get; set; }
    private float Speed { get; set; }

    public bool IsGrounded { get; private set; }
    public bool Enabled { get; set; }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        OrigGroundCheckDistance = groundCheckDistance;
        Enabled = true;
    }

    public void Move(Vector3 move, bool jump)
    {
        if (!Enabled)
            move = Vector3.zero;
        if (move.magnitude > 1f)
            move.Normalize();

        var groundNormal = CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, groundNormal);
        var turnAmount = (Mathf.Abs(move.x) < float.Epsilon) ?
            0 :
            Vector3.Angle(transform.forward, new Vector3(move.x, 0, 0)) * Mathf.Deg2Rad;
        var forwardAmount = move.x;
        Speed = move.magnitude;

        ApplyExtraTurnRotation(forwardAmount, turnAmount);

        if (IsGrounded)
            GroundedMovement(move, jump);
        else
            AirborneMovement();

        UpdateAnimator(move);
    }

    private Vector3 CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
        {
            IsGrounded = true;
            return hitInfo.normal;
        }

        IsGrounded = false;
        return Vector3.up;
    }

    private void ApplyExtraTurnRotation(float forwardAmount, float turnAmount)
    {
        var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    private void AirborneMovement()
    {
        if (Rigidbody == null)
            return;

        var extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        Rigidbody.AddForce(extraGravityForce);

        groundCheckDistance = Rigidbody.velocity.y < 0 ? OrigGroundCheckDistance : 0.01f;
    }

    private void GroundedMovement(Vector3 move, bool jump)
    {
        if (Rigidbody == null)
            return;
        if (!IsGrounded)
            return;

        var v = Rigidbody.velocity;
        if (jump)
        {
            v.y = jumpPower;
            IsGrounded = false;
            groundCheckDistance = 0.1f;
        }
        else
        {
            v = (move * moveSpeedMultiplier) / Time.deltaTime;
            v.y = Rigidbody.velocity.y;
        }
        
        Rigidbody.velocity = v;
    }

    private void UpdateAnimator(Vector3 move)
    {
        if (Animator == null)
            return;

        Animator.SetFloat("Speed", Speed);
        Animator.SetBool("OnGround", IsGrounded);

        if (IsGrounded && move.magnitude > 0)
        {
            Animator.speed = animSpeedMultiplier;
        }
        else
        {
            Animator.speed = 1;
        }
    }
}
