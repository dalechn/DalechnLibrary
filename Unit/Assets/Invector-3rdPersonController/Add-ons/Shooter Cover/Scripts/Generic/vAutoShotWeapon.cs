using Invector.vShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vAutoShotWeapon : vShooterWeaponBase
{
    
    protected virtual void Update()
    {
       if(this.enabled) Shoot();
    }
}
