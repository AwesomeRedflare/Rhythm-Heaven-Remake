using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public float tempo;
    private float quarterNote;
    [SerializeField]
    private int beat = 0;

    public float accuracyAmount;
    private bool canHit = false;
    [SerializeField]
    private bool successfulHit = false;

    private float accuracy;
    private float perfectTime;
    private float hitAccuracy;

    private void Start()
    {
        quarterNote = 60 / tempo;

        StartCoroutine("Metronome");
    }

    private void Update()
    {
        if(canHit == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                successfulHit = true;
                FindObjectOfType<AudioManager>().Play("Hit");

                hitAccuracy = accuracy;
            }
        }
    }

    private void FixedUpdate()
    {
        if(canHit == true)
        {
            accuracy += Time.deltaTime;
            //Debug.Log(accuracy);
        }
    }

    IEnumerator Metronome()
    {
        if (beat == 4)
        {
            beat = 0;
        }

        FindObjectOfType<AudioManager>().Play("Tick");
        beat++;

        if (beat == 3)
        {
            StartCoroutine("RhythmSet");
        }

        yield return new WaitForSeconds(quarterNote);

        StartCoroutine("Metronome");
    }

    IEnumerator RhythmSet()
    {
        successfulHit = false;
        hitAccuracy = 0;

        FindObjectOfType<AudioManager>().Play("Cue");

        yield return new WaitForSeconds(quarterNote * accuracyAmount);

        accuracy = 0;
        canHit = true;
        //Debug.Log(canHit);

        yield return new WaitForSeconds(quarterNote - (quarterNote * accuracyAmount));

        //FindObjectOfType<AudioManager>().Play("Hit");
        perfectTime = accuracy;

        yield return new WaitForSeconds(quarterNote - (quarterNote * accuracyAmount));

        canHit = false;
        //Debug.Log(canHit);

        if(successfulHit == false)
        {
            Debug.Log("fail");
        }
        else
        {
            Debug.Log("success");
            Debug.Log("accuracy: " + (hitAccuracy - perfectTime));
        }
    }

    /*
    IEnumerator Timer()
    {
        accuracy = 0;

        while(canHit == true)
        {
            Debug.Log(accuracy);

            accuracy += Time.deltaTime;

            yield return null;
        }

        Debug.Log(perfectTime);
    }
    */
}
