using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    public Material neutralObjectiveMaterial;
    public Material noGravityObjectiveMaterial;
    public Material gravityObjectiveMaterial;

    public Material noGravityPlanetMaterial;
    public Material gravityPlanetMaterial;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }
}
