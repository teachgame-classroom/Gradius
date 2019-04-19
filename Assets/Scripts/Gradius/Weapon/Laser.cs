using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    protected override float fireInterval
    {
        get { return 0.5f; }
    }

    public Laser(Transform[] shotPosTrans) : base(1, shotPosTrans)
    {

    }
}