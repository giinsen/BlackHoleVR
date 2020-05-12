using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public List<Movable.MovableType> movablesToAttract;
    public float attractForce;
    private void OnTriggerEnter(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null || !movablesToAttract.Contains(m.movableType)) return;
        m.OnEnterObjective(this);
    }
    private void OnTriggerExit(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null) return;
        m.OnExitObjective(this);
    }
}
