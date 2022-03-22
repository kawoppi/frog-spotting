﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FrogAutonomy : MonoBehaviour
{
    //random sound is picked for ribbiting
    public AudioClip[] ribbitSounds;

    //movement timings
    public readonly float minTurnTime = 0.5f;
    public readonly float maxTurnTime = 2.5f;
    public readonly float minIdleTime = 0.5f;
    public readonly float maxIdleTime = 1.5f;
    public readonly float minCooldown = 0.4f;
    public readonly float maxCooldown = 1.7f;
    private readonly float jumpDuration = 1.5f; //slightly longer than the jump animation

    private FrogMovementController controller;
    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody frogBody;

    void Start()
    {
        //set up attributes
        controller = gameObject.GetComponent<FrogMovementController>();
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        this.frogBody = GetComponent<Rigidbody>();

        //set up throwable events
        Throwable throwable = GetComponent<Throwable>();
        throwable.onPickUp.AddListener(this.OnGrabbed);
        throwable.onDetachFromHand.AddListener(this.OnReleased);

        //start with a jump as the first movement
        StartCoroutine(MoveForward(this.jumpDuration));
    }

    private void MoveCompleted()
    {
        //pick a random move and do a coroutine to execute it
        int action = Random.Range(0, 4);
        switch (action)
        {
            case 0:
                StartCoroutine(MoveForward(this.jumpDuration));
                break;
            case 1:
                StartCoroutine(MoveLeft(Random.Range(this.minTurnTime, this.maxTurnTime)));
                break;
            case 2:
                StartCoroutine(MoveRight(Random.Range(this.minTurnTime, this.maxTurnTime)));
                break;
            case 3:
                StartCoroutine(Idle(Random.Range(this.minIdleTime, this.maxIdleTime)));
                break;
        }
    }

    IEnumerator MoveForward(float time)
    {
        this.animator.applyRootMotion = true;
        this.animator.SetTrigger("Jump");

        yield return new WaitForSeconds(time);

        this.animator.applyRootMotion = false;
        this.animator.SetTrigger("Idle");

        this.MoveCompleted();
    }

    IEnumerator MoveLeft(float time)
    {
        yield return new WaitForSeconds(Random.Range(this.minCooldown, this.maxCooldown));

        this.controller.TurnInput = -1.0f;
        this.animator.SetTrigger("Crawl");

        yield return new WaitForSeconds(time);

        this.controller.TurnInput = 0.0f;
        this.animator.SetTrigger("Idle");

        this.MoveCompleted();
    }

    IEnumerator MoveRight(float time)
    {
        yield return new WaitForSeconds(Random.Range(this.minCooldown, this.maxCooldown));

        this.controller.TurnInput = 1.0f;
        this.animator.SetTrigger("Crawl");

        yield return new WaitForSeconds(time);

        this.controller.TurnInput = 0.0f;
        this.animator.SetTrigger("Idle");

        this.MoveCompleted();
    }

    IEnumerator Idle(float time)
    {
        this.audioSource.PlayOneShot(this.ribbitSounds[Random.Range(0, this.ribbitSounds.Length)]); //play random ribbit sound
        yield return new WaitForSeconds(time);
        this.MoveCompleted();
    }

    private void OnGrabbed()
    {
        StopAllCoroutines();
        this.controller.TurnInput = 0.0f;
        this.animator.applyRootMotion = false;
        this.frogBody.freezeRotation = false;
        this.animator.SetTrigger("Crawl");
    }

    private void OnReleased()
    {
        this.frogBody.freezeRotation = true;
        this.animator.SetTrigger("Idle");
        StartCoroutine(Idle(Random.Range(this.minIdleTime, this.maxIdleTime)));
    }
}
