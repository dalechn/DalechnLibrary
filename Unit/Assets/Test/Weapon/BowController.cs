using CJTools;
using MegaFiers;
using UnityEngine;
using Invector.vShooter;
using Invector;

public class BowController : vShooterWeapon
{
    [vEditorToolbar("Bow")]
    public Transform springBone;
    public Transform defaultChargePos;
    public Transform powerChargePos;
    public GameObject arrow;
    public GameObject arrows;
    public float bendFactor = -50;
    public float backTime = 0.3f;
    public float minPenetration = 0.1f, maxPenetration =0.3f;

    private float springDist;
    private MegaBend bend;
    private Vector3 originBonePos;
    private bool shooting;
    private bool powerCharging;
    private Transform rootBone;

    protected override void Start()
    {
        base.Start();

        rootBone = transform;
        bend = GetComponentInChildren<MegaBend>();

        originBonePos = rootBone.InverseTransformPoint(springBone.position);

       onInstantiateProjectile.AddListener(OnProjectile);
       onFullPower.AddListener(()=> { SetPrecision(1); });
       onPowerChargerChanged.AddListener(OnPowerCharge);
        onFinishReload.AddListener(() => { EnableArrow(); });
        //onReload.AddListener(()=> { EnableArrow(); });
    }

    private void Update()
    {
        springDist = Vector3.Distance(rootBone.InverseTransformPoint(springBone.position), originBonePos);

        if(!shooting)
        {
            //Debug.Log(springDist);
            bend.angle =  bendFactor * springDist;

            if (Mathf.Abs( bend.angle)<0.01f)
            {
                bend.angle = 0;
            }

        }
    }

    protected override void HandleShot(Vector3 aimPosition)
    {
        base.HandleShot(aimPosition);

        ShootBackAni();

        SetPrecision(0.1f);
    }

    public override void SetActiveAim(bool value)
    {
        //if(value)
        //{
        //    Charge(defaultChargePos.position);
        //}

        if (isAiming != value)
        {
            isAiming = value;
            if (isAiming)
            {
                onEnableAim.Invoke();

                SetPrecision(0.1f);

                EnableArrow();
            }
            else
            {
                onDisableAim.Invoke();

                ShootBackAni();

                DisableArrow();
            }
        }

    }

    //拉弓事件
    public void OnPowerCharge(float f)
    {
        Charge(powerChargePos.position);
    }

    //生成箭头事件
    public void OnProjectile(vProjectileControl pCtrl)
    {
        DisableArrow();
        var arrow = pCtrl.GetComponent<vArrow>();
        if (arrow)
        {
            arrow.penetration = Mathf.Lerp(minPenetration, maxPenetration, powerCharge);
        }
    }

    private void EnableArrow()
    {
        if(isAiming && HasAmmo())
        {
            arrow.SetActive(true);
            arrows.SetActive(true);
        }
      
    }

    private void DisableArrow()
    {
        arrow.SetActive(false);
        arrows.SetActive(false);
    }


    private void Charge(Vector3 pos)
    {
        //if (chargeSmooth <= 0)
        //{
        //    springBone.position = pos;
        //}
        springBone.position = Vector3.Lerp(springBone.position, pos,Time.deltaTime * chargeSpeed);

    }

    private void ShootBackAni()
    {
        if (!shooting && Mathf.Abs(bend.angle) > 0)
        {
            shooting = true;
            Vector3 originPos = springBone.position;
            float originBend = bend.angle;

            bl_UpdateManager.RunAction(null, backTime, (float t, float r) =>
            {
                springBone.position = Vector3.Lerp(originPos, rootBone.TransformPoint(originBonePos), t);
                bend.angle = Mathf.Lerp(originBend, 0, t);
            }, () =>
            {
                shooting = false;
            }, EaseType.BounceOut, ActionMode.MFixedUpdate);
        }
    }
}
