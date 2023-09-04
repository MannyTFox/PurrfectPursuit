using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGroup : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();

    public void PlayParticleGroup()
    {
        foreach (ParticleSystem particle in particles)
        {
            if (particle.isPlaying == false)
            {
                particle.Play();
            }
        }
    }

    public void StopParticleGroup()
    {
        foreach (ParticleSystem particle in particles)
        {
            if (particle.isPlaying == true)
            {
                particle.Stop();
            }
        }
    }
}
