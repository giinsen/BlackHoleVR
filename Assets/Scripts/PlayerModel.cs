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

    public GameObject planets;

    public GameObject neutralSmall;
    public GameObject neutralNormal;
    public GameObject neutralBig;
    public GameObject neutralHuge;

    public GameObject attractSmall;
    public GameObject attractNormal;
    public GameObject attractBig;
    public GameObject attractHuge;

    public GameObject ejectSmall;
    public GameObject ejectNormal;
    public GameObject ejectBig;
    public GameObject ejectHuge;

    private Player player;
    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Movable m = collision.gameObject.GetComponent<Movable>();
        if (m == null) return;
        player.Absorbtion(m);
        m.Absorption();
    }

    public void SetScale(Player.ScaleState scaleState)
    {
        neutralSmall.SetActive(false);
        neutralNormal.SetActive(false);
        neutralBig.SetActive(false);
        neutralHuge.SetActive(false);

        attractSmall.SetActive(false);
        attractNormal.SetActive(false);
        attractBig.SetActive(false);
        attractHuge.SetActive(false);

        ejectSmall.SetActive(false);
        ejectNormal.SetActive(false);
        ejectBig.SetActive(false);
        ejectHuge.SetActive(false);

        switch (scaleState)
        {
            case Player.ScaleState.SMALL:
                neutralSmall.SetActive(true);
                attractSmall.SetActive(true);
                ejectSmall.SetActive(true);
                player.transform.DOScale(new Vector3(player.smallScale, player.smallScale, player.smallScale), player.timeChangeScale);
                break;
            case Player.ScaleState.NORMAL:
                neutralNormal.SetActive(true);
                attractNormal.SetActive(true);
                ejectNormal.SetActive(true);
                player.transform.DOScale(new Vector3(player.normalScale, player.normalScale, player.normalScale), player.timeChangeScale);
                break;
            case Player.ScaleState.BIG:
                neutralBig.SetActive(true);
                attractBig.SetActive(true);
                ejectBig.SetActive(true);
                player.transform.DOScale(new Vector3(player.bigScale, player.bigScale, player.bigScale), player.timeChangeScale);
                break;
            case Player.ScaleState.HUGE:
                neutralHuge.SetActive(true);
                attractHuge.SetActive(true);
                ejectHuge.SetActive(true);
                player.transform.DOScale(new Vector3(player.hugeScale, player.hugeScale, player.hugeScale), player.timeChangeScale);
                break;
        }
    }
    public IEnumerator SetState(Player.State state)
    {
        float f = player.currentScale;
        player.transform.DOScale(Vector3.zero, 0.2f);
        switch (state)
        {
            case Player.State.NEUTRAL:
                SoundManager.Instance.StartTransition1();
                SoundManager.Instance.StopMoving();
                SetMaterial(neutralMaterial);
                break;
            case Player.State.ATTRACT:
                SoundManager.Instance.StartMoving();
                SoundManager.Instance.StartTransition2();
                SetMaterial(attractMaterial);
                break;
            case Player.State.EJECT:
                SoundManager.Instance.StartTransition3();
                SoundManager.Instance.StopMoving();
                SetMaterial(ejectMaterial);
                break;
        }

        StartCoroutine(SetElements(state));

        yield return new WaitForSeconds(0.2f);

        player.transform.DOScale(new Vector3(f,f,f), 0.2f);

        planets.GetComponent<Planets>().SetState(state);
    }


    public void SetMaterial(Material m)
    {
        meshRenderer.material = m;
    }

    public IEnumerator SetElements(Player.State state)
    {
        //particlesNeutral.transform.DOScale(Vector3.zero, 0.2f);
        //particlesAttract.transform.DOScale(Vector3.zero, 0.2f);
        //particlesEject.transform.DOScale(Vector3.zero, 0.2f);

        yield return new WaitForSeconds(0.0f);

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

        //particlesNeutral.transform.DOScale(player.currentScale, 0.2f);
        //particlesAttract.transform.DOScale(player.currentScale, 0.2f);
        //particlesEject.transform.DOScale(player.currentScale, 0.2f);
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