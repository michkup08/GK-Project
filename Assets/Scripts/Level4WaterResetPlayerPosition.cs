using TMPro;
using UnityEngine;

public class Level5LavaReset : MonoBehaviour
{
    private double lavaLevel;
    private double playerHeight;

    [SerializeField]
    private TMP_Text textGUI;

    private GameObject player;

    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private float numberOfSecondsToWait = 5;

    [SerializeField]
    private GameObject lavaObject;

    private float timeElapsed = 0;
    private int secondsElapsed = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        playerHeight = player.transform.position.y;
        lavaLevel = lavaObject.transform.position.y;
    }

    void Update()
    {
        if (player.transform.position.y - playerHeight / 2 - 0.2f <= lavaLevel)
        {
            player.transform.Find("PlayerObject/PlayerFire").gameObject.SetActive(true);
            timeElapsed += Time.deltaTime;
            secondsElapsed = (int)timeElapsed % 60;
            textGUI.text = (numberOfSecondsToWait - secondsElapsed) + "\nseconds to reset";
            textGUI.gameObject.SetActive(true);
            if (timeElapsed >= numberOfSecondsToWait)
            {
                ResetPlayerPosition();
                timeElapsed = 0;
            }
        }
        else
        {
            textGUI.gameObject.SetActive(false);
            timeElapsed = 0;
        }
    }

    private void ResetPlayerPosition()
    {
        player.transform.Find("PlayerObject/PlayerFire").gameObject.SetActive(false);
        var rigidBody = player.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.zero;
        player.transform.position = startPoint.position;
        rigidBody.MovePosition(startPoint.position);
    }
}
