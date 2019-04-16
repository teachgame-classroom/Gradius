using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopExplosion : MonoBehaviour
{
    public Vector2 explosionAreaMin;
    public Vector2 explosionAreaMax;

    public float explosionInterval;

    private float lastExplosionTime;

    private Transform effect;

    // Start is called before the first frame update
    void Start()
    {
        effect = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastExplosionTime > explosionInterval)
        {
            lastExplosionTime = Time.time;

            float posX = Random.Range(explosionAreaMin.x, explosionAreaMax.x);
            float posY = Random.Range(explosionAreaMin.y, explosionAreaMax.y);

            effect.position = transform.position + Vector3.right * posX + Vector3.up * posY;

            effect.gameObject.SetActive(false);
            effect.gameObject.SetActive(true);
        }
    }
}
