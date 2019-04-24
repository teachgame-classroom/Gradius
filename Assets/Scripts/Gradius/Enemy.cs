using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Straight, ZigZag, Sine, Static }

public enum MoveDirection { Left, Right, Up, Down }

public enum BulletMovePattern { AimAtPlayer, Horizontal, Vertical }

public class Enemy : MonoBehaviour
{
    public MovePattern movePattern;

    public MoveDirection straightMoveDirection;
    public float straightMoveDistance = 0;

    private float straightMoveTotalDistance = 0;

    public BulletMovePattern bulletMovePattern = BulletMovePattern.AimAtPlayer;

    public float speed = 10;
    public float changeDirectionPeriod = 1f;
    public float sineAmp = 1f;

    public Color trackColor = Color.white;

    private Vector3 velocity_v = Vector3.up;
    private Vector3 velocity_h = Vector3.left;

    private float lastChangeDirectionTime = 0;

    public int hp = 1;
    public bool invincible = false;
    public float invincibleTime;
    private float spawnTime;

    public bool playDamageEffect;

    public string explosionPrefabName = "Prefabs/Effects/Explosion_Red";
    private GameObject explosionPrefab;

    public SquadonManager squadonManager;   // 此敌人的所属小队，敌人生成的时候由所属小队脚本指定

    private List<Vector3> tracks = new List<Vector3>();
    private float lastRecordTime = 0;

    private GameObject player;

    public float turretMinAngle = 0;
    public float turretMaxAngle = 180;
    public Sprite[] turrentSprites;
    private SpriteRenderer spriteRenderer;
    private Transform shotPos;

    private Animator anim;

    public GameObject bulletPrefab;
    public float fireInterval;
    private float lastFireTime;

    public bool waitForPlayer;

    private bool activated;
    private float activeDistance;   // 激活此敌人的摄像机距离

    private Collider2D col;

    public bool dropPowerUp = true;
    private GameObject powerupPrefab;
    // Start is called before the first frame update
    void Start()
    {
        powerupPrefab = Resources.Load<GameObject>("Prefabs/PowerUp");
        explosionPrefab = Resources.Load<GameObject>(explosionPrefabName);

        player = GameObject.Find("Vic Viper");

        shotPos = transform.Find("ShotPos");

        if(shotPos == null)
        {
            shotPos = transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // 激活敌人的摄像机距离 = 摄像机宽度的一半 + 一个单位
        activeDistance = Camera.main.orthographicSize * Camera.main.aspect + 1;

        if(squadonManager == null)
        {
            if (waitForPlayer)
            {
                SetEnemyActive(false);
            }
            else
            {
                SetEnemyActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 敌人还没激活时，检查摄像机距离，判断玩家是否已经接近，如果已经足够接近，就激活敌人
        if(!activated)
        {
            if(squadonManager == null)
            {
                if (IsPlayerCloseEnough())
                {
                    SetEnemyActive(true);
                }
            }
        }
        else
        {
            if(Time.time - spawnTime > invincibleTime)
            {
                invincible = false;
            }

            if (squadonManager == null)
            {
                // 在没有所属小队的情况下，每个敌人自行移动
                switch (movePattern)
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
                    case MovePattern.Static:
                        StaticMove();
                        break;
                }

                if (Time.time - lastRecordTime > 0.1f)
                {
                    tracks.Add(transform.position);
                    if (tracks.Count > 48)
                    {
                        tracks.RemoveAt(0);
                    }
                    lastRecordTime = Time.time;
                }
            }

            if (bulletPrefab != null)
            {
                Shoot();
            }
        }
    }

    private void SetEnemyActive(bool isActive)
    {
        if(spriteRenderer != null)
        {
            if(!playDamageEffect)
            {
                spriteRenderer.enabled = isActive;
            }
        }

        col.enabled = isActive;
        activated = isActive;

        if(activated)
        {
            spawnTime = Time.time;
        }
    }

    // 检查摄像机与生成点的距离，如果小于激活距离，返回true，否则返回false
    private bool IsPlayerCloseEnough()
    {
        float playerDistanceX = transform.position.x - Camera.main.transform.position.x;

        return playerDistanceX < activeDistance;
    }

    void StaticMove()
    {
        Vector3 aimDirection = GetAimDirection(player.transform.position);
        SetSpriteByAimDirection(aimDirection);
    }

    void StraightMove()
    {
        if(straightMoveDistance > 0)
        {
            straightMoveTotalDistance += speed * Time.deltaTime;

            if(straightMoveTotalDistance < straightMoveDistance)
            {
                transform.Translate(GetStraightMoveDirection() * speed * Time.deltaTime, Space.World);
            }
            else
            {
                anim.SetBool("Stop", true);
            }
        }
        else
        {
            transform.Translate(GetStraightMoveDirection() * speed * Time.deltaTime, Space.World);
        }
    }

    Vector3 GetStraightMoveDirection()
    {
        switch(straightMoveDirection)
        {
            case MoveDirection.Left:
                return Vector3.left;
            case MoveDirection.Right:
                return Vector3.right;
            case MoveDirection.Up:
                return Vector3.up;
            case MoveDirection.Down:
                return Vector3.down;
            default:
                return Vector3.right;
        }
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

    Vector3 GetAimDirection(Vector3 targetPosition)
    {
        Vector3 aimDirection = Vector3.left;

        switch(bulletMovePattern)
        {
            case BulletMovePattern.Horizontal:
                aimDirection = Vector3.left;
                break;
            case BulletMovePattern.Vertical:
                aimDirection = Vector3.up;
                break;
            case BulletMovePattern.AimAtPlayer:
                aimDirection = (targetPosition - shotPos.position).normalized;
                break;
            default:
                aimDirection = Vector3.left;
                break;
        }

        return aimDirection;
    }

    void SetSpriteByAimDirection(Vector3 aimDirection)
    {
        if (turrentSprites.Length == 0) return;

        float angleStep = (turretMaxAngle - turretMinAngle) / (turrentSprites.Length - 1);

        float angle = Vector3.SignedAngle(Vector3.right, aimDirection, Vector3.forward) + angleStep / 2;

        angle = Mathf.Clamp(angle, turretMinAngle, turretMaxAngle);

        int spriteIdx = Mathf.Abs(Mathf.FloorToInt(angle / angleStep)) - 1;

        spriteIdx = Mathf.Clamp(spriteIdx, 0, turrentSprites.Length - 1);

        //Debug.Log("Step:" + angleStep + ", Aim Angle:" + angle + ", idx:" + spriteIdx);

        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = turrentSprites[spriteIdx];
        }
    }

    public void Shoot()
    {
        Vector3 aimDirection = GetAimDirection(player.transform.position);
        float angle = Vector3.SignedAngle(Vector3.right, aimDirection,Vector3.forward);
        bool playerNotInTurretAngle = (angle < turretMinAngle || angle > turretMaxAngle);

        // 如果玩家不在炮塔型敌人的射程角度内，不执行射击
        if (bulletMovePattern == BulletMovePattern.AimAtPlayer && playerNotInTurretAngle)
        {
            return;
        }

        if(Time.time - lastFireTime > fireInterval)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, shotPos.position, Quaternion.identity);
            bulletInstance.GetComponent<BulletMove>().moveDirection = GetAimDirection(player.transform.position);

            lastFireTime = Time.time;
        }
    }

    public void Hurt(int damage)
    {
        if(!invincible)
        {
            hp -= damage;

            if (hp <= 0)
            {
                Die();
            }
            else
            {
                if(playDamageEffect)
                {
                    spriteRenderer.enabled = true;
                    Invoke("TurnOffSprite", 0.1f);
                }
            }
        }
    }

    public void TurnOffSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void Die()
    {
        if(squadonManager != null)
        {
            squadonManager.OnMemberDestroy(transform.position);
        }

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        LoopExplosion loopExplosion = explosion.GetComponent<LoopExplosion>();

        if (loopExplosion)
        {
            loopExplosion.Attach(transform);
        }

        if(dropPowerUp)
        {
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit:" + gameObject.name);
    }

    private void OnDrawGizmos()
    {
        if(!activated)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.15f);
        }

        if(movePattern == MovePattern.Straight)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + GetStraightMoveDirection() * straightMoveDistance);
        }

        //for(int i = 0; i < tracks.Count; i++)
        //{
        //    Gizmos.DrawSphere(tracks[i], 0.1f);
        //}

        //if(shotPos != null && player != null)
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawLine(shotPos.position, shotPos.position + GetAimDirection(player.transform.position) * 5);
        //}
    }
}
