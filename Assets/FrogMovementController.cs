using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovementController : MonoBehaviour
{
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed = 2f;
    [Tooltip("Turn speed in degrees/second, left (+) or right (-)")]
    public float turnSpeed = 300;

    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }

    private Rigidbody frogRigidbody;

    void Awake()
    {
        this.frogRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //turning
        if (TurnInput != 0f)
        {
            float angle = Mathf.Clamp(TurnInput, -1f, 1f) * turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }

        //moving
        Vector3 move = transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed * Time.fixedDeltaTime;
        this.frogRigidbody.MovePosition(transform.position + move);
    }

}
