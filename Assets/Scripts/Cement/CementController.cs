using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CementEffect;

public class CementController : MonoBehaviour
{
    [SerializeField] CementMovement cementMovement;
    [SerializeField] Transform pourCenterPoint;
    [SerializeField] Transform pourBotPoint;
    [SerializeField] Transform pourTopPoint;
    [SerializeField] Transform liquid;
    [SerializeField] Transform spreadPrefab;
    [SerializeField] GameObject liquidBlock;
    [SerializeField] List<MeshRenderer> meshRenderers;
    [SerializeField] List<Material> materials;
    [SerializeField] float cementSize;

    private enum MaterialType
    {
        cement = 0,
        water = 1
    }

    private bool isPouring = false;
    public bool IsPouring
    {
        get
        {
            return isPouring;
        }
        set
        {
            isPouring = value;
        }
    }

    private bool isStartPour = false;
    public bool IsStartPour
    {
        get
        {
            return isStartPour;
        }
        set
        {
            isStartPour = value;
        }
    }

    private bool isProcess = false;
    private float targetPos = 0;
    private float targetScale = 1;
    private float targetOffside = .01f;
    private float scaleOffside = .02f;
    private float positionOffside = .5f;
    private float speed = .5f;
    private float smoothTime = .5f;
    private int materialCount;
    private Vector3 velocityScale;
    private Vector3 velocityPos;
    private Vector3 defaultLiquidPos;
    private Vector3 defaultLiquidScale;
    private MaterialType materialType;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        defaultLiquidPos = liquid.localPosition;
        defaultLiquidScale = liquid.localScale;

        liquid.gameObject.SetActive(false);
        cementMovement.SetCementComponent(new CementComponent
        {
            pourCenterPoint = this.pourCenterPoint,
            pourBotPoint = this.pourBotPoint,
            pourTopPoint = this.pourTopPoint,
            cementSize = this.cementSize,
            liquidBlock = this.liquidBlock,
            spreadPrefab = this.spreadPrefab
        });
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(IsPouring && IsStartPour)
        {
            LiquidPour(targetPos, targetScale);
        }
        else if(!IsPouring && IsStartPour)
        {
            LiquidPour(-defaultLiquidPos.x, defaultLiquidScale.x);
        }
    }

    void LiquidPour(float targetPos, float targetScale)
    {
        liquid.localScale = new Vector3(Mathf.SmoothDamp(liquid.localScale.x, targetScale + targetOffside, ref velocityScale.y, smoothTime, speed), liquid.localScale.y, liquid.localScale.z);
        liquid.localPosition = new Vector3(Mathf.SmoothDamp(liquid.localPosition.x, targetPos - targetOffside, ref velocityPos.y, smoothTime, speed*defaultLiquidPos.x), defaultLiquidPos.y, defaultLiquidPos.z);

        if(targetPos != this.targetPos)
        {
            if(liquid.localPosition.x <= -(defaultLiquidPos.x - positionOffside) && !isProcess)
            {
                SetLiquidDefault();
                cementMovement.SetGroundedState(true);
                cementMovement.CementPour();
            }
            else if(liquid.localScale.x <= this.targetScale && isProcess)
            {
                float liquidStartScale = defaultLiquidScale.x + scaleOffside;
                if(liquid.localScale.x <= liquidStartScale)
                {
                    SetLiquidDefault();
                }
            }
        }
        else
        {
            CementStartPouring();
            if(liquid.localPosition.x <= (targetPos + positionOffside))
            {
                SetLiquidDefault(false);
                cementMovement.CementPour();
            }
        }
    }

    void SetLiquidDefault(bool isDefault = true)
    {
        IsStartPour = false;
        isProcess = false;
        
        if(isDefault)
        {
            liquid.localPosition = defaultLiquidPos;
            liquid.localScale = defaultLiquidScale;
            liquid.gameObject.SetActive(false);
        }
    }

    void CementStartPouring()
    {
        if(!liquid.gameObject.activeInHierarchy)
        {
            liquid.localPosition = defaultLiquidPos;
            liquid.localScale = defaultLiquidScale;
        }

        isProcess = true;
        liquid.gameObject.SetActive(true);
    }

    public void SetMaterial()
    {
        if(!liquidBlock.activeInHierarchy && !liquid.gameObject.activeInHierarchy)
        {
            materialCount++;

            if(materialCount > materials.Count - 1)
            {
                materialCount = 0;
            }

            ChangeMaterial(materialCount);
        }
    }

    void ChangeMaterial(int index)
    {
        foreach (var item in meshRenderers)
        {
            item.material = materials[index];
        }
    }
}
