using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    public int score = 100;
    public SquadonManager squadonManager;

    protected override string deathClipName { get { return "Sound Effect (7)"; } }

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

        LoadDieEffect();
    }

    protected virtual void LoadDieEffect()
    {
        dieEffect = Resources.Load<GameObject>("Prefabs/Effects/Explosion_Red");
    }

    protected override void Move()
    {
        if(squadonManager == null)
        {
            base.Move();
        }
    }

    protected override void DoBeforeDie()
    {
        base.DoBeforeDie();
        GameController.instance.AddScore(score);
    }
}
