using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FrogAutonomy : MonoBehaviour
{
    //random sound is picked for ribbiting
    public AudioClip[] ribbitSounds;

    //movement timings
    public float minTurnTime = 0.5f;
    public float maxTurnTime = 2.5f;
    public float minIdleTime = 0.5f;
    public float maxIdleTime = 1.5f;
    public float minCooldown = 0.4f;
    public float maxCooldown = 1.7f;
    private readonly float jumpDuration = 1.5f; //slightly longer than the jump animation

    //material to recolor
    public Material recolorMaterial;

    private FrogMovementController controller;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        //set up attributes
        controller = gameObject.GetComponent<FrogMovementController>();
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();

        //set up throwable events
        Throwable throwable = GetComponent<Throwable>();
        throwable.onPickUp.AddListener(this.OnGrabbed);
        throwable.onDetachFromHand.AddListener(this.OnReleased);

        //randomize color
        if (recolorMaterial != null)
        {   
            Color color = Random.ColorHSV(0.5f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f); //randomly pick a new for color
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) //apply it to every match with the recolor material
            {
                if (renderer.sharedMaterial == this.recolorMaterial)
                {
                    renderer.material.color = color;
                }
            }
        }

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
        this.animator.SetTrigger("Crawl");
        this.animator.speed = 3.0f;
    }

    private void OnReleased()
    {
        this.animator.SetTrigger("Idle");
        this.animator.speed = 1.0f;
        StartCoroutine(Idle(Random.Range(this.minIdleTime, this.maxIdleTime)));
    }
}
