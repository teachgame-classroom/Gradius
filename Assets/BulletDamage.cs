using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
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
            enemy.Hurt(10);
            Destroy(gameObject);
        }
    }
}
