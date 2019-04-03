using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + "和" + collision.gameObject.name + "发生了实体碰撞");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + "和" + other.gameObject.name + "发生了触发碰撞");
    }
}
