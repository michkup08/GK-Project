using TMPro;
using UnityEngine;

/// <summary>
/// The Level4WaterReset class is responsible for resetting the player's position when the player is underwater.
/// It checks if the player is underwater and if so, starts a countdown.
/// If the player stays underwater for a certain amount of time, the player's position is reset.
/// </summary>
public class Level4WaterReset : MonoBehaviour
{
    /// <value><c>waterLevel</c> is the y-coordinate of the water's surface.</value>
    private double waterLevel;

    /// <value><c>playerHeight</c> is the height of the player.</value>
    private double playerHeight;

    /// <value><c>underWaterText</c> is the TextMeshPro component where the countdown is displayed.</value>
    [SerializeField]
    private TMP_Text underWaterText;

    /// <value><c>player</c> is the player's GameObject.</value>
    private GameObject player;

    /// <value><c>startPoint</c> is the Transform where the player is reset to after falling into the water.</value>
    [SerializeField]
    private Transform startPoint;

    /// <value><c>numberOfSecondsToWait</c> is the number of seconds to wait before resetting the player's position.</value>
    [SerializeField]
    private float numberOfSecondsToWait = 5;

    /// <value><c>timeElapsed</c> is the time in seconds that has elapsed since the player fell into the water.</value>
    private float timeElapsed = 0;

    /// <value><c>secondsElapsed</c> is the number of whole seconds that has elapsed since the player fell into the water.</value>
    private int secondsElapsed = 0;

    /// <summary>
    /// Initialization of player, playerHeight, and waterLevel variables.
    /// </summary>
    void Start()
    {
        waterLevel = GameObject.Find("Water Specular").transform.position.y;
        player = GameObject.Find("Player");
        playerHeight = player.transform.Find("Ch24_nonPBR").GetComponent<CapsuleCollider>().height;
    }

    /// <summary>
    /// Checks if the player has fallen into the water and if so, starts a countdown to reset the player's position.
    /// </summary>
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
                ResetPlayerPosition();
                timeElapsed = 0;
            }
        }
        else
        {
            underWaterText.gameObject.SetActive(false);
            timeElapsed = 0;
        }
    }

    /// <summary>
    /// The ResetPlayerPosition method resets the player's position to the startPoint and stops any movement.
    /// </summary>
    private void ResetPlayerPosition()
    {
        var rigidBody = player.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.zero;
        player.transform.position = startPoint.position;
        rigidBody.MovePosition(startPoint.position);
    }
}