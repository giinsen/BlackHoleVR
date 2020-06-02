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
    private Material currentMaterial;

    void Start()
    {
        player = FindObjectOfType<Player>();
        delayStartScale = player.delayStartScale;
        mesh = GetComponent<SkinnedMeshRenderer>();
        currentMaterial = neutralMaterial;
    }

    void Update()
    {
        if (mesh.material != currentMaterial)
        {
            Material[] mats = mesh.materials;
            mats[0] = currentMaterial;
            mesh.materials = mats;
        }
    }


    public void FillControlHand(float delayStartScaleTmp)
    {
        delayStartScaleImage.fillAmount = delayStartScaleTmp / delayStartScale;
        delayStartScaleImage2.fillAmount = delayStartScaleTmp / delayStartScale;
    }

    public void ActiveControlScaleHand()
    {
        Debug.Log("black material");
        currentMaterial = controlScaleMaterial;
    }

    public void ResetFillControlHand()
    {
        Debug.Log("white material");
        currentMaterial = neutralMaterial;
        delayStartScaleImage.fillAmount = 0;
        delayStartScaleImage2.fillAmount = 0;
    }
}
