using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public GameObject transition;

    private void Start()
    {
        transition = GameObject.FindGameObjectWithTag("transition");
    }

    public IEnumerator EndSong(float wait)
    {
        transition.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(wait);

        SceneManager.LoadScene("ResultScreen");
    }

    public IEnumerator StartSong(string name)
    {
        transition.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(name);
    }
}
