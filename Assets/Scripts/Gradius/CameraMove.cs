using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraState
{
    public enum TriggerType { Position, Object }

    public TriggerType triggerType;
    public float speed;
    public Vector3 direction;
    public Vector3 triggerPosition;
    public GameObject triggerObject;
}

public class CameraMove : MonoBehaviour
{
    public float baseSpeed = 1f;
    public Vector3 initMoveDirection = Vector3.right;

    private float speed;
    private Vector3 moveDirection;

    public CameraState[] cameraStates;
    private int currentStateIdx = 0;


    // Start is called before the first frame update
    void Start()
    {
        SetSpeed(baseSpeed);
        SetMoveDirection(initMoveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStateIdx < cameraStates.Length)
        {
            CameraState currentState = cameraStates[currentStateIdx];

            Debug.Log(currentStateIdx + ":" + currentState.triggerType);

            if (currentState.triggerType == CameraState.TriggerType.Object)
            {
                if (currentState.triggerObject == null)
                {
                    SetSpeed(currentState.speed);
                    SetMoveDirection(currentState.direction);
                    currentStateIdx++;
                }

                transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, currentState.triggerPosition, speed * Time.deltaTime);
                if (transform.position == currentState.triggerPosition)
                {
                    SetSpeed(currentState.speed);
                    SetMoveDirection(currentState.direction);
                    currentStateIdx++;
                }
            }
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetMoveDirection(Vector3 newDirection)
    {
        moveDirection = newDirection.normalized;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < cameraStates.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(cameraStates[i].triggerPosition, Vector3.one * 1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cameraStates[i].triggerPosition, cameraStates[i].triggerPosition + cameraStates[i].direction * cameraStates[i].speed);
        }
    }

    private void OnGUI()
    {
        for(int i = 0; i < cameraStates.Length; i++)
        {
            CameraState state = cameraStates[i];

            if(state.triggerType == CameraState.TriggerType.Position)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(state.triggerPosition);
                GUI.Label(new Rect(screenPos.x - 40, screenPos.y + 20, 160, 32), "CamState:" + i);
            }
            else
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(state.triggerObject.transform.position);
                GUI.Label(new Rect(screenPos.x - 40, screenPos.y + 20, 160, 32), "CamState:" + i);
            }
        }
    }
}
