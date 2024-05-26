using TMPro;

using UnityEngine;

/// <summary>
/// The Level5LavaReset class is responsible for resetting the player's position when the player fell into the lava.
/// It checks if the player fell into lava and if so, starts a countdown.
/// After a certain amount of time, the player's position is reset.
/// </summary>
public class Level5LavaReset : MonoBehaviour
{
    /// <value><c>lavaLevel</c> is the y-coordinate of the lava's surface.</value>
    private double lavaLevel;

    /// <value><c>playerHeight</c> is the height of the player.</value>
    private double playerHeight;

    /// <value><c>textGUI</c> is the TextMeshPro component where the countdown is displayed.</value>
    [SerializeField]
    private TMP_Text textGUI;

    /// <value><c>player</c> is the player's GameObject.</value>
    private GameObject player;

    /// <value><c>startPoint</c> is the Transform where the player is reset to after falling into the lava.</value>
    [SerializeField]
    private Transform startPoint;

    /// <value><c>numberOfSecondsToWait</c> is the number of seconds to wait before resetting the player's position.</value>
    [SerializeField]
    private float numberOfSecondsToWait = 5;

    /// <value><c>lavaObject</c> is the GameObject of the lava.</value>
    [SerializeField]
    private GameObject lavaObject;

    /// <value><c>timeElapsed</c> is the time in seconds that has elapsed since the player fell into the lava.</value>
    private float timeElapsed = 0;

    /// <value><c>secondsElapsed</c> is the number of whole seconds that has elapsed since the player fell into the lava.</value>
    private int secondsElapsed = 0;

    /// <summary>
    /// The Start method is called before the first frame update. It initializes the player, playerHeight, and lavaLevel variables.
    /// </summary>
    void Start()
    {
        player = GameObject.Find("Player");
        playerHeight = player.transform.position.y;
        lavaLevel = lavaObject.transform.position.y;
    }

    /// <summary>
    /// The Update method is called once per frame. It checks if the player has fallen into the lava and if so, starts a countdown to reset the player's position.
    /// </summary>
    void Update()
    {
        if (player.transform.position.y - playerHeight / 2 - 0.2f <= lavaLevel)
        {
            player.transform.Find("Ch24_nonPBR/PlayerFire").gameObject.SetActive(true);
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

    /// <summary>
    /// The ResetPlayerPosition method resets the player's position to the startPoint and stops any movement.
    /// </summary>
    private void ResetPlayerPosition()
    {
        player.transform.Find("Ch24_nonPBR/PlayerFire").gameObject.SetActive(false);
        var rigidBody = player.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.zero;
        player.transform.position = startPoint.position;
        rigidBody.MovePosition(startPoint.position);
    }
}