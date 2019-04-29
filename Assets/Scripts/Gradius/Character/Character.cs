using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string[] hurtTags;

    public bool invincible = false;
    public int maxHp;

    public bool dropPowerUp;

    public bool isAlive { get { return invincible || hp > 0; } }
    protected int hp;

    public bool drawMoveTrail;
    public float moveSpeed;

    protected Transform[] shotPosTrans;
    protected Weapon currentWeapon;

    protected List<Vector3> tracks = new List<Vector3>();
    protected float lastRecordTime;

    protected SpriteRenderer spriteRenderer;
    protected GameObject dieEffect;

    protected GameObject powerUpPrefab;

    protected abstract string deathClipName { get; }
    protected AudioClip deathClip;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitCharacter();
        InitWeapon();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(isAlive)
        {
            Move();
            Shoot();
        }

        if (drawMoveTrail)
        {
            RecordMoveTrail();
        }
    }

    protected virtual void InitCharacter()
    {
        hp = maxHp;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if(dropPowerUp)
        {
            powerUpPrefab = Resources.Load<GameObject>("Prefabs/PowerUp");
        }

        deathClip = Resources.Load<AudioClip>("Sounds/" + deathClipName);
    }

    protected virtual void InitWeapon()
    {
        ShotPosMarker[] markers = GetComponentsInChildren<ShotPosMarker>();

        if(markers.Length > 0)
        {
            shotPosTrans = new Transform[markers.Length];

            for (int i = 0; i < markers.Length; i++)
            {
                Debug.Log("ShotPos Found:" + markers[i].transform.name);
                shotPosTrans[i] = markers[i].transform;
            }
        }
    }

    protected virtual void Move()
    {
        Move(Vector3.left);
    }

    protected void Move(Vector3 moveDirection)
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void Shoot()
    {
        if(currentWeapon != null)
        {
            currentWeapon.TryShoot();
        }
    }

    public void Hurt(int damage)
    {
        if (!invincible)
        {
            hp -= damage;

            if (hp <= 0)
            {
                Die();
            }
            else
            {
                PlayHurtEffect();
            }
        }
    }

    protected virtual void PlayHurtEffect()
    {

    }

    protected virtual void PlayDieEffect()
    {
        GameObject explosion = Instantiate(dieEffect, transform.position, Quaternion.identity);
        LoopExplosion loopExplosion = explosion.GetComponent<LoopExplosion>();

        if(loopExplosion)
        {
            loopExplosion.Attach(transform);
        }
    }

    public void TurnOffSprite()
    {
        spriteRenderer.enabled = false;
    }

    public virtual void Die()
    {
        DoBeforeDie();

        Destroy(gameObject);
    }

    protected virtual void DoBeforeDie()
    {
        if (dropPowerUp)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

        if (dieEffect)
        {
            PlayDieEffect();
        }

        GetComponentInChildren<Collider2D>().enabled = false;

        AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position);
    }

    protected void RecordMoveTrail()
    {
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

    protected void OnDrawGizmos()
    {
        if (drawMoveTrail)
        {
            for (int i = 0; i < tracks.Count; i++)
            {
                Gizmos.DrawSphere(tracks[i], 0.1f);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible)
        {
            for (int i = 0; i < hurtTags.Length; i++)
            {
                if (collision.tag == hurtTags[i])
                {
                    int damage = maxHp;
                    BulletDamage bullet = collision.GetComponent<BulletDamage>();

                    if (bullet)
                    {
                        damage = bullet.damage;
                        bullet.OnHit();
                    }

                    Hurt(damage);
                }
            }
        }
    }

}
