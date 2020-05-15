using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public enum MovableType { GRAVITY, NOGRAVITY }
    public MovableType movableType;

    private Rigidbody rb;
    [HideInInspector] public Player player;
    [HideInInspector] public bool isAttracted;

    private bool isAbsorbed = false;
    private bool isUsingGravity;

    public bool canBeAttracted = true;
    public float startForce;
    public float domeCollisionForce;
    public GameObject domeCollisionParticles;

    public Vector2 randomScaleRange;

    private Objective currentObjective;
    private bool isAttractedObjective = false;
    

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>();
        isUsingGravity = rb.useGravity;

        Vector3 randomStartForce = new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb.AddForce(randomStartForce * startForce, ForceMode.Impulse);

        float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        rb.mass = randomScale;
    }

    void Update()
    {
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

        if (isAttractedObjective)
        {
            rb.AddForce((currentObjective.transform.position - transform.position).normalized * currentObjective.attractForce, ForceMode.Impulse);
        }
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
    }

    public void StartAttraction()
    {
        SetUseGravity(false);
        rb.velocity = rb.velocity * player.velocityMultiplierOnStartAttraction;
        rb.AddTorque(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
        isAttracted = true;
    }

    public void Absorption()
    {
        GetComponent<Collider>().enabled = false;
        SetUseGravity(false);
        StartCoroutine(_Absorption());
    }

    public IEnumerator _Absorption()
    {
        Vector3 startScale = transform.localScale;
        transform.DOMove(player.transform.position, 0.5f);
        transform.DOScale(Vector3.zero, 0.5f);

        yield return new WaitForSeconds(1f);

        GetComponent<MeshRenderer>().enabled = false;
        isAbsorbed = true;
        transform.localScale = startScale;
    }

    public void EjectFromPlayer(Vector3 direction)
    {
        isAbsorbed = false;
        GetComponent<MeshRenderer>().enabled = true;
        SetUseGravity(true);
        //Vector3 dir = -player.transform.position.normalized;
        Vector3 random = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Collider>().enabled = true;

        rb.AddForce(direction * player.explusionForce + random * player.randomEjectionDirection, ForceMode.Impulse);

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

    public void OnEnterObjective(Objective o)
    {
        isAttractedObjective = true;
        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        currentObjective = o;
    }

    public void OnExitObjective(Objective o)
    {
        isAttractedObjective = false;
        currentObjective = null;
    }
}
