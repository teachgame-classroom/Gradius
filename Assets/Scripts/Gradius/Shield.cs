using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int hp = 200;
    private GameObject bulletPrefab;

    private Collider2D col;
    private SpriteRenderer sr;

    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/EnemyBullets/Bullet_12");
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hurt(int damage)
    {
        if(Time.time - spawnTime > 30f)
        {
            if (hp > 0)
            {
                hp -= damage;

                if (hp <= 0)
                {
                    Die();
                }
            }
        }
    }

    void Die()
    {
        col.enabled = false;
        sr.enabled = true;

        GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet1.GetComponent<BulletMove>().moveDirection = Quaternion.Euler(0, 0, 120f) * Vector3.right;    // 四元数 * 向量 = 向量旋转后的新向量（四元数在乘号左边，向量在乘号的右边） 

        GameObject bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet2.GetComponent<BulletMove>().moveDirection = Quaternion.Euler(0, 0, 240f) * Vector3.right;    // 四元数 * 向量 = 向量旋转后的新向量（四元数在乘号左边，向量在乘号的右边） 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletDamage bullet = collision.GetComponent<BulletDamage>();

        if(bullet)
        {
            Hurt(bullet.damage);
            Destroy(bullet.gameObject);
        }
    }
}
