using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ReactionManager : MonoBehaviour
{
    public EventInstance CharacterReactions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO
        //Have a switch statement here that sets CharacterReactions to the correct designated character rection FMODEvent
        CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.UncleReact);
    }

    public void PlayReaction(int val)
    {
        //0 = neutral
        //1 = happy
        //2 = sad
        //3 = angry
        CharacterReactions.setParameterByName("Reactions", val);
        CharacterReactions.start();
    }
}
