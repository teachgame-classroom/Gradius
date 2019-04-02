using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10;
    private int hp = 1;
    private GameObject explosionPrefab;

    private SquadonManager squadonManager;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Effects/Explosion_Red");
        squadonManager = transform.parent.GetComponent<SquadonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
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
