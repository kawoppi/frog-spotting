using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        this.particles = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = collider.transform.position;
        this.particles.Emit(emitParams, 1);
    }
}
