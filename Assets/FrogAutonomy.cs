using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAutonomy : MonoBehaviour
{
    public AudioClip[] ribbitSounds;
    public float minTurnTime = 0.5f;
    public float maxTurnTime = 2.5f;
    public float minIdleTime = 0.5f;
    public float maxIdleTime = 1.5f;
    public float minCooldown = 0.4f;
    public float maxCooldown = 1.7f;

    private FrogMovementController controller;
    private Animator animator;
    private AudioSource audioSource;
    private float jumpDuration;

    void Start()
    {
        controller = gameObject.GetComponent<FrogMovementController>();
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        StartCoroutine(MoveForward(1.0f));

        //get the duration of the jump animation
        AnimationClip[] clips = this.animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if (clip.name == "Jump")
            {
                this.jumpDuration = clip.length;
            }
        }
    }

    private void MoveCompleted()
    {
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
        this.audioSource.PlayOneShot(this.ribbitSounds[Random.Range(0, this.ribbitSounds.Length)]);

        yield return new WaitForSeconds(time);

        this.MoveCompleted();
    }
}
