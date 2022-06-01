using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Conductor conductor;
    private KarateManPatterns KaratePatterns;

    private Animator anim;

    public AudioSource hitSound;

    private bool wasPot = false;

    private void Start()
    {
        conductor = GameObject.FindGameObjectWithTag("Rhythm").GetComponent<Conductor>();
        KaratePatterns = GameObject.FindGameObjectWithTag("Rhythm").GetComponent<KarateManPatterns>();

        anim = GetComponent<Animator>();

        anim.speed = conductor.songBpm / 60;

        Destroy(gameObject, conductor.secPerBeat * 3);
    }

    private void Update()
    {
        if(KaratePatterns.pot == null && wasPot == false)
        {
            KaratePatterns.pot =  GetComponent<Pot>();
            wasPot = true;
        }
    }

    public void Hit()
    {
        anim.SetTrigger("Punch");
        hitSound.Play();
    }
}
