using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Laser : EnemyBossPart
{
    protected override void InitWeapon()
    {
        base.InitWeapon();

        currentWeapon = new BossLaser(shotPosTrans, false);
    }
}
