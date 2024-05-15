using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to set the target frame rate for the application.
/// </summary>
public class FPSTarget : MonoBehaviour
{
    /// <value><c>targetFrameRate</c> represents the target frame rate for the application.</value>
    private int targetFrameRate = 144;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// It sets the vSyncCount to 0 and the target frame rate of the application.
    /// </summary>
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}
