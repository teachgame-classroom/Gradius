﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWeapon : Weapon
{
    protected override float fireInterval
    {
        get { return 0.15f; }
    }

    public NormalWeapon(Transform[] shotPosTrans, bool isPlayerWeapon) : base(0, shotPosTrans, isPlayerWeapon)
    {

    }
}
