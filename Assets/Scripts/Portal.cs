using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {


        SceneManager.LoadScene(sceneName);

    }
}
