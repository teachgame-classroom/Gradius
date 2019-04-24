using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCannon : Weapon
{
    protected override float fireInterval
    {
        get { return 0.2f; }
    }

    public DoubleCannon(Transform[] shotPosTrans, bool isPlayerWeapon) : base(0, shotPosTrans, isPlayerWeapon)
    {

    }

    protected override void Shoot(Transform shotPos)
    {
        base.Shoot(shotPos);

        GameObject extraBullet = bulletPool.Get(shotPos.position, Quaternion.Euler(0, 0, 45));
        extraBullet.GetComponent<BulletMove>().moveDirection = extraBullet.transform.right;
        //GameObject.Instantiate(bulletPrefab, shotPos.position, Quaternion.Euler(0, 0, 45));        
    }
}
