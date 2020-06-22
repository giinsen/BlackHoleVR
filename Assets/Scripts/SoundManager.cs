using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using EventInstance = FMOD.Studio.EventInstance;
using RuntimeManager = FMODUnity.RuntimeManager;

public class SoundManager : MonoBehaviour
{
    public GameObject player;
    [EventRef]
    public string texture;
    [EventRef]
    public string violinMoving;
    [EventRef]
    public string transition1;
    [EventRef]
    public string transition2;
    [EventRef]
    public string transition3;
    [EventRef]
    public string ejection;
    [EventRef]
    public string enterObjective;
    [EventRef]
    public string tension;


    private EventInstance textureInstance;
    private EventInstance violinMovingInstance;
    private EventInstance transition1Instance;
    private EventInstance transition2Instance;
    private EventInstance transition3Instance;
    private EventInstance ejectionInstance;
    private EventInstance enterObjectiveInstance;
    private EventInstance tensionInstance;


    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        textureInstance = RuntimeManager.CreateInstance(texture);
        violinMovingInstance = RuntimeManager.CreateInstance(violinMoving);
        transition1Instance = RuntimeManager.CreateInstance(transition1);
        transition2Instance = RuntimeManager.CreateInstance(transition2);
        transition3Instance = RuntimeManager.CreateInstance(transition3);
        ejectionInstance = RuntimeManager.CreateInstance(ejection);
        enterObjectiveInstance = RuntimeManager.CreateInstance(enterObjective);
        tensionInstance = RuntimeManager.CreateInstance(tension);


        textureInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinMovingInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        transition1Instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        transition2Instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        transition3Instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        ejectionInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        enterObjectiveInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        tensionInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
    }

    public void StartTexture()
    {
        textureInstance.start();
    }

    public void StopTexture()
    {
        textureInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StartMoving()
    {
        violinMovingInstance.start();
    }

    public void StopMoving()
    {
        violinMovingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StartTransition1()
    {
        transition1Instance.start();
    }

    public void StartTransition2()
    {
        transition2Instance.start();
    }

    public void StartTransition3()
    {
        transition3Instance.start();
    }

    public void StartEjection()
    {
        //ejectionInstance.start();
    }

    public void StartEnterObjective()
    {
        enterObjectiveInstance.start();
    }

    public void StartTension()
    {
        tensionInstance.start();
    }
}