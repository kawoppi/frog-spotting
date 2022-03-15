using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAutonomy : MonoBehaviour
{
    private FrogMovementController controller;

    // Start is called before the first frame update
    void Start()
    {
        //this.controller = GetComponent<FrogMovementController>();
        //this.MoveCompleted();
        //this.MoveForward();
    }

    void Awake()
    {
        controller = gameObject.GetComponent<FrogMovementController>();
        Debug.Log(controller);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        bool jump = Input.GetButtonDown("Jump");

        this.controller.ForwardInput = vertical;
        this.controller.TurnInput = horizontal;
        this.controller.JumpInput = jump;
        */
    }

    private void MoveCompleted()
    {
        
    }

    private void MoveForward()
    {
        this.controller.ForwardInput = 1.0f;
        Debug.Log("starting");
        //StartCoroutine(Wait(3.0f));
        this.controller.ForwardInput = 0.0f;
        Debug.Log("Stopping");
    }

    private void MoveLeft()
    {

    }

    private void MoveRight()
    {

    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
