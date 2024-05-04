using TMPro;
using UnityEngine;

public class Level4WaterResetPlayerPosition : MonoBehaviour
{
    private double waterLevel;
    private double playerHeight;

    [SerializeField]
    private TMP_Text underWaterText;

    private GameObject player;

    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private float numberOfSecondsToWait = 5;

    private float timeElapsed = 0;
    private int secondsElapsed = 0;

    void Start()
    {
        waterLevel = GameObject.Find("Water Specular").transform.position.y;
        player = GameObject.Find("Player");
        playerHeight = player.transform.Find("PlayerObject").GetComponent<CapsuleCollider>().height;
    }

    void Update()
    {
        if (waterLevel > (player.transform.position.y + playerHeight / 2) + 0.1)
        {
            timeElapsed += Time.deltaTime;
            secondsElapsed = (int)timeElapsed % 60;
            underWaterText.text = "Underwater!\n" + (numberOfSecondsToWait - secondsElapsed) + "\nseconds to reset";
            underWaterText.gameObject.SetActive(true);
            if (timeElapsed >= numberOfSecondsToWait)
            {
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.transform.position = startPoint.position;
                timeElapsed = 0;
            }
        }
        else
        {
            underWaterText.gameObject.SetActive(false);
            timeElapsed = 0;
        }
    }
}
