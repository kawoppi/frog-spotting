using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FrogAutonomy : MonoBehaviour
{
	[SerializeField]
	BoundingArea frighteningRange;
	[SerializeField]
	private AudioClip[] ribbitSounds; //random sound is picked for ribbiting

	//movement timings
	public float minTurnTime = 0.5f;
	public float maxTurnTime = 2.5f;
	public float minIdleTime = 0.5f;
	public float maxIdleTime = 1.5f;
	public float minCooldown = 0.4f;
	public float maxCooldown = 1.7f;
	private readonly float jumpDuration = 1.5f; //slightly longer than the jump animation

	private FrogMovementController controller;
	private Animator animator;
	private AudioSource audioSource;

	private delegate IEnumerator FrogAction();
	private FrogAction[] randomActions;

	private List<GameObject> frighteners; //only those inside of the bounding box

	void Start()
	{
		//set up attributes
		this.controller = gameObject.GetComponent<FrogMovementController>();
		this.animator = GetComponent<Animator>();
		this.audioSource = GetComponent<AudioSource>();

		//set up bounding area callbacks
		if (this.frighteningRange != null)
		{
			this.frighteningRange.enterCallbacks.Add(this.FrightenerEntered);
			this.frighteningRange.exitCallbacks.Add(this.FrightenerExited);
		}
		this.frighteners = new List<GameObject>();

		//set up throwable events
		Throwable throwable = GetComponent<Throwable>();
		throwable.onPickUp.AddListener(this.OnGrabbed);
		throwable.onDetachFromHand.AddListener(this.OnReleased);

		//set list of coroutines to pick from for random frog movement
		FrogAction[] tempActions = { MoveForward, MoveLeft, MoveRight, Idle };
		this.randomActions = tempActions;

		//start with a jump as the first movement
		StartCoroutine(MoveForward());
	}

	private void MoveCompleted()
	{
		//pick a random action and do a coroutine to execute it
		int action = Random.Range(0, this.randomActions.Length);
		StartCoroutine(this.randomActions[action]());
	}

	IEnumerator MoveForward()
	{
		this.animator.applyRootMotion = true;
		this.animator.SetTrigger("Jump");

		yield return new WaitForSeconds(this.jumpDuration);

		this.animator.applyRootMotion = false;
		this.animator.SetTrigger("Idle");

		this.MoveCompleted();
	}

	IEnumerator MoveLeft()
	{
		yield return new WaitForSeconds(Random.Range(this.minCooldown, this.maxCooldown));

		this.controller.TurnInput = -1.0f;
		this.animator.SetTrigger("Crawl");

		yield return new WaitForSeconds(Random.Range(this.minTurnTime, this.maxTurnTime));

		this.controller.TurnInput = 0.0f;
		this.animator.SetTrigger("Idle");

		this.MoveCompleted();
	}

	IEnumerator MoveRight()
	{
		yield return new WaitForSeconds(Random.Range(this.minCooldown, this.maxCooldown));

		this.controller.TurnInput = 1.0f;
		this.animator.SetTrigger("Crawl");

		yield return new WaitForSeconds(Random.Range(this.minTurnTime, this.maxTurnTime));

		this.controller.TurnInput = 0.0f;
		this.animator.SetTrigger("Idle");

		this.MoveCompleted();
	}

	IEnumerator Idle()
	{
		this.audioSource.PlayOneShot(this.ribbitSounds[Random.Range(0, this.ribbitSounds.Length)]); //play random ribbit sound
		yield return new WaitForSeconds(Random.Range(this.minIdleTime, this.maxIdleTime));
		this.MoveCompleted();
	}

	private void FrightenerEntered(GameObject frightener)
	{
		if (frightener.tag == "FrogFrightener")
		{
			this.frighteners.Add(frightener);
			Debug.Log("entered");
		}
	}

	private void FrightenerExited(GameObject frightener)
	{
		if (this.frighteners.Contains(frightener))
		{
			this.frighteners.Remove(frightener);
			Debug.Log("left");
		}
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
		StartCoroutine(Idle());
	}
}
