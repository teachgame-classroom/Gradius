using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage = 10;
    public bool isBarrier;
    public bool isLaser;
    public int laserCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("子弹打中了" + collision.gameObject.name);

        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy != null)
        {
            // 打中的是敌人
            enemy.Hurt(damage);

            if(isLaser == false)
            {
                if(isBarrier == false)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                laserCount--;

                if(laserCount == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
