using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 15;
    public Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        if(moveDirection == Vector3.zero)
        {
            moveDirection = transform.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);        
    }
}
