using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    private Player player;
    private PlayerModel playerModel;
    public GameObject planetGravity;
    public GameObject planetNoGravity;

    public float rotationSpeed;


    void Start()
    {
        player = GetComponentInParent<Player>();
        playerModel = player.GetComponentInChildren<PlayerModel>();
    }

    void Update()
    {
        transform.RotateAround(playerModel.transform.forward, Time.deltaTime * rotationSpeed);
    }

    private void OnEnable()
    {
        Debug.Log("ok");
    }
}
