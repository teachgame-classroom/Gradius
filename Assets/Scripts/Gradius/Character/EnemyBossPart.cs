using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossPart : EnemyBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {

    }

    protected override void LoadDieEffect()
    {
        dieEffect = Resources.Load<GameObject>("Prefabs/Effects/LoopExplosion");
    }

    public override void Die()
    {
        DoBeforeDie();
    }
}
