using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    protected int optionLevel;

    protected abstract float fireInterval { get; }
    protected float lastFireTime;

    protected GameObject bulletPrefab;
    protected Transform[] shotPosTrans;

    public Weapon(int bulletPrefabIndex, Transform[] shotPosTrans)
    {
        string bulletPrefabName = "Prefabs/Bullets/Bullet_" + bulletPrefabIndex;
        bulletPrefab = Resources.Load<GameObject>(bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
    }

    public Weapon(string bulletPrefabName, Transform[] shotPosTrans)
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/" + bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
    }

    public void TryShoot()
    {
        if(CanFire())
        {
            Shoot();
        }
    }

    public bool CanFire()
    {
        if(Time.time - lastFireTime > fireInterval)
        {
            lastFireTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Shoot()
    {
        Debug.Log("ShotPos Count:" + shotPosTrans.Length + ", Option Level:" + optionLevel);

        for(int i = 0; i < shotPosTrans.Length; i++)
        {

            if(i <= optionLevel)
            {
                Debug.Log("Shoot:" + i);
                Shoot(shotPosTrans[i]);
            }
        }
        //GameObject.Instantiate(bulletPrefab, shotPosTrans.position, shotPosTrans.rotation);
    }

    protected virtual void Shoot(Transform shotPos)
    {
        GameObject.Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);
    }

    public void PowerOption()
    {
        if(optionLevel < 2)
        {
            optionLevel++;
            Debug.Log("Option Level:" + optionLevel);
        }
    }
}
