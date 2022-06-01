using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTimer : MonoBehaviour
{
    public LoadManager loadManager;

    public string SongName;

    public float tempo;
    private float quarterNote;
    private float beatTimer;
    public int beat = 0;

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

    [TextArea(1, 3)]
    public string[] resultTexts;

    private int rhythmValue;
    private int rhythmBeat;
    public int endBeat;
    public int[] rhythmTimings;


    private void Start()
    {
        loadManager = GameObject.FindGameObjectWithTag("load").GetComponent<LoadManager>();
        ResultScreen.totalScore = rhythmTimings.Length;

        ResultScreen.score = 0;
        ResultScreen.evaluations = resultTexts;

        rhythmValue = 0;
        rhythmBeat = (int)rhythmTimings.GetValue(rhythmValue);

        quarterNote = 60f / tempo;

        UpdateAnimations();
    }

    private void Update()
    {
        Metronome();
        Accuracy();
        InputActive();
    }

    void UpdateAnimations()
    {
        Animator[] animations = FindObjectsOfType<Animator>();

        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].speed = tempo / 60f;
        }
    }

    void Metronome()
    {
        beatTimer -= Time.deltaTime;

        if (beatTimer <= 0)
        {
            if(beat < 4)
            {
                FindObjectOfType<AudioManager>().Play("Tick");
            }

            beat++;

            if(beat == 5)
            {
                FindObjectOfType<AudioManager>().Play(SongName);
            }

            RhythmSongPattern();

            beatTimer += quarterNote;
        }

    }

    void RhythmSongPattern()
    {
        if (beat == rhythmBeat)
        {
            StartCoroutine("RhythmType");
            StartCoroutine("HalfMissCheck");

            rhythmValue++;

            if(rhythmValue < rhythmTimings.Length)
            {
                rhythmBeat = (int)rhythmTimings.GetValue(rhythmValue);

                if ((int)rhythmTimings.GetValue(rhythmValue) < (int)rhythmTimings.GetValue(rhythmValue - 1))
                {
                    Debug.LogError("Next Rhythm beat can't be less than the previous one. " + (int)rhythmTimings.GetValue(rhythmValue - 1) + " can't go to " + rhythmTimings.GetValue(rhythmValue));
                }
            }
        }

        if(beat == endBeat)
        {
            loadManager.StartCoroutine(loadManager.EndSong(quarterNote * 2));
        }
    }

    void Accuracy()
    {
        if (canAccuracyCheck == true)
        {
            accuracy += Time.deltaTime;
        }
    }

    void InputActive()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canHit == true && canHalfMiss == false)
            {
                successfulHit = true;
                ResultScreen.score++;
                canHit = false;
                FindObjectOfType<AudioManager>().Play("Hit");

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

            if(canHalfMiss == false)
            {
                playerAnim.SetTrigger("Jump");
            }
        }
    }

    void FixObstacle()
    {
        GameObject o = Instantiate(obstacle);

        o.GetComponent<Animator>().speed = tempo / 60;

        Destroy(o, quarterNote * 4);
    }

    IEnumerator RhythmType()
    {
        FixObstacle();
        FindObjectOfType<AudioManager>().Play("Cue");

        yield return new WaitForSeconds(quarterNote * accuracyAmount);

        successfulHit = false;
        didHalfMiss = false;
        hitAccuracy = 0;

        accuracy = 0;
        canHit = true;
        canAccuracyCheck = true;

        yield return new WaitForSeconds(quarterNote - (quarterNote * accuracyAmount));

        perfectTime = accuracy;

        yield return new WaitForSeconds(quarterNote - (quarterNote * accuracyAmount));

        canHit = false;
        canAccuracyCheck = false;

        yield return new WaitForSeconds(quarterNote * halfMissAccuracy);

        if (successfulHit == false && didHalfMiss == false)
        {
            FindObjectOfType<AudioManager>().Play("Miss");
            playerAnim.SetTrigger("Miss");
            Debug.Log("fail");
        }
        
        if(successfulHit == true && didHalfMiss == false)
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
        yield return new WaitForSeconds(quarterNote * accuracyAmount);

        canHalfMiss = true;

        yield return new WaitForSeconds(quarterNote * halfMissAccuracy);

        canHalfMiss = false;

        yield return new WaitForSeconds(quarterNote - (quarterNote * accuracyAmount) - (quarterNote * halfMissAccuracy));

        //middle

        yield return new WaitForSeconds(quarterNote - (quarterNote * accuracyAmount) - (quarterNote * halfMissAccuracy));

        canHalfMiss = true;

        yield return new WaitForSeconds(quarterNote * halfMissAccuracy);

        canHalfMiss = false;
    }
}
