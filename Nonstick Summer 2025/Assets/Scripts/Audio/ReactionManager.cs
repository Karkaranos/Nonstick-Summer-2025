using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ReactionManager : MonoBehaviour
{
    public EventInstance CharacterReactions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.CharacterReactGenericSFX);
    }

    public void PlayReaction(int val)
    {
        CharacterReactions.setParameterByName("Reactions", val);
        CharacterReactions.start();
    }
}
