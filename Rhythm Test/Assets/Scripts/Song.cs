using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{
    public Conductor conductor;

    public string SongName;

    public string adviceGiver;
    [TextArea(1, 3)]
    public string[] resultTexts;

    public Animator[] animations;

    public int endBeat;

    public Action[] rhythmEvents;
    private int rhythmValue;
    private int rhythmBeat;

    public Action[] events;
    private int eventValue;
    private int eventBeat;

    private bool hasEnded = false;

    private void Start()
    {
        conductor = GetComponent<Conductor>();

        rhythmValue = 0;
        rhythmBeat = (int)rhythmEvents[rhythmValue].startBeat;

        eventValue = 0;
        eventBeat = (int)events[eventValue].startBeat;

        ResultScreen.score = 0;
        ResultScreen.totalScore = rhythmEvents.Length;

        ResultScreen.evaluations = resultTexts;
        ResultScreen.speaker = adviceGiver;

        UpdateAnimations();
    }

    private void Update()
    {
        BeatChecker();
    }

    void UpdateAnimations()
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].speed = conductor.songBpm / 60f;
        }
    }


    public void BeatChecker()
    {
        //Rhythm Events
        if (conductor.songPositionInBeats >= rhythmBeat && rhythmValue < rhythmEvents.Length)
        {
            rhythmEvents[rhythmValue].action.Invoke();

            rhythmValue++;

            if (rhythmValue < rhythmEvents.Length)
            {
                rhythmBeat = (int)rhythmEvents[rhythmValue].startBeat;

                if (rhythmEvents[rhythmValue].startBeat <= rhythmEvents[rhythmValue - 1].startBeat)
                {
                    Debug.LogError("Next Rhythm beat can't be less than the previous one. " + rhythmEvents[rhythmValue - 1].startBeat + " can't go to " + rhythmEvents[rhythmValue].startBeat);
                }
            }
        }

        //Visual Events
        if (conductor.songPositionInBeats >= eventBeat && eventValue < events.Length)
        {
            events[eventValue].action.Invoke();

            eventValue++;

            if (eventValue < events.Length)
            {
                eventBeat = (int)events[eventValue].startBeat;

                if (events[eventValue].startBeat <= events[eventValue - 1].startBeat)
                {
                    Debug.LogError("Next Rhythm beat can't be less than the previous one. " + events[eventValue - 1].startBeat + " can't go to " + events[eventValue].startBeat);
                }
            }
        }

        if (conductor.songPositionInBeats >= endBeat && hasEnded == false)
        {
            hasEnded = true;
            conductor.loadManager.StartCoroutine(conductor.loadManager.EndSong(conductor.secPerBeat * 2));
        }
    }
}
