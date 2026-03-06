using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.TextCore.Text;

public class ReactionManager : MonoBehaviour
{
    public EventInstance CharacterReactions;

    public static ReactionManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one ReactionManager in the scene");
        }
        instance = this;

        //CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.UncleReact);
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

    public void SetCharacter (Character thisChar)
    {
        if (thisChar == Character.Grandma)
        {
            CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.GrandmaReact);
        }
        else if (thisChar == Character.Mom)
        {
            CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.MomReact);
        }
        else if (thisChar == Character.Cousin)
        {
            CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.CousinReact);
        }
        else if (thisChar == Character.Uncle)
        {
            CharacterReactions = AudioManager.instance.CreateEventInstance(FMODEvents.instance.UncleReact);
        }
        else if (thisChar == Character.Tutorial)
        {
            Debug.Log("No tutorial voice lines.");
        }
    }
}
