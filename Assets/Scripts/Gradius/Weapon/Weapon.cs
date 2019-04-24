using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public bool isPlayerWeapon;

    protected int optionLevel;

    protected abstract float fireInterval { get; }
    protected float lastFireTime;

    protected GameObject bulletPrefab;
    protected Transform[] shotPosTrans;

    protected ObjectPool bulletPool;

    public Weapon(int bulletPrefabIndex, Transform[] shotPosTrans, bool isPlayerWeapon)
    {
        string bulletPrefabPath = "Prefabs/Bullets/Bullet_" + bulletPrefabIndex;
        Init(shotPosTrans, isPlayerWeapon, bulletPrefabPath);
    }

    public Weapon(string bulletPrefabName, Transform[] shotPosTrans, bool isPlayerWeapon)
    {
        string bulletPrefabPath = "Prefabs/Bullets/" + bulletPrefabName;
        Init(shotPosTrans, isPlayerWeapon, bulletPrefabPath);
    }

    private void Init(Transform[] shotPosTrans, bool isPlayerWeapon, string bulletPrefabName)
    {
        bulletPrefab = Resources.Load<GameObject>(bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
        this.isPlayerWeapon = isPlayerWeapon;

        bulletPool = new ObjectPool(bulletPrefab, 30);
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
        //GameObject.Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);
        //GameObject instance = GameObject.Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);

        GameObject instance = bulletPool.Get(shotPos.position, shotPos.rotation);

        //instance.tag = isPlayerWeapon ? "PlayerBullet" : "EnemyBullet";
        if(isPlayerWeapon)
        {
            instance.tag = "PlayerBullet";
        }
        else
        {
            instance.tag = "EnemyBullet";
        }
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
