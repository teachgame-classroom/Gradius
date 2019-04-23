using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
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

    protected override void InitCharacter()
    {
        base.InitCharacter();
        if(hurtTags.Length == 0)
        {
            hurtTags = new string[] { "PlayerBullet" };
        }

        LoadDamageEffect();
    }

    protected virtual void LoadDamageEffect()
    {
        dieEffect = Resources.Load<GameObject>("Prefabs/Effects/Explosion_Red");

    }
}
