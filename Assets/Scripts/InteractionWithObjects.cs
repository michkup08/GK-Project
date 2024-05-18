using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class InteractionWithObjects : MonoBehaviour
{
    [Header("References")]
    public GameObject Interactinfo;
    public LayerMask interactable;
    public LayerMask defaultLayer;
    public GameObject playerObject;
    public string interaction = "animate";

    [Header("Variables")]
    public float rayDistance;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(playerObject.transform.position, playerObject.transform.forward, out hit, rayDistance, interactable))
        {
            Debug.Log("can interact");
            Interactinfo.gameObject.SetActive(true);
            GameObject hitted = hit.collider.transform.root.gameObject;
            Animator animator = hitted.GetComponent<Animator>();
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(interaction == "animate")
                {
                    animator.SetTrigger("interact");
                    hitted.layer = defaultLayer;
                }
                else if(interaction == "exit map")
                {
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
