using TMPro;
using UnityEngine;

public class VelocityText : MonoBehaviour
{
    public GameObject textMeshProVelocity;

    public float playerVelocity;

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
