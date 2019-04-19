using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 15;
    public bool isMissile;
    public bool isBarrier;
    public Vector3 moveDirection;

    private string[] stageLayerMask = new string[] { "Stage" };

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
        if (isMissile)
        {
            if(GetGroundNormal() != Vector3.zero)
            {
                transform.up = GetGroundNormal();
                moveDirection = transform.right;
            }
        }

        if(isBarrier)
        {
            transform.RotateAround(transform.parent.position, Vector3.forward, speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
    }

    Vector3 GetGroundNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up * 0.1f, -transform.up, 0.2f, LayerMask.GetMask(stageLayerMask));

        if(hit.transform != null)
        {
            //Debug.DrawLine(transform.position, hit.point, Color.red, 1f);

            //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, 1f);

            //Debug.Log("导弹正接近：" + hit.transform.name);
            return hit.normal;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
