using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicViper : MonoBehaviour
{
    public float speed = 10;

    private Transform shotPosTrans;

    private GameObject[] bullets;

    // Start is called before the first frame update
    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Prefabs/Bullets");
        shotPosTrans = transform.Find("ShotPos");
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += (Vector3.right * h + Vector3.up * v) * speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.J))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullets[0], shotPosTrans.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("碰到了" + collision.gameObject.name);
    }
}
