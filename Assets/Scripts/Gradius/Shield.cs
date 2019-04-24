using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EnemyBossPart
{
    private Collider2D col;
    public float invincibleTime = 30;

    protected override void InitCharacter()
    {
        base.InitCharacter();
        TurnOffSprite();
        invincible = true;
        Invoke("TurnOffInvincible", invincibleTime);
    }

    protected override void LoadDieEffect()
    {
        dieEffect = Resources.Load<GameObject>("Prefabs/Bullets/EnemyBullets/Bullet_12");
    }

    protected override void PlayDieEffect()
    {
        col.enabled = false;
        spriteRenderer.enabled = true;

        GameObject bullet1 = Instantiate(dieEffect, transform.position, Quaternion.identity);
        bullet1.GetComponent<BulletMove>().moveDirection = Quaternion.Euler(0, 0, 120f) * Vector3.right;    // 四元数 * 向量 = 向量旋转后的新向量（四元数在乘号左边，向量在乘号的右边） 

        GameObject bullet2 = Instantiate(dieEffect, transform.position, Quaternion.identity);
        bullet2.GetComponent<BulletMove>().moveDirection = Quaternion.Euler(0, 0, 240f) * Vector3.right;    // 四元数 * 向量 = 向量旋转后的新向量（四元数在乘号左边，向量在乘号的右边） 
    }

    private void TurnOffInvincible()
    {
        invincible = false;
    }

}
