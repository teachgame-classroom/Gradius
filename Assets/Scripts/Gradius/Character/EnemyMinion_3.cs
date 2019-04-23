using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMinion_3 : EnemyBase
{
    public Sprite[] turretSprites;
    public float turretMinAngle;
    public float turretMaxAngle;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Shoot();
        SetSpriteByAimDirection(shotPosTrans[0].right);
    }

    protected override void InitWeapon()
    {
        base.InitWeapon();
        currentWeapon = new GuidedWeapon(shotPosTrans, "Player", false);
    }

    void SetSpriteByAimDirection(Vector3 aimDirection)
    {
        if (turretSprites.Length == 0) return;

        float angleStep = (turretMaxAngle - turretMinAngle) / (turretSprites.Length - 1);

        float angle = Vector3.SignedAngle(Vector3.right, aimDirection, Vector3.forward) + angleStep / 2;

        angle = Mathf.Clamp(angle, turretMinAngle, turretMaxAngle);

        int spriteIdx = Mathf.Abs(Mathf.FloorToInt(angle / angleStep)) - 1;

        spriteIdx = Mathf.Clamp(spriteIdx, 0, turretSprites.Length - 1);

        //Debug.Log("Step:" + angleStep + ", Aim Angle:" + angle + ", idx:" + spriteIdx);

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = turretSprites[spriteIdx];
        }
    }

}
