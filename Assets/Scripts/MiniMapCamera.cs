using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    void LateUpdate()
    {
        transform.position = player.position + new Vector3(0, 10, 0);
        transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
    }
}
