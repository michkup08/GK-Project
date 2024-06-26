using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages interactions with objects based on raycasting from the player's position.
/// </summary>
public class InteractionWithObjects : MonoBehaviour
{
    /// <value> Reference to the GameObject displaying interaction information. </value>
    [Header("References")]
    public GameObject Interactinfo;
    /// <value> Layer mask for interactable objects. </value>
    public LayerMask interactable;
    /// <value> Default layer mask. </value>
    public LayerMask defaultLayer;
    /// <value> Reference to the player's GameObject. </value>
    public GameObject playerObject;
    /// <value> Type of interaction ("animate" or "exit map"). </value>
    public string interaction = "animate";
    /// <value> Reference to the LevelStatistics script. </value>
    public LevelStatistics ls;
    /// <value> Level to unlock upon interaction. </value>
    public int levelUnlocked;

    /// <value> Distance of the raycast for interaction. </value>
    [Header("Variables")]
    public float rayDistance;

    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(playerObject.transform.position, playerObject.transform.forward, out hit, rayDistance, interactable))
        {
            Debug.Log("can interact");
            Interactinfo.gameObject.SetActive(true);
            GameObject hitted = hit.collider.transform.root.gameObject;
            Animator animator = hitted.GetComponent<Animator>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (interaction == "animate")
                {
                    animator.SetTrigger("interact");
                    hitted.layer = defaultLayer;
                }
                else if (interaction == "exit map")
                {
                    SaveSystem.updateLevel(levelUnlocked, ls.collectedCount);
                    SceneManager.LoadScene("MainLocation");
                }
            }
        }
        else
        {
            Interactinfo.gameObject.SetActive(false);
        }
    }
}
