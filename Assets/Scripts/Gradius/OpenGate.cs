using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public float openOffset;
    public float closeOffset;
    private float openDistance;     // 播放开门动画的摄像机距离
    private float closeDistance;   // 播放开门动画的摄像机距离


    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //// 播放开门动画的摄像机距离
        //openDistance = Camera.main.orthographicSize * Camera.main.aspect + openOffset;
        //closeDistance = Camera.main.orthographicSize * Camera.main.aspect + closeOffset;
        anim = GetComponent<Animator>();

        //Debug.Log("open:" + openDistance + ",close:" + closeDistance);
    }

    // Update is called once per frame
    void Update()
    {
        //float playerDistanceX = Mathf.Abs(transform.position.x - Camera.main.transform.position.x);

        //Debug.Log("Distance:" + playerDistanceX + ",Close:" + IsPlayerCloseEnough() + ",Far:" + IsPlayerFarEnough());

        //if(IsPlayerCloseEnough())
        //{
        //    anim.SetBool("open", true);
        //}
        //else if(IsPlayerFarEnough())
        //{
        //    anim.SetBool("open", false);
        //}
    }

    // 检查摄像机与生成点的距离，如果小于激活距离，返回true，否则返回false
    private bool IsPlayerCloseEnough()
    {
        float playerDistanceX = Mathf.Abs(transform.position.x - Camera.main.transform.position.x);

        return playerDistanceX < openDistance;
    }

    // 检查摄像机与生成点的距离，如果小于激活距离，返回true，否则返回false
    private bool IsPlayerFarEnough()
    {
        float playerDistanceX = Mathf.Abs(transform.position.x - Camera.main.transform.position.x);

        return playerDistanceX > closeDistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            anim.SetBool("open", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetBool("open", false);
        }
    }
}
