using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCannon : GuidedWeapon
{
    protected override float fireInterval { get { return 2; } }

    public BossCannon(Transform[] shotPosTrans, bool isPlayerWeapon) : base(12, shotPosTrans, "Player", isPlayerWeapon)
    {

    }
}
