using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointsMenager : MonoBehaviour
{
    Vector3 AfterDeadPosition;
    public Transform Player;

    void Start()
    {
        AfterDeadPosition = Player.position;
    }

    public Vector3 getPosition()
    {
        return AfterDeadPosition;
    }

    public void setPosition(Vector3 position)
    {
        AfterDeadPosition = position;
    }

    private void Update()
    {
        Debug.Log("position: " + AfterDeadPosition.x + AfterDeadPosition.y + AfterDeadPosition.z);
    }
}
