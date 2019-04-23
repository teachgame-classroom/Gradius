using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected const int KEY_W = 0;
    protected const int KEY_S = 1;
    protected const int KEY_A = 2;
    protected const int KEY_D = 3;
    protected const int KEY_J = 4;
    protected const int KEY_K = 5;

    protected const int DOUBLE = 1;
    protected const int LASER = 2;

    protected bool[] keyState = new bool[6];    // 0 - W; 1 - S; 2 - A; 3 - D; 4 - J; 5 - K

    protected const int TOTAL_WEAPON = 3;
    protected Weapon[] weapons = new Weapon[TOTAL_WEAPON];
    protected int currentWeaponIdx = 0;
    protected Missile missile;

    protected int powerup = 0;
    protected float finalSpeed = 0;
    protected float speedLevel = 0;
    protected int optionLevel = 0;

    private List<Vector3> trackList = new List<Vector3>();
    private float trackNodeDistance = 0.04f * 0.04f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        trackList.Add(transform.position);
    }

    // Update is called once per frame
    protected override void Update()
    {
        UpdateKeyState();

        bool powerupKeyDown = keyState[KEY_K];

        if(powerupKeyDown)
        {
            TryPowerUp();
        }

        UpdateTrackList();

        Debug.Log(finalSpeed);

        shotPosTrans[1].position = Vector3.MoveTowards(shotPosTrans[1].position, trackList[trackList.Count / 2], finalSpeed * Time.deltaTime);
        shotPosTrans[2].position = Vector3.MoveTowards(shotPosTrans[2].position, trackList[0], finalSpeed * Time.deltaTime);

        base.Update();
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        powerup = 0;
        speedLevel = 0;
        optionLevel = 0;

        SetSpeed();

        SetBarrierActive(false);

        if (hurtTags.Length == 0)
        {
            hurtTags = new string[] { "Stage", "Enemy", "EnemyBullet" };
        }

        dieEffect = Resources.Load<GameObject>("Prefabs/Effects/Explosion_player");
    }

    protected override void InitWeapon()
    {
        base.InitWeapon();

        weapons[0] = new NormalWeapon(shotPosTrans, true);
        weapons[1] = new DoubleCannon(shotPosTrans, true);
        weapons[2] = new Laser(shotPosTrans, true);

        missile = new Missile(new Transform[] { transform, shotPosTrans[1], shotPosTrans[2] }, true);
        missile.LevelUp();
        currentWeaponIdx = 0;

        currentWeapon = weapons[currentWeaponIdx];

        SetOptionActive(false);
    }

    protected override void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        if (keyState[KEY_W]) moveDirection += Vector3.up;
        if (keyState[KEY_S]) moveDirection += Vector3.down;
        if (keyState[KEY_A]) moveDirection += Vector3.left;
        if (keyState[KEY_D]) moveDirection += Vector3.right;

        Move(moveDirection);
    }

    protected override void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            powerup++;
            powerup = powerup % 7;
        }

        bool isFireKeyDown = keyState[KEY_J];

        if(isFireKeyDown)
        {
            if(missile != null)
            {
                missile.TryShoot();
            }

            base.Shoot();
        }
    }

    protected void UpdateKeyState()
    {
        keyState[KEY_W] = Input.GetKey(KeyCode.W);
        keyState[KEY_S] = Input.GetKey(KeyCode.S);
        keyState[KEY_A] = Input.GetKey(KeyCode.A);
        keyState[KEY_D] = Input.GetKey(KeyCode.D);
        keyState[KEY_J] = Input.GetKeyDown(KeyCode.J);
        keyState[KEY_K] = Input.GetKeyDown(KeyCode.K);
    }

    protected void ChangePrimaryWeapon(int newWeaponIdx)
    {
        newWeaponIdx = Mathf.Clamp(newWeaponIdx, 0, weapons.Length - 1);
        currentWeaponIdx = newWeaponIdx;

        currentWeapon = weapons[currentWeaponIdx];

        powerup = 0;
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
        powerup = 0;
    }

    void PowerUpOption()
    {
        powerup = 0;

        for (int i = 1; i < shotPosTrans.Length; i++)
        {
            if (shotPosTrans[i].gameObject.activeSelf == false)
            {
                SetOptionActive(i, true);
                break;
            }
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].PowerOption();
        }

        missile.PowerOption();
    }

    private void SetOptionActive(bool isActive)
    {
        for (int i = 1; i < shotPosTrans.Length; i++)
        {
            SetOptionActive(i, isActive);
        }
    }

    private void SetOptionActive(int idx, bool isActive)
    {
        shotPosTrans[idx].gameObject.SetActive(isActive);
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
        finalSpeed = moveSpeed + speedLevel * 2;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("Power Up:{0}", powerup));
        GUILayout.Label(string.Format("Missle level:{0}", missile.level));
        GUILayout.Label(string.Format("Track Node Count:" + trackList.Count));
        GUILayout.EndVertical();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < trackList.Count; i++)
        {
            Gizmos.DrawSphere(trackList[i], 0.1f);
        }
    }

    void UpdateTrackList()
    {
        if (Vector3.SqrMagnitude(transform.position - trackList[trackList.Count - 1]) > trackNodeDistance)
        {
            trackList.Add(transform.position);

            if (trackList.Count > 16)
            {
                trackList.RemoveAt(0);
            }
        }
    }
}
