using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage = 10;
    public bool isBarrier;
    public bool isLaser;
    public int laserCount;
    
    public void OnHit()
    {
        if (isLaser == false)
        {
            if (isBarrier == false)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            laserCount--;

            if (laserCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
