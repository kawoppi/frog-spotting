using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    Rigidbody frogBody;
    public float force = 40.0f;
    public float turnForce = 10.0f;
    Animator animator;
    int jumpHash = Animator.StringToHash("Jump");

    // Start is called before the first frame update
    void Start()
    {
        this.frogBody = GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            this.Jump();
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            this.TurnRight();
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            this.TurnLeft();
        }
    }

    private void Jump()
    {
        this.frogBody.AddForce((transform.up * this.force) + (transform.forward * this.force));
        this.animator.SetTrigger(this.jumpHash);
    }

    private void TurnRight()
    {
        this.frogBody.AddTorque(transform.up * this.turnForce);
    }

    private void TurnLeft()
    {
        this.frogBody.AddTorque(transform.up * -this.turnForce);
    }
}
