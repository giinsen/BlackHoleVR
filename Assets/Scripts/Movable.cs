using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private Rigidbody rb;
    [HideInInspector] public bool isAttracted;
    [HideInInspector] public float attractForce;
    [HideInInspector] public Player player;

    private bool isAbsorbed = false;
    private bool isUsingGravity;

    public bool canBeAttracted = true;
    public float startForce;
    public float domeCollisionForce;
    public GameObject domeCollisionParticles;

    public Vector2 randomScaleRange;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>();
        isUsingGravity = rb.useGravity;
        rb.AddForceAtPosition(Vector3.one * startForce, Vector3.zero);

        float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        rb.mass = randomScale;
    }

    void Update()
    {
        if (isAttracted)
            rb.AddForce((player.transform.position - transform.position).normalized * attractForce, ForceMode.Impulse);

        if (isAbsorbed)
            transform.position = player.transform.position;
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

    public void EjectFromPlayer()
    {
        isAbsorbed = false;
        GetComponent<MeshRenderer>().enabled = true;
        SetUseGravity(true);
        Vector3 dir = -player.transform.position.normalized;
        Vector3 random = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        rb.AddForce(dir * 30 + random * 8, ForceMode.Impulse);
        StartCoroutine(EnableCollider());
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider>().enabled = true;      
    }

    public void SetUseGravity(bool useGravity)
    {
        if (isUsingGravity)
            rb.useGravity = useGravity;
    }
}
