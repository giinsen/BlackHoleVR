using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private Player player;
    public Movable.MovableType _movableTypeToAttract;
    private Movable.MovableType movableTypeToAttract { get { return _movableTypeToAttract; } set { _movableTypeToAttract = value; SetObjectiveMaterial(); } }
    public float attractForce;
    public float repulseForce;

    private MeshRenderer mesh;
    private List<Movable> movablesAttracted = new List<Movable>();

    [Header("Galaxy")]
    public GameObject sun;
    public GameObject blackHole;
    public GameObject galaxyParticles;
    public Vector3 rotationAxis;
    public GameObject planetsGalaxy;
    private float rotationSpeed;
    public float galaxyRotationSpeed;
    public float galaxyAttractedRotationSpeed;
    private int maxPlanetsInGalaxy = 8;
    public float minScaleAttract;
    private float startScaleAttract;
    private float difScaleAttract;
    public float durationAttract;


    [HideInInspector] public bool isAttracted = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        rotationSpeed = galaxyRotationSpeed;
        mesh = GetComponent<MeshRenderer>();
        movableTypeToAttract = Movable.MovableType.NONE;
        startScaleAttract = planetsGalaxy.transform.localScale.x;
        difScaleAttract = startScaleAttract - minScaleAttract;

        HidePlanetsGalaxy();
    }

    private void Update()
    {
        planetsGalaxy.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

        if (isAttracted)
        {
            float x = (difScaleAttract * Time.deltaTime) / durationAttract;
            float s = planetsGalaxy.transform.localScale.x - x;
            planetsGalaxy.transform.localScale = new Vector3(s, s, s);

            if (planetsGalaxy.transform.localScale.x <= minScaleAttract)
            {
                EndAttract();
            }
        }
    }

    private void EndAttract() 
    {
        foreach(Movable m in movablesAttracted)
        {
            if (!m.isAbsorbed)
            {
                m.Absorption();
                player.Absorbtion(m);           
            }        
        }

        HidePlanetsGalaxy();
        movableTypeToAttract = Movable.MovableType.NONE;
        movablesAttracted.Clear();
        //player.movablesAbsorbed.AddRange(movablesAttracted);
    }

    private void HidePlanetsGalaxy()
    {
        for (int i = 0; i < planetsGalaxy.transform.childCount; i++)
        {
            planetsGalaxy.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Movable m = other.gameObject.GetComponent<Movable>();
        if (m == null) return;

        if (movablesAttracted.Count == 0)
            movableTypeToAttract = m.movableType;

        if (m.movableType == movableTypeToAttract && movablesAttracted.Count < maxPlanetsInGalaxy)
        {
            movablesAttracted.Add(m);
            m.GetComponent<Rigidbody>().isKinematic = true;
            Transform pl = planetsGalaxy.transform.GetChild(movablesAttracted.Count - 1);
            m.OnEnterObjective(this, pl.position);
            pl.gameObject.SetActive(true);
        }
        else
        {
            m.OnExpulseObjective(this);
        }          
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    Movable m = other.gameObject.GetComponent<Movable>();
    //    if (m == null) return;

    //    if (m.movableType == movableTypeToAttract && movablesAttracted.Contains(m))
    //    {
    //        movablesAttracted.Remove(m);
    //        m.OnExitObjective(this);
    //    }

    //    if (movablesAttracted.Count == 0)
    //        movableTypeToAttract = Movable.MovableType.NONE;
    //}

    private void SetObjectiveMaterial()
    {
        Material mObjective = ObjectiveManager.Instance.neutralObjectiveMaterial;
        Material mPlanetsGalaxy = ObjectiveManager.Instance.neutralObjectiveMaterial;
        switch (movableTypeToAttract)
        {
            case Movable.MovableType.GRAVITY:
                mObjective = ObjectiveManager.Instance.gravityObjectiveMaterial;
                mPlanetsGalaxy = ObjectiveManager.Instance.gravityPlanetMaterial;
                break;
            case Movable.MovableType.NOGRAVITY:
                mObjective = ObjectiveManager.Instance.noGravityObjectiveMaterial;
                mPlanetsGalaxy = ObjectiveManager.Instance.noGravityPlanetMaterial;
                break;
            case Movable.MovableType.NONE:
                mObjective = ObjectiveManager.Instance.neutralObjectiveMaterial;
                break;
        }

        mesh.material = mObjective;

        for (int i = 0; i < planetsGalaxy.transform.childCount; i++)
        {
            planetsGalaxy.transform.GetChild(i).GetComponent<MeshRenderer>().material = mPlanetsGalaxy;
        }
    }

    public void StartAttraction()
    {
        rotationSpeed = galaxyAttractedRotationSpeed;
        isAttracted = true;
    }

    public void StopAttraction()
    {
        planetsGalaxy.transform.localScale = new Vector3(startScaleAttract, startScaleAttract, startScaleAttract);
        rotationSpeed = galaxyRotationSpeed;
        isAttracted = false;
    }
}