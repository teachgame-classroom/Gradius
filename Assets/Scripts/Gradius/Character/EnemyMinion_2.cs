using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMinion_2 : EnemyBase
{
    public float period = 2f;
    public float sineAmp = 2f;

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

    protected override void InitCharacter()
    {
    }

    protected override void Move()
    {
        Vector3 velocity_v = Vector3.up * Mathf.Sin(Mathf.PI * 2 * Time.time / period) * sineAmp;
        Vector3 velocity_h = Vector3.left * moveSpeed;

        Vector3 velocity = velocity_h + velocity_v;

        Move(velocity);
    }
}
