using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    protected override float fireInterval
    {
        get { return 0.5f; }
    }

    public Laser(Transform[] shotPosTrans, bool isPlayerWeapon) : base(1, shotPosTrans, isPlayerWeapon)
    {

    }
}