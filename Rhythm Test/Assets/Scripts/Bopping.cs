using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bopping : MonoBehaviour
{
    private Animator anim;

    private Conductor conductor;

    public float beatsBtwBops;

    private float nextBeat;

    private void Start()
    {
        anim = GetComponent<Animator>();
        conductor = FindObjectOfType<Conductor>();

        SetBeat();
    }

    void SetBeat()
    {
        nextBeat = Mathf.Round(conductor.songPositionInBeats) + beatsBtwBops;
    }

    private void Update()
    {
        Bop();
    }

    void Bop()
    {
        if (conductor.songPositionInBeats >= nextBeat)
        {
            anim.SetTrigger("Bop");

            nextBeat = nextBeat + beatsBtwBops;
        }
    }
}
