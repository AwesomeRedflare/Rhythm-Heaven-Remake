using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public LoadManager loadManager;

    public float introWaitTime;
    public float waitTime;

    public AudioSource introSong;

    public Animator introAnim;

    public string gameName;

    private void Start()
    {
        loadManager = GameObject.FindGameObjectWithTag("load").GetComponent<LoadManager>();

        StartCoroutine("Intro");
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(introWaitTime);

        introSong.Play();

        introAnim.SetTrigger("Intro");

        yield return new WaitForSeconds(waitTime);

        loadManager.StartCoroutine(loadManager.StartSong(gameName));
    }
}
