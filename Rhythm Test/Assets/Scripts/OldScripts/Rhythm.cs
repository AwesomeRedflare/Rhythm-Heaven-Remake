using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm : MonoBehaviour
{
    public Song song;

    public float tempo;
    [HideInInspector]
    public float quarterNote;
    [HideInInspector]
    public float beatTimer;
    public int beat;

    public LoadManager loadManager;

    private void Start()
    {
        //beat = song.IntroAmount;

        song = GetComponent<Song>();
        
        loadManager = GameObject.FindGameObjectWithTag("load").GetComponent<LoadManager>();

        quarterNote = 60f / tempo;
    }

    private void Update()
    {
        Metronome();
    }

    void Metronome()
    {
        beatTimer -= Time.deltaTime;

        if (beatTimer <= 0)
        {
            if (beat < 0)
            {
                FindObjectOfType<AudioManager>().Play("Tick");
            }

            beat++;

            if (beat == 1)
            {
                FindObjectOfType<AudioManager>().Play(song.SongName);
            }

            song.BeatChecker();

            beatTimer += quarterNote;
        }

    }

}
