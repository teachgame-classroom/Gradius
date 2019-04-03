using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10;
    private int hp = 1;
    private GameObject explosionPrefab;

    public SquadonManager squadonManager;   // 此敌人的所属小队，敌人生成的时候由所属小队脚本指定

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Effects/Explosion_Red");
    }

    // Update is called once per frame
    void Update()
    {
        // 沿路径移动的敌人的路线由所属小队脚本控制，敌人不再自行移动
        // transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

    public void Hurt(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        squadonManager.OnMemberDestroy(transform.position);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
