using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using static UnityEngine.Rendering.DebugUI;

public class MusicManager : MonoBehaviour
{
    private EventInstance HomeBGM;

    public static MusicManager instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one MusicManager in the scene");
        }
        instance = this;

        HomeBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.HomeBGM);
        HomeBGM.start();
    }

    public void TransitionMusic(bool isNormal)
    {
        if (isNormal)
        {
            StartCoroutine(MusicToBattle(0));
        }
        else
        {
            StartCoroutine(MusicToNormal(1));
        }
    }

    IEnumerator MusicToBattle(float val)
    {
        val += 0.05f;
        HomeBGM.setParameterByName("Battle", val);
        yield return new WaitForSecondsRealtime(0.2f);
        if (val < 1)
        {
            StartCoroutine(MusicToBattle(val));
        }
    }

    IEnumerator MusicToNormal(float val)
    {
        val -= 0.05f;
        HomeBGM.setParameterByName("Battle", val);
        yield return new WaitForSecondsRealtime(0.2f);
        if (val > 0)
        {
            StartCoroutine(MusicToNormal(val));
        }
    }
}
