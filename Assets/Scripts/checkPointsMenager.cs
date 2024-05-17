using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointsMenager : MonoBehaviour
{
    Transform AfterDeadPosition;
    public Transform Player;

    void Start()
    {
        AfterDeadPosition = Player.transform;
    }

    public Transform getTransform()
    {
        return AfterDeadPosition;
    }

    public void setTransform(Transform transform)
    {
        AfterDeadPosition = transform;
    }
}
