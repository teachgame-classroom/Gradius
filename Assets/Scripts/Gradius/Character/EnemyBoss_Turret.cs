using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Turret : EnemyBossPart
{
    protected override void InitWeapon()
    {
        base.InitWeapon();

        currentWeapon = new BossCannon(shotPosTrans, false);
    }
}
