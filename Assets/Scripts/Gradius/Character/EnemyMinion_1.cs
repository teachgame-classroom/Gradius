using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinion_1 : EnemyBase
{
    public float changeDirectionPeriod = 2f;
    public Vector3 velocity_v = new Vector3(0,5,0);

    protected float lastChangeDirectionTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        if (Time.time - lastChangeDirectionTime > changeDirectionPeriod)
        {
            velocity_v = -velocity_v;
            lastChangeDirectionTime = Time.time;
        }

        Vector3 velocity_h = Vector3.left * moveSpeed;

        Vector3 velocity = velocity_h + velocity_v;

        transform.right = -velocity;

        Move(velocity);
    }
}
