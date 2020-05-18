using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    private Player player;
    private float delayStartScale;
    public Canvas canvas;
    public Image delayStartScaleImage;
    public Image delayStartScaleImage2;

    private SkinnedMeshRenderer mesh;

    public Material neutralMaterial;
    public Material controlPositionMaterial;
    public Material controlScaleMaterial;

    void Start()
    {
        player = FindObjectOfType<Player>();
        delayStartScale = player.delayStartScale;
        mesh = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        
    }

    public void FillControlHand(float delayStartScaleTmp)
    {
        delayStartScaleImage.fillAmount = delayStartScaleTmp / delayStartScale;
        delayStartScaleImage2.fillAmount = delayStartScaleTmp / delayStartScale;
    }

    public void ActiveControlHand()
    {
        mesh.material = controlScaleMaterial;
    }

    public void ResetFillControlHand()
    {
        mesh.material = neutralMaterial;
        delayStartScaleImage.fillAmount = 0;
        delayStartScaleImage2.fillAmount = 0;
    }
}
