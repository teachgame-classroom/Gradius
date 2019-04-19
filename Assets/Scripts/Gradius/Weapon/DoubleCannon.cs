using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCannon : Weapon
{
    protected override float fireInterval
    {
        get { return 0.2f; }
    }

    public DoubleCannon(Transform[] shotPosTrans) : base(0, shotPosTrans)
    {

    }

    protected override void Shoot(Transform shotPos)
    {
        base.Shoot(shotPos);
        GameObject.Instantiate(bulletPrefab, shotPos.position, Quaternion.Euler(0, 0, 45));        
    }
}
