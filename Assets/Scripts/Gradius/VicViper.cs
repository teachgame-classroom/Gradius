﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryWeaponType { Normal, Double = 3, Laser = 4 }

public class VicViper : MonoBehaviour
{
    public const int NORMAL = 0;
    public const int DOUBLE = 1;
    public const int LASER = 2;
    public const int MISSILE = 2;

    public const int TOTAL_WEAPON = 3;
    private Weapon[] weapons = new Weapon[TOTAL_WEAPON];
    private Weapon currentWeapon;
    private int currentWeaponIdx;

    private Missile missile;

    public int life = 99;

    public float baseSpeed = 10;
    private float speed;
    public PrimaryWeaponType primaryWeapon;

    private bool isAutoPilot = true;
    private bool invicible = false;
    private float lastSpawnTime = 0;
    private float lastBlinkTime = 0;
    private float lastMissileTime = 0;
    private float missileInterval = 5f;

    private int speedLevel = 0;
    private int missileLevel = 0;
    private int optionLevel = 0;

    private int powerup = 0;

    private Transform[] shotPosTrans;
    private Transform spawnTrans;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    private GameObject[] bullets;
    private GameObject dieEffectPrefab;

    public Transform[] options;


    private List<Vector3> trackList = new List<Vector3>();

    private float trackNodeDistance = 0.04f * 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Prefabs/Bullets");
        dieEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Explosion_player");
        shotPosTrans = new Transform[3];

        shotPosTrans[0] = transform.Find("ShotPos");
        shotPosTrans[1] = options[0];
        shotPosTrans[2] = options[1];

        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spawnTrans = Camera.main.transform.Find("PlayerSpawn");
        //options = GameObject.Find("Option").transform;

        trackList.Add(transform.position);

        InitWeapon();

        Spawn();
    }

    private void InitWeapon()
    {
        //weapons[0] = new GuidedWeapon(shotPosTrans, "Enemy");
        weapons[0] = new NormalWeapon(shotPosTrans, true);
        weapons[1] = new DoubleCannon(shotPosTrans, true);
        weapons[2] = new Laser(shotPosTrans, true);

        missile = new Missile(shotPosTrans, true);

        currentWeaponIdx = 0;

        currentWeapon = weapons[currentWeaponIdx];
    }

    // Update is called once per frame
    void Update()
    {
        // 重生5秒后打开碰撞体，使玩家可以被伤害
        if(Time.time - lastSpawnTime < 5)
        {
            if(Time.time - lastBlinkTime > 0.1f)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                lastBlinkTime = Time.time;
            }
        }
        else
        {
            if(invicible)
            {
                spriteRenderer.enabled = true;
                invicible = false;
                //col.enabled = true;
            }
        }

        if(isAutoPilot)
        {
            if(Time.time - lastSpawnTime > 1)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;

                float distanceToCamera = Camera.main.transform.position.x - transform.position.x;
                float distanceToExitSpawnState = Camera.main.orthographicSize * Camera.main.aspect * 0.75f;
                if (distanceToCamera < distanceToExitSpawnState)
                {
                    isAutoPilot = false;
                }
            }
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            transform.position += (Vector3.right * h + Vector3.up * v) * speed * Time.deltaTime;

            ClampPlayerPosition();

            UpdateTrackList();

            options[0].position = Vector3.MoveTowards(options[0].position, trackList[0], speed * Time.deltaTime);
            options[1].position = Vector3.MoveTowards(options[1].position, trackList[trackList.Count / 2], speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.J))
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                TryPowerUp();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangePrimaryWeapon(0);
                //ChangePrimaryWeapon(PrimaryWeaponType.Normal);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangePrimaryWeapon(1);
                //ChangePrimaryWeapon(PrimaryWeaponType.Double);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangePrimaryWeapon(2);
                //ChangePrimaryWeapon(PrimaryWeaponType.Laser);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangePrimaryWeapon(3);
                //ChangePrimaryWeapon(PrimaryWeaponType.Laser);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PowerUpOption();
                //ChangePrimaryWeapon(PrimaryWeaponType.Laser);
            }
        }
    }

    private void ClampPlayerPosition()
    {
        Vector3 camPos = Camera.main.transform.position;

        float left = camPos.x - Camera.main.orthographicSize * Camera.main.aspect + 0.5f;
        float right = camPos.x + Camera.main.orthographicSize * Camera.main.aspect - 0.5f;
        float top = camPos.y + Camera.main.orthographicSize - 0.5f;
        float bottom = camPos.y - Camera.main.orthographicSize + 0.5f;

        float clamp_x = Mathf.Clamp(transform.position.x, left, right);
        float clamp_y = Mathf.Clamp(transform.position.y, bottom, top);

        Vector3 clampPos = Vector3.right * clamp_x + Vector3.up * clamp_y;

        transform.position = clampPos;
    }

    void Shoot()
    {
        currentWeapon.TryShoot();
        missile.TryShoot();
    }

    void TryPowerUp()
    {
        switch (powerup)
        {
            case 1:
                PowerUPSpeed();
                break;
            case 2:
                PowerUpMissile();
                break;
            case 3:
                ChangePrimaryWeapon(DOUBLE);
                break;
            case 4:
                ChangePrimaryWeapon(LASER);
                break;
            case 5:
                PowerUpOption();
                break;
            case 6:
                PowerUpBarrier();
                break;
        }
    }

    void ChangePrimaryWeapon(int newWeaponIdx)
    {
        newWeaponIdx = Mathf.Clamp(newWeaponIdx, 0, weapons.Length - 1);
        currentWeaponIdx = newWeaponIdx;

        currentWeapon = weapons[currentWeaponIdx];

        powerup = 0;
    }

    void PowerUPSpeed()
    {
        powerup = 0;

        speedLevel++;
        speedLevel = Mathf.Min(5, speedLevel);

        SetSpeed();
    }

    void PowerUpMissile()
    {
        missile.LevelUp();
        powerup -= MISSILE;
    }

    void PowerUpOption()
    {
        powerup = 0;

        for(int i = 0; i < options.Length; i++)
        {
            if(options[i].gameObject.activeSelf == false)
            {
                SetOptionActive(i, true);
                break;
            }
        }

        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].PowerOption();
        }

        missile.PowerOption();
    }

    private void SetOptionActive(bool isActive)
    {
        for (int i = 0; i < options.Length; i++)
        {
            SetOptionActive(i, isActive);
        }
    }

    private void SetOptionActive(int idx, bool isActive)
    {
        options[idx].gameObject.SetActive(isActive);
    }


    void PowerUpBarrier()
    {
        SetBarrierActive(true);
        powerup = 0;
    }

    private void SetBarrierActive(bool isActive)
    {
        transform.Find("Bullet_4_1").gameObject.SetActive(isActive);
        transform.Find("Bullet_4_2").gameObject.SetActive(isActive);
    }

    private void SetSpeed()
    {
        speed = baseSpeed + speedLevel * 2;
    }

    void UpdateTrackList()
    {
        if(Vector3.SqrMagnitude(transform.position - trackList[trackList.Count - 1]) > trackNodeDistance)
        {
            trackList.Add(transform.position);

            if(trackList.Count > 16)
            {
                trackList.RemoveAt(0);
            }
        }
    }

    void Hurt()
    {
        Die();
    }

    void Die()
    {
        Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        life--;

        if(life > 0)
        {
            // Revive
            Spawn();
        }
        else
        {
            // Game Over
        }
    }

    void Spawn()
    {
        powerup = 0;
        speedLevel = 0;
        missileLevel = 0;
        optionLevel = 0;

        SetSpeed();

        primaryWeapon = PrimaryWeaponType.Normal;
        SetBarrierActive(false);
        SetOptionActive(false);
        transform.position = spawnTrans.position;
        isAutoPilot = true;
        invicible = true;
        lastSpawnTime = Time.time;
        //col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("碰到了" + collision.gameObject.name);

        if(collision.tag == "PowerUp")
        {
            powerup++;
            if(powerup > 6)
            {
                powerup = 1;
            }
            Destroy(collision.gameObject);
        }

        if(!invicible)
        {
            if (collision.tag == "Stage" || collision.tag == "Enemy" || collision.tag == "EnemyBullet")
            {
                Hurt();
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("Power Up:{0}", powerup));
        GUILayout.Label(string.Format("Missle level:{0}", missileLevel));
        GUILayout.Label(string.Format("Track Node Count:" + trackList.Count));
        GUILayout.EndVertical();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for(int i = 0; i < trackList.Count; i++)
        {
            Gizmos.DrawSphere(trackList[i], 0.1f);
        }
    }
}
