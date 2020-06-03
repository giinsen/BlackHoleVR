using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum ScaleState { SMALL, NORMAL, BIG, HUGE }
    private ScaleState _scaleState;
    private ScaleState scaleState { get { return _scaleState; } set { _scaleState = value; playerModel.SetScale(value); } }
    public enum State { NEUTRAL, ATTRACT, EJECT }
    private State _state;
    private State state { get { return _state; } set { _state = value; playerModel.SetState(value); } }

    public OVRHand rightHand;
    private HandController rightHandController;
    public OVRHand leftHand;
    private HandController leftHandController;
    private OVRHand currentHand;
    private OVRInput.Button currentButton;
    public GameObject cam;

    [Header("Black Hole Position")]
    public float distanceFromCenter;
    public float moveSpeed;
    public float offsetUp;
    public float offsetRightRightHand;
    public float offsetRightLeftHand;
    private bool canMove = false;

    [Header("Ejection")]
    public float movablesBeforeEjection;
    public float delayBetweenExplusion;
    public float randomEjectionDirection;
    public float explusionForce;
    public float durationAnimationBeforeEjection;
    private bool ejectionPhase = false;

    [Header("Attraction")]
    public float[] attractionForceArray;
    public float velocityMultiplierOnStartAttraction;
    public AnimationCurve attractCurve;
    private bool canAttract = false;
    [HideInInspector] public float attractForce;

    [Header("Bigger/Smaller")]
    public float distanceChangeScale;
    public float delayStartScale;
    private float delayStartScaleTmp;
    public float smallScale;
    public float normalScale;
    public float bigScale;
    public float hugeScale;    
    private bool scaleInProgress = false;
    private float startDistanceBetweenHands = 0;

    [HideInInspector] private List<Movable> movablesToAttract = new List<Movable>();
    [HideInInspector] public List<Movable> movablesAbsorbed = new List<Movable>();

    [HideInInspector] private List<Objective> objectivesToAttract = new List<Objective>();

    private PlayerModel playerModel;
    private Planets planets;

    
    void Start()
    {
        playerModel = GetComponentInChildren<PlayerModel>();
        planets = GetComponentInChildren<Planets>();
        state = State.NEUTRAL;
        scaleState = ScaleState.NORMAL;
        rightHandController = rightHand.GetComponent<HandController>();
        leftHandController = leftHand.GetComponent<HandController>();
    }

    void Update()
    {
        //detect currentHand
        if (rightHand.IsTracked && OVRInput.GetDown(OVRInput.Button.One) && !OVRInput.Get(OVRInput.Button.Three))//A
        {
            currentHand = rightHand;
            currentButton = OVRInput.Button.One;
        }
        if (leftHand.IsTracked && OVRInput.GetDown(OVRInput.Button.Three) && !OVRInput.Get(OVRInput.Button.One))//Y
        {
            currentHand = leftHand;
            currentButton = OVRInput.Button.Three;
        }

        //scale player
        if (OVRInput.Get(OVRInput.Button.One) && OVRInput.Get(OVRInput.Button.Three)) //les deux mains fermés
        {
            if (!scaleInProgress)
            {
                delayStartScaleTmp += Time.deltaTime;

                if (currentHand == rightHand)
                    leftHandController.FillControlHand(delayStartScaleTmp);
                else
                    rightHandController.FillControlHand(delayStartScaleTmp);
               
                if (delayStartScaleTmp >= delayStartScale)
                {
                    if (currentHand == rightHand)
                        leftHandController.ActiveControlScaleHand();
                    else
                        rightHandController.ActiveControlScaleHand();

                    scaleInProgress = true;
                    startDistanceBetweenHands = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
                }                
            }           
        }
        else if (scaleInProgress || delayStartScaleTmp != 0) //1ere frame ou les deux mains ne sont plus ouvertes
        {
            delayStartScaleTmp = 0;
            rightHandController.ResetFillControlHand();
            leftHandController.ResetFillControlHand();
            scaleInProgress = false;
        }

        if (scaleInProgress && rightHand.IsTracked && leftHand.IsTracked)
        {
            if (Vector3.Distance(leftHand.transform.position, rightHand.transform.position) > startDistanceBetweenHands + distanceChangeScale)
            {
                startDistanceBetweenHands = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
                SetScaleState((int)scaleState + 1);
            }
            else if(Vector3.Distance(leftHand.transform.position, rightHand.transform.position) < startDistanceBetweenHands - distanceChangeScale)
            {
                startDistanceBetweenHands = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
                SetScaleState((int)scaleState - 1);
            }
        }
            
        
        //call events on the currentHand
        if (currentHand != null && currentHand.IsTracked && OVRInput.GetDown(currentButton) && !ejectionPhase)
        {
            OnHandClosed();
        }
        if (currentHand != null && currentHand.IsTracked && OVRInput.GetUp(currentButton) && !ejectionPhase)
        {
            OnHandOpened();
        }

        //move and attract
        transform.rotation = Quaternion.LookRotation(transform.position);
        if (currentHand != null && currentHand.IsTracked && canMove)
        {
            float r = 0;
            if (currentHand == rightHand)
                r = offsetRightRightHand;
            if (currentHand == leftHand)
                r = offsetRightLeftHand;

            Vector3 h = currentHand.transform.position + Vector3.up * offsetUp + Vector3.right * r;
            Vector3 v = (h - cam.transform.position).normalized;
            transform.position = Vector3.Lerp(transform.position, v * distanceFromCenter, moveSpeed * Time.deltaTime);
        }
        if (currentHand != null && currentHand.IsTracked && canAttract && !ejectionPhase)
        {
            foreach (Movable m in movablesToAttract)
            {
                if (!m.isAttracted)
                {
                    m.StartAttraction();
                }
            }
            foreach (Objective o in objectivesToAttract)
            {
                if (!o.isAttracted)
                {
                    o.StartAttraction();
                }
            }
        }
    }

    private void SetScaleState(int scaleState)
    {
        if (scaleState < 0 || scaleState >= Enum.GetNames(typeof(ScaleState)).Length) return;
        this.scaleState = (ScaleState)scaleState;
        attractForce = attractionForceArray[scaleState];
    }

    public void OnAttractZoneEnter(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m != null)
        {
            if (!movablesToAttract.Contains(m) && m.canBeAttracted)
                movablesToAttract.Add(m);
        }

        Objective o = other.gameObject.GetComponent<Objective>();
        if (o != null)
        {
            if (!objectivesToAttract.Contains(o))
                objectivesToAttract.Add(o);
        }
    }
    
    public void OnAttractZoneExit(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m != null)
        {
            if (movablesToAttract.Contains(m))
            {
                m.StopAttraction();
                movablesToAttract.Remove(m);
            }
        }

        Objective o = other.gameObject.GetComponent<Objective>();
        if (o != null)
        {
            if (objectivesToAttract.Contains(o))
            {
                o.StopAttraction();
                objectivesToAttract.Remove(o);
            }                
        }

    }

    public void OnHandClosed()
    {
        canMove = true;
        canAttract = true;
        state = State.ATTRACT;
    }

    public void OnHandOpened()
    {
        canMove = false;
        canAttract = false;
        StopAllAttraction();
        if (!ejectionPhase)
            state = State.NEUTRAL;
    }

    public void StopAllAttraction()
    {
        foreach (Movable m in movablesToAttract)
        {
            m.StopAttraction();
        }

        foreach (Objective o in objectivesToAttract)
        {
            o.StopAttraction();
        }
    }

    public void Absorbtion(Movable movable)
    {
        if (movablesToAttract.Contains(movable))
            movablesToAttract.Remove(movable);

        movablesAbsorbed.Add(movable);
        movable.StopAttraction();
    }

    public void EjectMovables()
    {
        if (ejectionPhase) 
            return;
        StartCoroutine(_EjectMovables());
        state = State.EJECT;
    }

    private IEnumerator _EjectMovables()
    {
        ejectionPhase = true;
        playerModel.GetComponents<Collider>()[0].enabled = false;
        playerModel.GetComponents<Collider>()[1].enabled = false;
        OnHandOpened();

        yield return StartCoroutine(AnimationBeforeEjection());

        bool movablesAbsorbedEmpty = false;
        while (!movablesAbsorbedEmpty)
        {
            List<Movable> tmp = new List<Movable>(movablesAbsorbed);
            foreach (Movable m in tmp)
            {
                Vector3 direction = new Vector3();
                GameObject pl = new GameObject();
                switch (m.movableType)
                {
                    case Movable.MovableType.GRAVITY:
                        pl = planets.planetGravity;
                        direction = planets.planetGravityDirection;
                        break;
                    case Movable.MovableType.NOGRAVITY:
                        pl = planets.planetNoGravity;
                        direction = planets.planetNoGravityDirection;
                        break;
                }
                m.EjectFromPlayer(direction, pl);
                m.SetScale(scaleState);
                playerModel.AnimationEjectMovable(delayBetweenExplusion);
                planets.AnimationEjectMovable(delayBetweenExplusion, pl);
                yield return new WaitForSeconds(delayBetweenExplusion);
            }

            movablesAbsorbed.RemoveRange(0, tmp.Count);
            if (movablesAbsorbed.Count == 0)
                movablesAbsorbedEmpty = true;
        }

        movablesAbsorbed.Clear();
        ejectionPhase = false;
        playerModel.GetComponents<Collider>()[0].enabled = true;
        playerModel.GetComponents<Collider>()[1].enabled = true;
        state = State.NEUTRAL;
    }

    private IEnumerator AnimationBeforeEjection()
    {
        playerModel.AnimationBeforeEjection(durationAnimationBeforeEjection);
        yield return new WaitForSeconds(durationAnimationBeforeEjection);
    }
}
