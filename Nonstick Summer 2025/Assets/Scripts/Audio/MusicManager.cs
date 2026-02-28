using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private EventInstance HouseBGM;
    private EventInstance CombatBGM;
    private EventInstance ReflectionBGM;
    private EventInstance TitleBGM;

    public static MusicManager instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one MusicManager in the scene");
        }
        instance = this;

        HouseBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.HouseBGM);
        CombatBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.CombatBGM);
        ReflectionBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.ReflectionBGM);
        TitleBGM = AudioManager.instance.CreateEventInstance(FMODEvents.instance.TitleBGM);

        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            StartTitle();
            return;
        }
        if (SceneManager.GetActiveScene().name.Equals("Tutorial"))
        {
            StartReflection();
            return;
        }

        StartReflection();
    }

    public void StartCombat(float val)
    {
        StopAll();

        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            StartReflection();
            return;
        }

        CombatBGM.setParameterByName("WhichBoss", val);
        float isItWorkingChat;
        CombatBGM.getParameterByName("WhichBoss", out isItWorkingChat);
        //Debug.Log(isItWorkingChat);
        CombatBGM.start();
    }

    public void StartReflection()
    {
        StopAll();
        ReflectionBGM.start();
    }

    public void StartTitle()
    {
        StopAll();
        TitleBGM.start();
    }

    public void StartHouse()
    {
        StopAll();
        HouseBGM.start();
    }

    private void StopAll()
    {
        HouseBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        CombatBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ReflectionBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        TitleBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnDisable()
    {
        HouseBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        CombatBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ReflectionBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        TitleBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    /*IEnumerator MusicToBattle(float val)
    {
        val += 0.05f;
        HomeBGM.setParameterByName("Battle", val);
        yield return new WaitForSecondsRealtime(0.1f);
        if (val < 1)
        {
            StartCoroutine(MusicToBattle(val));
        }
    }
    IEnumerator MusicToNormal(float val)
    {
        val -= 0.05f;
        HomeBGM.setParameterByName("Battle", val);
        yield return new WaitForSecondsRealtime(0.1f);
        if (val > 0)
        {
            StartCoroutine(MusicToNormal(val));
        }
    }*/
}
