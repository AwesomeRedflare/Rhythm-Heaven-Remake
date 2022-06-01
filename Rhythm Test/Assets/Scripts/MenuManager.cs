using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public LoadManager loadManager;

    public void PlayGame(string gameName)
    {
        loadManager.StartCoroutine(loadManager.StartSong(gameName));
    }
}
