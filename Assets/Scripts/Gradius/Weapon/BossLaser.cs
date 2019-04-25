using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : Weapon
{
    protected override float fireInterval { get { return 5; } }

    public BossLaser(Transform[] shotPosTrans, bool isPlayerWeapon) : base(11, shotPosTrans, isPlayerWeapon)
    {

    }

}
