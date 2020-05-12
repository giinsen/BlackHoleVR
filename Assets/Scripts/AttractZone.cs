using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractZone : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.OnAttractZoneEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        player.OnAttractZoneExit(other);
    }

}
