using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Core : EnemyBossPart
{
    public float invincibleTime = 30;

    protected override void InitCharacter()
    {
        base.InitCharacter();
        TurnOffSprite();
        invincible = true;
        Invoke("TurnOffInvincible", invincibleTime);
    }

    protected override void PlayHurtEffect()
    {
        spriteRenderer.enabled = true;
        Invoke("TurnOffSprite", 0.1f);
    }

    private void TurnOffInvincible()
    {
        invincible = false;
    }
}
