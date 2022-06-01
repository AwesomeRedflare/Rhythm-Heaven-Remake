using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarateManPatterns : MonoBehaviour
{
    public Conductor conductor;

    public float accuracyAmount;
    private bool canHit = false;
    private bool successfulHit = false;

    private float perfectTime;
    private float hitAccuracy;

    public float halfMissAccuracy;
    private bool canHalfMiss;
    private bool didHalfMiss = false;

    public Animator playerAnim;
    public Animator shockAnim;
    public Bopping BGBop;
    public Animator TextAnim;

    [HideInInspector]
    public Pot pot;

    private void Start()
    {
        conductor = GetComponent<Conductor>();
    }

    private void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            PlayerAction();
        }
    }

    void PlayerAction()
    {
        FindObjectOfType<AudioManager>().Play("ArmWoosh");
        playerAnim.SetTrigger("Punch");

        if (canHit == true && canHalfMiss == false)
        {
            successfulHit = true;
            ResultScreen.score++;
            canHit = false;

            FindObjectOfType<AudioManager>().Play("Hit");

            shockAnim.SetTrigger("Punch");

            pot.Hit();
            pot = null;

            hitAccuracy = conductor.songPositionInBeats;
        }

        if (canHit == true && canHalfMiss == true)
        {
            successfulHit = true;
            didHalfMiss = true;
            canHalfMiss = true;
            canHit = false;

            pot.GetComponent<Animator>().SetTrigger("HalfMiss");
            FindObjectOfType<AudioManager>().Play("HalfMiss");

            pot = null;

            hitAccuracy = conductor.songPositionInBeats;
        }
    }

    public void PotCue(GameObject obj)
    {
        StartCoroutine("KarateRhythm", obj);
    }

    IEnumerator KarateRhythm(GameObject obstacle)
    {
        Instantiate(obstacle);
        FindObjectOfType<AudioManager>().Play("Cue");

        float cueBeat = conductor.songPositionInBeats;

        //early
        while (conductor.songPositionInBeats <= cueBeat + accuracyAmount)
        {
            yield return null;
        }

        hitAccuracy = 0;
        successfulHit = false;
        didHalfMiss = false;
        canHit = true;
        canHalfMiss = true;

        while (conductor.songPositionInBeats <= cueBeat + accuracyAmount + halfMissAccuracy)
        {
            yield return null;
        }

        canHalfMiss = false;

        while (conductor.songPositionInBeats <= cueBeat + 1)
        {
            yield return null;
        }
        //perfect

        perfectTime = conductor.songPositionInBeats;

        while (conductor.songPositionInBeats <= cueBeat + 1 + (1 - accuracyAmount - halfMissAccuracy))
        {
            yield return null;
        }

        canHalfMiss = true;

        while (conductor.songPositionInBeats <= cueBeat + 1 + (1 - accuracyAmount))
        {
            yield return null;
        }
        //late

        canHalfMiss = false;
        canHit = false;

        if (successfulHit == false && didHalfMiss == false)
        {
            FindObjectOfType<AudioManager>().Play("Miss");
            playerAnim.SetTrigger("Miss");
            Debug.Log("fail");

            pot = null;
        }

        if (successfulHit == true && didHalfMiss == false)
        {
            Debug.Log("success");
            Debug.Log("accuracy: " + ((hitAccuracy - perfectTime)));
        }

        if (successfulHit == true && didHalfMiss == true)
        {
            Debug.Log("HalfMiss");
            Debug.Log("accuracy: " + (hitAccuracy - perfectTime));
        }
    }

    public void BackGroundOn()
    {
        BGBop.enabled = true;
        BGBop.gameObject.GetComponent<Animator>().SetTrigger("Bop");
    }

    public void BackGroundOff()
    {
        BGBop.gameObject.GetComponent<Animator>().SetTrigger("BGOff");
        BGBop.enabled = false;
    }

    public void HitThreeCue()
    {
        FindObjectOfType<AudioManager>().Play("HitSound");
        TextAnim.SetTrigger("Three");
    }

    public void ThreeSound()
    {
        FindObjectOfType<AudioManager>().Play("ThreeSound");
    }

    public void Smirk()
    {
        if(successfulHit == true)
        {
            playerAnim.SetTrigger("Smirk");
        }
    }
}
