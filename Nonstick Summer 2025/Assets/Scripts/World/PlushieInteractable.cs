using UnityEngine;

public class PlushieInteractable : MonoBehaviour, IInteractable
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void Interact(GameObject player)
    {
        if (particleSystem != null)
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop(false);
            }

            particleSystem.Play(false);
        }
    }
}
