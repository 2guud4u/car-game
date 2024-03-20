using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public ParticleSystem LVParticleSystem;
    public ParticleSystem RVParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        LVParticleSystem.Play();
        RVParticleSystem.Play();
    }

}
