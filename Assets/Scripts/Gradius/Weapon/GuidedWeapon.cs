using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedWeapon : Weapon
{
    public float searchRange = 10f;
    protected string targetTag;

    protected override float fireInterval
    {
        get { return 0.2f; }
    }

    public GuidedWeapon(int bulletPrefabIdx, Transform[] shotPosTrans, string targetTag, bool isPlayerWeapon) : base(bulletPrefabIdx, shotPosTrans, isPlayerWeapon)
    {
        this.targetTag = targetTag;
    }

    protected override void Shoot(Transform shotPos)
    {
        Vector3 pos;

        if(FindTargetPosition(targetTag, out pos))
        {
            SetAimDirection(shotPos, pos);
            base.Shoot(shotPos);
        }
    }

    protected void SetAimDirection(Transform shotPos, Vector3 targetPosition)
    {
        Vector3 aimDirection;
        aimDirection = (targetPosition - shotPos.position).normalized;
        shotPos.right = aimDirection;
    }

    protected virtual bool FindTargetPosition(string targetTag, out Vector3 targetPos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(shotPosTrans[0].position, searchRange);

        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].tag == targetTag)
            {
                targetPos = colliders[i].transform.position;
                return true;
            }
        }

        targetPos = Vector3.zero;
        return false;
    }
}
