using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
