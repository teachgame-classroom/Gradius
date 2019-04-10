using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人小队，管理一支小队的敌人数量，移动速度，移动路线
public class SquadonManager : MonoBehaviour
{
    public int memberCount = 5;     // 小队的敌人数量
    public float moveSpeed = 10;    // 小队的移动速度

    private Transform[] waypoints;  // 移动路线的路径点

    private GameObject powerupPrefab;   // 小队全灭后掉落的物品Prefab
    private GameObject[] enemyPrefabs;  // 敌人Prefab

    private GameObject[] members;       // 小队中的全部敌人
    private int[] memberWaypointIdx;    // 每个敌人的当前移动路径点

    private GameObject player;        // 存放玩家
    private bool isMemberActivated;   // 小队成员是否已经被激活

    private float activeDistance;   // 激活小队成员的摄像机距离

    // Start is called before the first frame update
    void Start()
    {
        // 加载 Prefab
        powerupPrefab = Resources.Load<GameObject>("Prefabs/PowerUp");
        enemyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        // 初始化数组
        members = new GameObject[memberCount];
        memberWaypointIdx = new int[memberCount];

        // 生成小队中的每个敌人
        for(int i = 0; i < memberCount; i++)
        {
            // 小队成员的生成位置从左到右一字排开
            members[i] = Instantiate(enemyPrefabs[0], transform.position + Vector3.right * i, Quaternion.identity);

            // 将本小队设定为小队每个成员的“所属小队”
            // 小队成员被消灭时，会通过这个变量调用自己所属小队的OnMemberDestroy方法，通知所属小队，自己已经被消灭
            members[i].GetComponent<Enemy>().squadonManager = this;

            members[i].SetActive(false);
        }

        // 查找所有子对象，存放到waypoints数组，这些就是路径点
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }

        player = GameObject.Find("Vic Viper");

        // 激活小队成员的摄像机距离 = 摄像机宽度的一半 + 一个单位
        activeDistance = Camera.main.orthographicSize * Camera.main.aspect + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMemberActivated)
        {
            if(IsPlayerCloseEnough())
            {
                ActivateMembers();
            }
        }
        else
        {
            // 让每个小队成员沿路径点移动
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] != null)
                {
                    members[i].transform.position = MoveAlongPath(members[i].transform.position, i);
                }
            }
        }
    }

    // 检查摄像机与生成点的距离，如果小于激活距离，返回true，否则返回false
    private bool IsPlayerCloseEnough()
    {
        float playerDistanceX = transform.position.x - Camera.main.transform.position.x;

        return playerDistanceX < activeDistance;
    }

    private void ActivateMembers()
    {
        for(int i = 0; i < members.Length; i++)
        {
            members[i].SetActive(true);
        }

        isMemberActivated = true;
    }

    // 计算某个小队成员沿路径移动的位置，currentPosition-当前位置，memberIdx-成员编号
    private Vector3 MoveAlongPath(Vector3 currentPosition, int memberIdx)
    {
        // newPos 是存放返回值的变量
        Vector3 newPos = currentPosition;

        // 获取要移动的小队成员的目标路径点
        int waypointIdx = memberWaypointIdx[memberIdx];

        // 使用 Vector3.MoveTowards 方法计算“以速度 moveSpeed 从当前位置移动到目标路径点，这一帧应该移动到什么位置”
        newPos = Vector3.MoveTowards(currentPosition, waypoints[waypointIdx].position, moveSpeed * Time.deltaTime);

        // 如果这一帧已经到达目标路径点
        if (newPos == waypoints[waypointIdx].position)
        {
            // 如果已经到达最后一个路径点，路径点编号不再变化，停留在终点位置
            if (waypointIdx == waypoints.Length - 1)
            {
                return newPos;
            }

            // 如果后面还有路径点，路径点编号加一
            waypointIdx += 1;
            memberWaypointIdx[memberIdx] = waypointIdx;
        }

        // 返回新位置的计算结果
        return newPos;
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

    private void OnDrawGizmos()
    {
        if (waypoints == null)
        {
            waypoints = new Transform[transform.childCount];

            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = transform.GetChild(i);
            }
        }

        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(waypoints[i].position, Vector3.one * 0.1f);

            if (i < waypoints.Length - 1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }

}
