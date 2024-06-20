using UnityEngine;

public class AvalibleIfLevel : MonoBehaviour
{
    public int levelToActive;
    void Start()
    {
        if (SaveSystem.Load().currentLevel < levelToActive)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
