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
    public string violinUp;
    [EventRef]
    public string violinUpRight;
    [EventRef]
    public string violinRight;
    [EventRef]
    public string violinDownRight;
    [EventRef]
    public string violinDown;
    [EventRef]
    public string violinDownLeft;
    [EventRef]
    public string violinLeft;
    [EventRef]
    public string violinUpLeft;

    private EventInstance textureInstance;
    private EventInstance violinUpInstance;
    private EventInstance violinUpRightInstance;
    private EventInstance violinRightInstance;
    private EventInstance violinDownRightInstance;
    private EventInstance violinDownInstance;
    private EventInstance violinDownLeftInstance;
    private EventInstance violinLeftInstance;
    private EventInstance violinUpLeftInstance;

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
        violinUpInstance = RuntimeManager.CreateInstance(violinUp);
        violinUpRightInstance = RuntimeManager.CreateInstance(violinUpRight);
        violinRightInstance = RuntimeManager.CreateInstance(violinRight);
        violinDownRightInstance = RuntimeManager.CreateInstance(violinDownRight);
        violinDownInstance = RuntimeManager.CreateInstance(violinDown);
        violinDownLeftInstance = RuntimeManager.CreateInstance(violinDownLeft);
        violinLeftInstance = RuntimeManager.CreateInstance(violinLeft);
        violinUpLeftInstance = RuntimeManager.CreateInstance(violinUpLeft);

        textureInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinUpInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinUpRightInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinRightInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinDownRightInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinDownInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinDownLeftInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinLeftInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
        violinUpLeftInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform.position));
    }

    public void StartTexture()
    {
        textureInstance.start();        
    }

    private Vector3 oldDirection = new Vector3();
    private enum DirectionState { LEFT, UPLEFT, UP, UPRIGHT, RIGHT, DOWNRIGHT, DOWN, DOWNLEFT};
    private DirectionState directionState;
    private DirectionState currentDirectionState;
    private List<DirectionState> directionStateList = new List<DirectionState>();
    private EventInstance currentViolin;
    public void ViolinFromDirection(Vector3 direction)
    {
        Vector3 norm = (direction - oldDirection).normalized;

        if (norm.x < -0.75f)
            directionState = DirectionState.LEFT;
        else if (norm.x > 0.75f)
            directionState = DirectionState.RIGHT;
        else if (norm.y > 0.75f)
            directionState = DirectionState.UP;
        else if (norm.y < -0.75f)
            directionState = DirectionState.DOWN;
        else if (norm.x < -0.25f && norm.x > -0.75f)
        {
            if (norm.y > 0)
                directionState = DirectionState.UPLEFT;
            else if (norm.y < 0)
                directionState = DirectionState.DOWNLEFT;
        }
        else if (norm.x > 0.25f && norm.x < 0.75f)
        {
            if (norm.y > 0)
                directionState = DirectionState.UPRIGHT;
            else if (norm.y < 0)
                directionState = DirectionState.DOWNRIGHT;
        }

        directionStateList.Add(directionState);
        if (directionStateList.Count > 5)
        {
            directionStateList.RemoveAt(0);
            bool equals = true;
            DirectionState d = directionStateList[0];
            foreach (DirectionState state in directionStateList)
            {
                if (state.ToString() != d.ToString())
                    equals = false;
            }

            if (equals && directionState != currentDirectionState)
                ChangeViolinSound(directionState);
        }
        oldDirection = direction;
    }

    private IEnumerator ReduceVolumeCurrentViolin()
    {
        float f = 1;
        DOTween.To(() => f, x => f = x, 0, 0.2f);
        while (f > 0.1f)
        {
            currentViolin.setParameterValue("Volume", f);
            yield return new WaitForEndOfFrame();
        }
        currentViolin.setParameterValue("Volume", 0);
    }

    private void ChangeViolinSound(DirectionState d)
    {
        
        UnityEngine.Debug.Log(d.ToString());

        //StartCoroutine(ReduceVolumeCurrentViolin());
        currentViolin.setParameterValue("Volume", 0);

        switch (d)
        {
            case DirectionState.LEFT:
                currentViolin = violinLeftInstance;
                break;
            case DirectionState.UPLEFT:
                currentViolin = violinUpLeftInstance;
                break;
            case DirectionState.UP:
                currentViolin = violinUpInstance;
                break;
            case DirectionState.UPRIGHT:
                currentViolin = violinUpRightInstance;
                break;
            case DirectionState.RIGHT:
                currentViolin = violinRightInstance;
                break;
            case DirectionState.DOWNRIGHT:
                currentViolin = violinDownRightInstance;
                break;
            case DirectionState.DOWN:
                currentViolin = violinDownInstance;
                break;
            case DirectionState.DOWNLEFT:
                currentViolin = violinDownLeftInstance;
                break;
        }
        currentDirectionState = d;
        currentViolin.start();
        currentViolin.setParameterValue("Volume", 1);
    }
}