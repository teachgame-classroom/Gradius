using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Straight, ZigZag, Sine }

public class Enemy : MonoBehaviour
{
    public MovePattern movePattern;
    public float speed = 10;
    public float changeDirectionPeriod = 1f;
    public float sineAmp = 1f;

    public Color trackColor = Color.white;

    private Vector3 velocity_v = Vector3.up;
    private Vector3 velocity_h = Vector3.left;

    private float lastChangeDirectionTime = 0;

    private int hp = 1;
    private GameObject explosionPrefab;

    public SquadonManager squadonManager;   // 此敌人的所属小队，敌人生成的时候由所属小队脚本指定

    private List<Vector3> tracks = new List<Vector3>();
    private float lastRecordTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Effects/Explosion_Red");
    }

    // Update is called once per frame
    void Update()
    {
        if(squadonManager == null)
        {
            // 在没有所属小队的情况下，每个敌人自行移动
            switch(movePattern)
            {
                case MovePattern.Straight:
                    StraightMove();
                    break;
                case MovePattern.ZigZag:
                    ZigSawMove();
                    break;
                case MovePattern.Sine:
                    SineMove();
                    break;
            }

            if(Time.time - lastRecordTime > 0.1f)
            {
                tracks.Add(transform.position);
                if(tracks.Count > 48)
                {
                    tracks.RemoveAt(0);
                }
                lastRecordTime = Time.time;
            }
        }
    }

    void StraightMove()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

    void ZigSawMove()
    {
        if(Time.time - lastChangeDirectionTime > changeDirectionPeriod)
        {
            velocity_v = -velocity_v;
            lastChangeDirectionTime = Time.time;
        }

        Vector3 velocity = velocity_h + velocity_v;

        transform.right = -velocity;

        transform.Translate(velocity * speed * Time.deltaTime, Space.World);
    }

    void SineMove()
    {
        velocity_v = Vector3.up * Mathf.Sin(Mathf.PI * 2 * Time.time / changeDirectionPeriod) * sineAmp;

        Vector3 velocity = velocity_h * speed + velocity_v;

        transform.Translate(velocity * Time.deltaTime, Space.World);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = trackColor;

        for(int i = 0; i < tracks.Count; i++)
        {
            Gizmos.DrawSphere(tracks[i], 0.1f);
        }
    }
}
