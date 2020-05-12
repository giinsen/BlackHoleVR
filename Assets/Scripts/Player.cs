﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { NEUTRAL, ATTRACT, EJECT }
    private State _state;
    private State state { get { return _state; } set { _state = value; playerModel.SetState(value); } }

    public OVRHand hand;
    public GameObject controller;
    public GameObject cam;
    public float moveSpeed;
    public float offsetUp;

    private bool canAttract = false;

    private List<Movable> movablesToAttract = new List<Movable>();
    private List<Movable> movablesAbsorbed = new List<Movable>();


    private bool ejectionPhase = false;

    public float attractForce;
    
    private PlayerModel playerModel;

    public float durationAnimationBeforeEjection;
    void Start()
    {
        playerModel = GetComponentInChildren<PlayerModel>();
        state = State.NEUTRAL;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position);

        if (hand.IsTracked)
        {
            Vector3 h = controller.transform.position + Vector3.up * offsetUp;// + (hand.gameObject.transform.forward * -0.2f);
            Vector3 v = (h - cam.transform.position).normalized;
            transform.position = Vector3.Lerp(transform.position, v * 29, moveSpeed * Time.deltaTime);
        }
        
        //TEMPORAIRE
        if (hand.IsTracked && OVRInput.GetDown(OVRInput.Button.One) && !ejectionPhase)//A
        {
            Attract();
        }
        if (hand.IsTracked && OVRInput.GetUp(OVRInput.Button.One) && !ejectionPhase)//A
        {
            StopAttract();
        }
        //TEMPORAIRE STOP

        if (hand.IsTracked && canAttract && !ejectionPhase)
        {
            foreach (Movable m in movablesToAttract)
            {
                if (!m.isAttracted)
                {
                    m.attractForce = attractForce;
                    m.StartAttraction();
                }
            }
        }

        if (movablesAbsorbed.Count >= 10 && !ejectionPhase)
        {
            EjectMovables();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null) return;

        if (!movablesToAttract.Contains(m) && m.canBeAttracted)
            movablesToAttract.Add(m);
    }
    
    private void OnTriggerExit(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null) return;

        if (movablesToAttract.Contains(m))
        {
            m.StopAttraction();
            movablesToAttract.Remove(m);
        }
    }

    public void Attract()
    {
        canAttract = true;
        state = State.ATTRACT;
    }

    public void StopAttract()
    {
        canAttract = false;
        foreach (Movable m in movablesToAttract)
        {
            m.StopAttraction();
        }
        if (!ejectionPhase)
            state = State.NEUTRAL;
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
        StartCoroutine(_EjectMovables());
        state = State.EJECT;
    }

    private IEnumerator _EjectMovables()
    {
        ejectionPhase = true;
        playerModel.GetComponent<Collider>().enabled = false;
        StopAttract();

        yield return StartCoroutine(AnimationBeforeEjection());

        bool movablesAbsorbedEmpty = false;
        while (!movablesAbsorbedEmpty)
        {
            List<Movable> tmp = new List<Movable>(movablesAbsorbed);
            foreach (Movable m in tmp)
            {
                m.EjectFromPlayer();
                playerModel.AnimationEjectMovable(0.2f);
                yield return new WaitForSeconds(0.2f);
            }

            movablesAbsorbed.RemoveRange(0, tmp.Count);
            if (movablesAbsorbed.Count == 0)
                movablesAbsorbedEmpty = true;
        }
        
        movablesAbsorbed.Clear();
        ejectionPhase = false;
        playerModel.GetComponent<Collider>().enabled = true;
        state = State.NEUTRAL;
    }

    private IEnumerator AnimationBeforeEjection()
    {
        playerModel.AnimationBeforeEjection(durationAnimationBeforeEjection);
        yield return new WaitForSeconds(durationAnimationBeforeEjection);
    }
}
