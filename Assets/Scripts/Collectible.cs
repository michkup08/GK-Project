using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int collectedCounter = 0;
    LevelStatistics levelStatistics;

    void Start()
    {
        levelStatistics = GetComponent<LevelStatistics>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            collectedCounter++;
            levelStatistics.collectedCount = collectedCounter;
        }
    }
}
