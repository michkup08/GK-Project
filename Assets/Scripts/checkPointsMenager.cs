using UnityEngine;

public class checkPointsMenager : MonoBehaviour
{
    Vector3 AfterDeadPosition;
    public Transform Player;
    public bool trigger = false;
    void Start()
    {
        AfterDeadPosition = Player.position;
    }

    public Vector3 getPosition()
    {
        trigger = true;
        return AfterDeadPosition;

    }

    public void setPosition(Vector3 position)
    {
        AfterDeadPosition = position;
    }

    private void Update()
    {

    }
}
