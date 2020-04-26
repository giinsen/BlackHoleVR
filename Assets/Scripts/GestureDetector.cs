using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingersData;
    public UnityEvent onRecognized;
}
public class GestureDetector : MonoBehaviour
{
    public float threshold;
    public UnityEvent onNoGesture;
    private bool onNoGestureInvoked = true;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;

    private void Awake()
    {
        
    }
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveGesture();

        Gesture currentGesture = Recognize();
        Debug.Log(currentGesture.name);
  
        if (!currentGesture.Equals(new Gesture()) && !currentGesture.Equals(previousGesture))
        {
            //Debug.Log("currentGesture.name : " + currentGesture.name);
            currentGesture.onRecognized.Invoke();
            previousGesture = currentGesture;
            onNoGestureInvoked = false;
        }

        if (currentGesture.Equals(new Gesture()) && !onNoGestureInvoked)
        {
            //no current gesture
            onNoGesture.Invoke();
            onNoGestureInvoked = true;
            previousGesture = new Gesture();
        }
    }

    void SaveGesture()
    {
        Gesture gesture = new Gesture();
        gesture.name = "Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (OVRBone bone in fingerBones)
        {
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        gesture.fingersData = data;
        gestures.Add(gesture);
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach(Gesture gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingersData[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }
        return currentGesture;     
    }
}
