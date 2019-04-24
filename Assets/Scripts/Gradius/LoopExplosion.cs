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

    public void Attach(Transform attachTo)
    {
        transform.SetParent(attachTo.parent);
        Collider2D col = attachTo.GetComponent<Collider2D>();

        float xMax = col.bounds.center.x + col.bounds.extents.x - transform.position.x;
        float xMin = col.bounds.center.x - col.bounds.extents.x - transform.position.x;
        float yMax = col.bounds.center.y + col.bounds.extents.y - transform.position.y;
        float yMin = col.bounds.center.y - col.bounds.extents.y - transform.position.y;

        explosionAreaMin = Vector2.right * xMin + Vector2.up * yMin;
        explosionAreaMax = Vector2.right * xMax + Vector2.up * yMax;
    }
}
