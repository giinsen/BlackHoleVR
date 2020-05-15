using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { NEUTRAL, ATTRACT, EJECT }
    private State _state;
    private State state { get { return _state; } set { _state = value; playerModel.SetState(value); } }

    public OVRHand rightHand;
    public OVRHand leftHand;
    private OVRHand currentHand;
    private OVRInput.Button currentButton;
    public GameObject cam;

    [Header("Black Hole Position")]
    public float distanceFromCenter;
    public float moveSpeed;
    public float offsetUp;

    [Header("Ejection")]
    public float movablesBeforeEjection;
    public float delayBetweenExplusion;
    public float randomEjectionDirection;
    public float explusionForce;
    public float durationAnimationBeforeEjection;
    private bool ejectionPhase = false;

    [Header("Attraction")]
    public float attractForce;
    public float velocityMultiplierOnStartAttraction;
    public AnimationCurve attractCurve;

    [HideInInspector] private List<Movable> movablesToAttract = new List<Movable>();
    [HideInInspector] public List<Movable> movablesAbsorbed = new List<Movable>();

    private bool canAttract = false;
    private bool canMove = false;

   
    private PlayerModel playerModel;
    private Planets planets;

    
    void Start()
    {
        playerModel = GetComponentInChildren<PlayerModel>();
        planets = GetComponentInChildren<Planets>();
        state = State.NEUTRAL;
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
        
        //call events on the currentHand
        if (currentHand != null && currentHand.IsTracked && OVRInput.GetDown(currentButton) && !ejectionPhase)//A
        {
            OnHandClosed();
        }
        if (currentHand != null && currentHand.IsTracked && OVRInput.GetUp(currentButton) && !ejectionPhase)//A
        {
            OnHandOpened();
        }

        //move and attract
        transform.rotation = Quaternion.LookRotation(transform.position);
        if (currentHand != null && currentHand.IsTracked && canMove)
        {
            Vector3 h = currentHand.transform.position + Vector3.up * offsetUp;// + (hand.gameObject.transform.forward * -0.2f);
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
        }
    }

    public void OnAttractZoneEnter(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null) return;

        if (!movablesToAttract.Contains(m) && m.canBeAttracted)
            movablesToAttract.Add(m);
    }
    
    public void OnAttractZoneExit(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null) return;

        if (movablesToAttract.Contains(m))
        {
            m.StopAttraction();
            movablesToAttract.Remove(m);
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
    }

    public void Absorbtion(Movable movable)
    {
        if (movablesToAttract.Contains(movable))
            movablesToAttract.Remove(movable);

        movablesAbsorbed.Add(movable);
        movable.StopAttraction();

        transform.DOScale(Vector3.one * 0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);
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
                switch (m.movableType)
                {
                    case Movable.MovableType.GRAVITY:
                        direction = planets.planetGravityDirection;
                        break;
                    case Movable.MovableType.NOGRAVITY:
                        direction = planets.planetNoGravityDirection;
                        break;
                }
                m.EjectFromPlayer(direction);
                playerModel.AnimationEjectMovable(delayBetweenExplusion);
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
