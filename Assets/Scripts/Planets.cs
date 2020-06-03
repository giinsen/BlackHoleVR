using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    private Player player;
    private PlayerModel playerModel;
    public GameObject planetGravity;
    public GameObject planetNoGravity;

    [HideInInspector] public Vector3 planetGravityDirection;
    [HideInInspector] public Vector3 planetNoGravityDirection;

    public float rotationSpeed;
    public float scaleForOneObject;
    public float maxScale;

    private bool canRotate = false;

    void Start()
    {
        player = GetComponentInParent<Player>();
        playerModel = player.GetComponentInChildren<PlayerModel>();
        //SetPlanetsSize();
    }

    void Update()
    {
        if (canRotate)
            transform.RotateAround(playerModel.transform.forward, Time.deltaTime * rotationSpeed);

        planetGravityDirection = (planetGravity.transform.position - transform.position).normalized;
        planetNoGravityDirection = (planetNoGravity.transform.position - transform.position).normalized;
    }

    public void AnimationEjectMovable(float time, GameObject planet)
    {
        Vector3 startScale = planet.transform.localScale;
        planet.transform.DOScale(startScale * 0.2f, time / 3).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce);
        planet.transform.DOShakePosition(time / 6);
    }

    public void SetState(Player.State state)
    {
        switch (state)
        {
            case Player.State.NEUTRAL:
                canRotate = true;
                //SetPlanetsSize();
                gameObject.SetActive(true);
                break;
            case Player.State.ATTRACT:
                gameObject.SetActive(false);
                break;
            case Player.State.EJECT:
                canRotate = false;
                gameObject.SetActive(true);
                break;
        }
    }

    public void SetPlanetsSize()
    {
        float planetGravityCount = 0;
        float planetNoGravityCount = 0;
        foreach (Movable m in player.movablesAbsorbed)
        {
            if (m.movableType == Movable.MovableType.GRAVITY)
                planetGravityCount++;
            if (m.movableType == Movable.MovableType.NOGRAVITY)
                planetNoGravityCount++;
        }


        //float scaleGravity = (planetGravityCount / player.movablesAbsorbed.Count) * balancedPlanetScale;
        //float scaleNoGravity = (planetNoGravityCount / player.movablesAbsorbed.Count) * balancedPlanetScale;

        float scaleGravity = Mathf.Min(planetGravityCount * scaleForOneObject, maxScale);
        float scaleNoGravity = Mathf.Min(planetNoGravityCount * scaleForOneObject, maxScale);

        planetGravity.transform.localScale = new Vector3(scaleGravity, scaleGravity, scaleGravity);
        planetNoGravity.transform.localScale = new Vector3(scaleNoGravity, scaleNoGravity, scaleNoGravity);
    }
}
