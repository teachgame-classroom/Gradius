using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool drawMoveTrail;
    public float moveSpeed;

    protected Transform[] shotPosTrans;
    protected Weapon currentWeapon;

    protected List<Vector3> tracks = new List<Vector3>();
    protected float lastRecordTime;

    protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitCharacter();
        InitWeapon();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        Shoot();

        if(drawMoveTrail)
        {
            RecordMoveTrail();
        }
    }

    protected virtual void InitCharacter()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

    protected void Shoot()
    {
        if(currentWeapon != null)
        {
            currentWeapon.TryShoot();
        }
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
}
