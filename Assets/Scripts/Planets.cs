using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    private Player player;
    public GameObject planetGravity;
    public GameObject planetNoGravity;

    public float rotationSpeed;


    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        transform.Rotate(player.transform.forward, Time.deltaTime * rotationSpeed);
    }

    private void OnEnable()
    {
        Debug.Log("ok");
    }
}
