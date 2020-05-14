using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// coucou lol
public class PlayerModel : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material neutralMaterial;
    public Material attractMaterial;
    public Material ejectMaterial;

    public GameObject particlesNeutral;
    public GameObject particlesAttract;
    public GameObject particlesEject;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Movable m = collision.gameObject.GetComponent<Movable>();
        if (m == null) return;
        GetComponentInParent<Player>().Absorbtion(m);
        m.Absorption();
    }

    public void SetState(Player.State state)
    {
        switch (state)
        {
            case Player.State.NEUTRAL:
                SetMaterial(neutralMaterial);
                break;
            case Player.State.ATTRACT:
                SetMaterial(attractMaterial);
                break;
            case Player.State.EJECT:
                SetMaterial(ejectMaterial);
                break;
        }
        SetElements(state);
    }


    public void SetMaterial(Material m)
    {
        meshRenderer.material = m;
    }

    public void SetElements(Player.State state)
    {
        switch (state)
        {
            case Player.State.NEUTRAL:
                particlesNeutral.SetActive(true);
                particlesAttract.SetActive(false);
                particlesEject.SetActive(false);
                break;
            case Player.State.ATTRACT:
                particlesNeutral.SetActive(false);
                particlesAttract.SetActive(true);
                particlesEject.SetActive(false);
                break;
            case Player.State.EJECT:
                particlesNeutral.SetActive(false);
                particlesAttract.SetActive(false);
                particlesEject.SetActive(true);
                break;
        }
    }

    public void AnimationBeforeEjection(float time)
    {
        Vector3 startScale = transform.localScale;
        float timePart1 = time * 0.95f;
        float timePart2 = time * 0.03f;
        float timePart3 = time * 0.02f;

        transform.DOScale(startScale * 1.2f, timePart1 / 100f).SetLoops(100, LoopType.Yoyo);
        transform.DOShakePosition(timePart1);

        transform.DOScale(startScale * 0.3f, timePart2).SetDelay(timePart1);

        transform.DOScale(startScale, timePart3).SetDelay(timePart1 + timePart2).SetEase(Ease.InOutBounce);
    }

    public void AnimationEjectMovable(float time)
    {
        Vector3 startScale = transform.localScale;
        transform.DOScale(startScale * 1.15f, time/3).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce);
        transform.DOShakePosition(time / 6);
    }
}