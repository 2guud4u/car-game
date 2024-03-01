using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayEffect(GameObject particle, Vector3 particlePosition, float duration)
    {
        StartCoroutine(PlayingParticleEffect(particle, particlePosition, duration));
    }

    // plays effect at the position of the torso of the current game object
    // currently not working, effect doesn't play and location is off
    IEnumerator PlayingParticleEffect(GameObject particle, Vector3 particlePosition, float duration)
    {
        if (particle != null){
            GameObject newParticleEffect = Instantiate(particle, particlePosition, Quaternion.identity);
            ParticleSystem effect = newParticleEffect.GetComponent<ParticleSystem>();
            StartEffects(effect);

            yield return new WaitForSeconds(duration); 
            
            StopEffects(effect);
            Destroy(newParticleEffect);
        }
    }

    public void StartEffects(ParticleSystem effect)
    {
        effect.Play();
    }

    public void StopEffects(ParticleSystem effect)
    {
        effect.Stop();
    }
}
