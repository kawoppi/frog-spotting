using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashCollider : MonoBehaviour
{
    public GameObject attachedTo;
    public int splashCount = 10;
    private ParticleSystem particles;
    private AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        this.particles = GetComponent<ParticleSystem>();
        this.AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WaterSurface")
        {
            this.particles.Emit(splashCount);
            AudioSource.Play();
        }
    }

    private void LateUpdate()
    {
        //take the same position as the attached object
        //this does not change the rotation
        transform.position = this.attachedTo.transform.position;
    }
}
