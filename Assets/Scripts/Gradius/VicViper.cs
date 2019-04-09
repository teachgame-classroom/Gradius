using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryWeaponType { Normal, Double = 3, Laser = 4 }

public class VicViper : MonoBehaviour
{
    public const int NORMAL = 0;
    public const int LASER = 1;
    public const int MISSILE = 2;

    public float speed = 10;
    public PrimaryWeaponType primaryWeapon;

    private int missileLevel = 0;

    private int powerup = 0;

    private Transform shotPosTrans;

    private GameObject[] bullets;

    public Transform[] options;


    private List<Vector3> trackList = new List<Vector3>();

    private float trackNodeDistance = 0.04f * 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Prefabs/Bullets");
        shotPosTrans = transform.Find("ShotPos");
        //options = GameObject.Find("Option").transform;

        trackList.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
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
            ChangePrimaryWeapon(PrimaryWeaponType.Normal);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangePrimaryWeapon(PrimaryWeaponType.Double);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangePrimaryWeapon(PrimaryWeaponType.Laser);
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
        switch(primaryWeapon)
        {
            case PrimaryWeaponType.Normal:
                ShootNormal();
                break;
            case PrimaryWeaponType.Double:
                ShootDouble();
                break;
            case PrimaryWeaponType.Laser:
                ShootLaser();
                break;
        }

        if(missileLevel > 0)
        {
            ShootMissile(shotPosTrans);
            ShootMissile(options[0]);
            ShootMissile(options[1]);
        }
    }

    void TryPowerUp()
    {
        switch (powerup)
        {
            case 1:
                break;
            case 2:
                PowerUpMissile();
                break;
            case 3:
                ChangePrimaryWeapon(PrimaryWeaponType.Double);
                break;
            case 4:
                ChangePrimaryWeapon(PrimaryWeaponType.Laser);
                break;
            case 5:
                break;
            case 6:
                PowerUpBarrier();
                break;
        }
    }

    void ChangePrimaryWeapon(PrimaryWeaponType newWeaponType)
    {
        if(primaryWeapon != newWeaponType)
        {
            primaryWeapon = newWeaponType;
            powerup -= (int)newWeaponType;
        }
    }

    void ShootNormal()
    {
        Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.identity);

        Instantiate(bullets[NORMAL], options[0].position, Quaternion.identity);
        Instantiate(bullets[NORMAL], options[1].position, Quaternion.identity);
    }

    void ShootDouble()
    {
        Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.identity);
        Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.Euler(0,0,45));

        Instantiate(bullets[NORMAL], options[0].position, Quaternion.identity);
        Instantiate(bullets[NORMAL], options[0].position, Quaternion.Euler(0, 0, 45));
        
        Instantiate(bullets[NORMAL], options[1].position, Quaternion.identity);
        Instantiate(bullets[NORMAL], options[1].position, Quaternion.Euler(0, 0, 45));
    }

    void ShootLaser()
    {
        Instantiate(bullets[LASER], shotPosTrans.position, Quaternion.identity);

        Instantiate(bullets[LASER], options[0].position, Quaternion.identity);
        Instantiate(bullets[LASER], options[1].position, Quaternion.identity);

    }

    void ShootMissile(Transform firePos)
    {
        GameObject missileInstance = Instantiate(bullets[MISSILE], firePos.position, Quaternion.Euler(0, 0, -45));

        if(missileLevel == 2)
        {
            Instantiate(bullets[MISSILE], firePos.position + missileInstance.transform.right * 0.75f , Quaternion.Euler(0, 0, -45));
        }
    }

    void PowerUpMissile()
    {
        if(missileLevel < 2)
        {
            missileLevel++;
            powerup -= MISSILE;
        }
    }

    void PowerUpBarrier()
    {
        transform.Find("Bullet_4_1").gameObject.SetActive(true);
        transform.Find("Bullet_4_2").gameObject.SetActive(true);
        powerup = 0;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("碰到了" + collision.gameObject.name);

        if(collision.tag == "PowerUp")
        {
            powerup++;
            Destroy(collision.gameObject);
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
