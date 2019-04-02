using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadonManager : MonoBehaviour
{
    private int memberCount = 0;
    private GameObject powerupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        powerupPrefab = Resources.Load<GameObject>("Prefabs/PowerUp");

        memberCount = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMemberDestroy(Vector3 diePosition)
    {
        memberCount--;

        if(memberCount <= 0)
        {
            Instantiate(powerupPrefab, diePosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
