using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovementController : MonoBehaviour
{
    [Tooltip("Maximum slope the character can jump on")]
    [Range(5f, 60f)]
    public float slopeLimit = 45f;
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed = 2f;
    [Tooltip("Turn speed in degrees/second, left (+) or right (-)")]
    public float turnSpeed = 300;
    [Tooltip("Whether the character can jump")]
    public bool allowJump = false;
    [Tooltip("Upward speed to apply when jumping in meters/second")]
    public float jumpSpeed = 4f;

    public bool IsGrounded { get; private set; }
    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }
    public bool JumpInput { get; set; }

    private Rigidbody frogRigidbody;
    private CapsuleCollider capsuleCollider;
    private Animator animator;

    void Awake()
    {
        frogRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        this.animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        ProcessActions();
    }

    /// <summary>
    /// Checks whether the character is on the ground and updates <see cref="IsGrounded"/>
    /// </summary>
    private void CheckGrounded()
    {
        IsGrounded = false;
        float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        Vector3 capsuleBottom = transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            float normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit)
            {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    IsGrounded = true;
            }
        }
    }

    /// <summary>
    /// Processes input actions and converts them into movement
    /// </summary>
    private void ProcessActions()
    {
        // Turning
        if (TurnInput != 0f)
        {
            float angle = Mathf.Clamp(TurnInput, -1f, 1f) * turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }

        // Movement
        Vector3 move = transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) *
            moveSpeed * Time.fixedDeltaTime;
        frogRigidbody.MovePosition(transform.position + move);

        // Jump
        if (JumpInput && allowJump && IsGrounded)
        {
            frogRigidbody.AddForce(transform.up * jumpSpeed, ForceMode.VelocityChange);
        }
    }

    private void UpdateAnimation()
    {
        if (ForwardInput > 0.0f)
        {
            this.animator.SetTrigger("Jump");
        }
        else if (TurnInput != 0.0f)
        {
            if (TurnInput > 0.0f)
            {
                this.animator.SetTrigger("TurnRight");
            }
            else
            {
                this.animator.SetTrigger("TurnLeft");
            }
        }
        else
        {
            this.animator.SetTrigger("Idle");
        }
    }

}
