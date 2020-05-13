using OVRTouchSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapDetector : MonoBehaviour
{
    public OVRHand rightHand;
    public OVRHand leftHand;

    public float thresholdDistance = 0.15f;

    private Vector3 newPosRight;
    private Vector3 prevPosRight;
    private Vector3 rightHandVelocity;

    private Vector3 newPosLeft;
    private Vector3 prevPosLeft;
    private Vector3 leftHandVelocity;

    public GameObject clapFeedback;

    private bool clapInvoked = true;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        newPosRight = rightHand.transform.position;
        rightHandVelocity = (newPosRight - prevPosRight) / Time.fixedDeltaTime;
        prevPosRight =  newPosRight;

        newPosLeft = leftHand.transform.position;
        leftHandVelocity = (newPosLeft - prevPosLeft) / Time.fixedDeltaTime;
        prevPosLeft = newPosLeft;
    }

    void Update()
    {
        //Debug.Log(handVelocity);
        if (rightHand.IsTracked && leftHand.IsTracked)
        {
            if (!clapInvoked && Vector3.Distance(rightHand.transform.position, leftHand.transform.position) <= thresholdDistance
                && rightHandVelocity.x < -0.3f && leftHandVelocity.x > 0.3f)
            {
                //invokeclap
                Debug.Log(rightHand.transform.position);
                Instantiate(clapFeedback, rightHand.transform.position, Quaternion.identity);
                clapInvoked = true;
            }

            if (Vector3.Distance(rightHand.transform.position, leftHand.transform.position) > thresholdDistance*2)
            {
                clapInvoked = false;
            }
        }       
    }
}
