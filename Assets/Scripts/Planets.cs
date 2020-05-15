using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    private Player player;
    private PlayerModel playerModel;
    public GameObject planetGravity;
    public GameObject planetNoGravity;

    public Vector3 planetGravityDirection;
    public Vector3 planetNoGravityDirection;

    public float rotationSpeed;

    private bool canRotate = false;

    void Start()
    {
        player = GetComponentInParent<Player>();
        playerModel = player.GetComponentInChildren<PlayerModel>();
    }

    void Update()
    {
        if (canRotate)
            transform.RotateAround(playerModel.transform.forward, Time.deltaTime * rotationSpeed);

        planetGravityDirection = (planetGravity.transform.position - transform.position).normalized;
        planetNoGravityDirection = (planetNoGravity.transform.position - transform.position).normalized;
    }

    public void SetState(Player.State state)
    {
        switch (state)
        {
            case Player.State.NEUTRAL:
                canRotate = true;
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

    private void OnEnable()
    {
        Debug.Log("ok");
    }
}
