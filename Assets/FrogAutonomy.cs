using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAutonomy : MonoBehaviour
{
    private FrogMovementController controller;
    private Animator animator;

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
        this.animator = GetComponent<Animator>();
        StartCoroutine(MoveForward(1.0f));
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
        //yield return new WaitForSeconds(1.0f);
        int action = Random.Range(0, 3);
        switch (action)
        {
            case 0:
                StartCoroutine(MoveForward(1.0f));
                break;
            case 1:
                StartCoroutine(MoveLeft(Random.Range(0.5f, 2.5f)));
                break;
            case 2:
                StartCoroutine(MoveRight(Random.Range(0.5f, 2.5f)));
                break;
        }
    }

    IEnumerator MoveForward(float time)
    {
        this.controller.ForwardInput = 1.0f;
        this.animator.SetTrigger("Jump");

        yield return new WaitForSeconds(time);

        this.controller.ForwardInput = 0.0f;
        this.animator.SetTrigger("Idle");

        this.MoveCompleted();
    }

    IEnumerator MoveLeft(float time)
    {
        yield return new WaitForSeconds(Random.Range(0.4f, 1.7f));

        this.controller.TurnInput = -1.0f;
        this.animator.SetTrigger("Crawl");

        yield return new WaitForSeconds(time);

        this.controller.TurnInput = 0.0f;
        this.animator.SetTrigger("Idle");

        this.MoveCompleted();
    }

    IEnumerator MoveRight(float time)
    {
        yield return new WaitForSeconds(Random.Range(0.4f, 1.7f));

        this.controller.TurnInput = 1.0f;
        this.animator.SetTrigger("Crawl");

        yield return new WaitForSeconds(time);

        this.controller.TurnInput = 0.0f;
        this.animator.SetTrigger("Idle");

        this.MoveCompleted();
    }
}
