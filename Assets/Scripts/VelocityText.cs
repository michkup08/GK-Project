using TMPro;
using UnityEngine;

/// <summary>
/// Displays the current player velocity using TextMeshProUGUI.
/// </summary>
public class VelocityText : MonoBehaviour
{
    /// <value>
    /// The GameObject containing the TextMeshProUGUI component for displaying velocity.
    /// </value>
    public GameObject textMeshProVelocity;

    /// <value>
    /// The current velocity of the player.
    /// </value>
    public float playerVelocity;

    /// <summary>
    /// The TextMeshProUGUI component used to display the player velocity.
    /// </summary>
    TextMeshProUGUI textMeshProVelocityText;

    [SerializeField]
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProVelocityText = textMeshProVelocity.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = playerMovement.velocity;
        textMeshProVelocityText.text = "Velocity: " + playerVelocity.ToString("F2");
    }
}
