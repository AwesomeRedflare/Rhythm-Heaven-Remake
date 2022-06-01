using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaterPatterns : MonoBehaviour
{
    public Conductor conductor;

    public float accuracyAmount;
    private bool canHit = false;
    private bool successfulHit = false;

    private float accuracy;
    private float perfectTime;
    private float hitAccuracy;
    private bool canAccuracyCheck;

    public float halfMissAccuracy;
    private bool canHalfMiss;
    private bool didHalfMiss = false;

    public Animator playerAnim;
    public GameObject obstacle;

    private void Start()
    {
        conductor = GetComponent<Conductor>();
    }

    private void Update()
    {
        Accuracy();
        Inputs();
    }

    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerAction();
        }
    }

    void PlayerAction()
    {
        if (canHit == true && canHalfMiss == false)
        {
            successfulHit = true;
            ResultScreen.score++;
            canHit = false;

            hitAccuracy = accuracy;
        }

        if (canHit == true && canHalfMiss == true)
        {
            successfulHit = true;
            didHalfMiss = true;
            canHalfMiss = true;
            canHit = false;
            playerAnim.SetTrigger("HalfMiss");
            FindObjectOfType<AudioManager>().Play("HalfMiss");

            hitAccuracy = accuracy;
        }

        if (canHalfMiss == false)
        {
            playerAnim.SetTrigger("Jump");
            FindObjectOfType<AudioManager>().Play("Hit");
        }
    }

    void Accuracy()
    {
        if (canAccuracyCheck == true)
        {
            accuracy += Time.deltaTime;
        }
    }

    void FixObstacle()
    {
        GameObject o = Instantiate(obstacle);

        o.GetComponent<Animator>().speed = conductor.songBpm / 60;

        Destroy(o, conductor.secPerBeat * 4);
    }

    public void JumpCue()
    {
        StartCoroutine("HalfMissCheck");
        StartCoroutine("SkaterRhythm");
    }

    IEnumerator SkaterRhythm()
    {
        FixObstacle();
        FindObjectOfType<AudioManager>().Play("Cue");

        yield return new WaitForSeconds(conductor.secPerBeat * accuracyAmount);

        successfulHit = false;
        didHalfMiss = false;
        hitAccuracy = 0;

        accuracy = 0;
        canHit = true;
        canAccuracyCheck = true;

        yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount));

        perfectTime = accuracy;

        yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount));

        canHit = false;
        canAccuracyCheck = false;

        yield return new WaitForSeconds(conductor.secPerBeat * halfMissAccuracy);

        if (successfulHit == false && didHalfMiss == false)
        {
            FindObjectOfType<AudioManager>().Play("Miss");
            playerAnim.SetTrigger("Miss");
            Debug.Log("fail");
        }

        if (successfulHit == true && didHalfMiss == false)
        {
            Debug.Log("success");
            Debug.Log("accuracy: " + (hitAccuracy - perfectTime));
        }

        if (successfulHit == true && didHalfMiss == true)
        {
            Debug.Log("HalfMiss");
            Debug.Log("accuracy: " + (hitAccuracy - perfectTime));
        }
    }

    IEnumerator HalfMissCheck()
    {
        yield return new WaitForSeconds(conductor.secPerBeat * accuracyAmount);

        canHalfMiss = true;

        yield return new WaitForSeconds(conductor.secPerBeat * halfMissAccuracy);

        canHalfMiss = false;

        yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount) - (conductor.secPerBeat * halfMissAccuracy));

        //middle

        yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount) - (conductor.secPerBeat * halfMissAccuracy));

        canHalfMiss = true;

        yield return new WaitForSeconds(conductor.secPerBeat * halfMissAccuracy);

        canHalfMiss = false;
    }
}


/*
 * Old Hit Detection Code  (from Karate Man)
 * 
IEnumerator KarateRhythm(GameObject obstacle)
{
    Instantiate(obstacle);
    FindObjectOfType<AudioManager>().Play("Cue");

    yield return new WaitForSeconds(conductor.secPerBeat * accuracyAmount);

    successfulHit = false;
    didHalfMiss = false;
    hitAccuracy = 0;

    accuracy = 0;
    canHit = true;
    canAccuracyCheck = true;

    yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount));

    perfectTime = accuracy;

    yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount));

    canHit = false;
    canAccuracyCheck = false;

    yield return new WaitForSeconds(conductor.secPerBeat * halfMissAccuracy);

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
        Debug.Log("accuracy: " + (hitAccuracy - perfectTime));
    }

    if (successfulHit == true && didHalfMiss == true)
    {
        Debug.Log("HalfMiss");
        Debug.Log("accuracy: " + (hitAccuracy - perfectTime));
    }
}

IEnumerator HalfMissCheck()
{
    yield return new WaitForSeconds(conductor.secPerBeat * accuracyAmount);

    canHalfMiss = true;

    yield return new WaitForSeconds(conductor.secPerBeat * halfMissAccuracy);

    canHalfMiss = false;

    yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount) - (conductor.secPerBeat * halfMissAccuracy));

    //middle

    yield return new WaitForSeconds(conductor.secPerBeat - (conductor.secPerBeat * accuracyAmount) - (conductor.secPerBeat * halfMissAccuracy));

    canHalfMiss = true;

    yield return new WaitForSeconds(conductor.secPerBeat * halfMissAccuracy);

    canHalfMiss = false;
}
*/
