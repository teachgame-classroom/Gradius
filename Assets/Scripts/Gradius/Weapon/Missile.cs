using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon
{
    protected int level = 0;

    protected override float fireInterval
    {
        get { return 2f; }
    }

    public Missile(Transform[] shotPosTrans) : base(2, shotPosTrans)
    {
        this.level = 0;
    }

    protected override void Shoot(Transform shotPos)
    {
        if (this.level > 0)
        {
            GameObject missileInstance = GameObject.Instantiate(bulletPrefab, shotPos.position, Quaternion.Euler(0, 0, -45));

            if (this.level > 1)
            {
                GameObject.Instantiate(bulletPrefab, shotPos.position + missileInstance.transform.right * 0.75f, Quaternion.Euler(0, 0, -45));
            }
        }
    }

    public void LevelUp()
    {
        level++;
    }
}
