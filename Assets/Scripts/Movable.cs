using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public enum MovableType { GRAVITY, NOGRAVITY, NONE }
    public MovableType movableType;

    private Rigidbody rb;
    [HideInInspector] public Player player;
    [HideInInspector] public bool isAttracted;

    public bool isAbsorbed = false;
    private bool isUsingGravity;

    public bool canBeAttracted = true;
    public float startForce;
    public float domeCollisionForce;
    public float objectiveCollisionForce;
    public GameObject domeCollisionParticles;

    private Objective currentObjective;
    private bool isAttractedObjective = false;

    //public Vector2 randomScaleRange;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>();
        isUsingGravity = rb.useGravity;

        Vector3 randomStartForce = new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb.AddForce(randomStartForce * startForce, ForceMode.Impulse);

        //float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
        //transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        //rb.mass = randomScale;
    }

    void Update()
    {

        float scaleX = Mathf.Cos(Time.time * 5f) * 0.5f + 1;
        float scaleY = Mathf.Sin(Time.time * 5f) * 0.5f + 1;
        GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));


        if (isAttracted)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 force = (player.transform.position - transform.position).normalized * player.attractForce * player.attractCurve.Evaluate(Mathf.Clamp01(distance / player.distanceFromCenter));
            rb.AddForce(force, ForceMode.Impulse);
        }

        if (isAbsorbed)
        {
            transform.position = player.transform.position;
        }

        //if (isAttractedObjective)
        //{
        //    rb.AddForce((currentObjective.transform.position - transform.position).normalized * currentObjective.attractForce, ForceMode.Impulse);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dome")
        {
            rb.AddForce(-(collision.contacts[0].point - transform.position).normalized * domeCollisionForce, ForceMode.Impulse);
            if (domeCollisionParticles != null)
                Instantiate(domeCollisionParticles, collision.contacts[0].point, Quaternion.identity);
        }
    }

    public void StopAttraction()
    {
        SetUseGravity(true);
        isAttracted = false;

        GetComponent<MeshRenderer>().material.SetFloat("_AtmosFalloff", 3f);
    }

    public void StartAttraction()
    {
        SetUseGravity(false);
        rb.velocity = rb.velocity * player.velocityMultiplierOnStartAttraction;
        rb.AddTorque(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
        isAttracted = true;

        GetComponent<MeshRenderer>().material.SetFloat("_AtmosFalloff", 1f);
    }

    public void Absorption()
    {
        GetComponent<Collider>().enabled = false;
        SetUseGravity(false);
        StartCoroutine(_Absorption());
        GetComponent<MeshRenderer>().material.SetFloat("_AtmosFalloff", 3f);
    }

    public IEnumerator _Absorption()
    {
        Vector3 startScale = transform.localScale;
        transform.DOMove(player.transform.position, 0.5f);
        transform.DOScale(Vector3.zero, 0.5f);
        isAbsorbed = true;

        yield return new WaitForSeconds(1f);

        GetComponent<MeshRenderer>().enabled = false;
        transform.localScale = startScale;
    }

    public void EjectFromPlayer(Vector3 direction, GameObject planet)
    {
        transform.position = planet.transform.position; 
        isAbsorbed = false;
        GetComponent<MeshRenderer>().enabled = true;
        SetUseGravity(true);
        Vector3 random = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        rb.AddForce(direction * player.explusionForce + random * player.randomEjectionDirection, ForceMode.Impulse);
    }

    public void SetScale(Player.ScaleState s)
    {
        float scale = 0;
        switch (s)
        {
            case Player.ScaleState.SMALL:
                scale = player.smallScale;
                break;
            case Player.ScaleState.NORMAL:
                scale = player.normalScale;
                break;
            case Player.ScaleState.BIG:
                scale = player.bigScale;
                break;
            case Player.ScaleState.HUGE:
                scale = player.hugeScale;
                break;
        }
        transform.localScale = new Vector3(scale, scale, scale);
        rb.mass = scale;
    }
    public IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<Collider>().enabled = true;      
    }

    public void SetUseGravity(bool useGravity)
    {
        if (isUsingGravity)
            rb.useGravity = useGravity;
    }

    public void OnEnterObjective(Objective o, Vector3 planetPosition)
    {
        isAttractedObjective = true;
        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        currentObjective = o;
        GetComponent<Collider>().enabled = false;
        transform.DOMove(planetPosition, 0.2f).OnComplete(() => { GetComponent<MeshRenderer>().enabled = false; });
        
    }

    public void OnExitObjective(Objective o)
    {
        isAttractedObjective = false;
        currentObjective = null;
    }

    public void OnExpulseObjective(Objective o)
    {
        rb.AddForce(-(o.transform.position - transform.position).normalized * o.repulseForce, ForceMode.Impulse);
    }
}