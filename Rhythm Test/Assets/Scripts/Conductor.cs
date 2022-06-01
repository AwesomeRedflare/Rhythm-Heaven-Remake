using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;
    //The number of seconds for each song beat (quarter note length in seconds)
    public float secPerBeat;
    //Current song position, in seconds
    public float songPosition;
    //Current song position, in beats
    public float songPositionInBeats;
    //How many seconds have passed since the song started
    public float dspSongTime;
    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;
    //The offset to the first beat of the song in seconds (for if the audio file does not start right away, but has a pause)
    public float firstBeatOffset;

    public LoadManager loadManager;

    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        Invoke("StartSong", 1f);
    }

    void StartSong()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }

    void Update()
    {
        if(musicSource.isPlaying == true || songPositionInBeats > 2)
        {
            //determine how many seconds since the song started
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

            //determine how many beats since the song started
            songPositionInBeats = (songPosition / secPerBeat) + 1;
        }
    }
}
