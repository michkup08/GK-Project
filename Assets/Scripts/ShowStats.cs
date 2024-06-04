using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ShowStats : MonoBehaviour
{
    public TMP_Text levelInfo;
    public int level, maxPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            levelInfo.gameObject.SetActive(true);
            int currentLevel = SaveSystem.Load().currentLevel;
            if(currentLevel > level)
            {
                int points = SaveSystem.Load().points[level - 1];
                levelInfo.text = "Level " + level + " succeed with " + points + "/" + maxPoints;
            }
            else
            {
                levelInfo.text = "Level " + level + " not succeed";
            }
            
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            levelInfo.gameObject.SetActive(false);
        }
    }
}
