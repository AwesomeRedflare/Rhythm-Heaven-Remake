using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    public Text speakerText;
    public Text descriptionText;

    public static float score;
    public static float totalScore;

    public static string speaker;
    public static string[] evaluations;

    public SpriteRenderer resultSprite;

    public Sprite[] results;

    private void Start()
    {
        speakerText.text = speaker;
        speakerText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        resultSprite.gameObject.SetActive(false);

        if(score > (totalScore * .8))
        {
            resultSprite.sprite = results[0];
            descriptionText.text = evaluations[2];
        }

        if (score > (totalScore * .6) && score < (totalScore * .8))
        {
            resultSprite.sprite = results[1];
            descriptionText.text = evaluations[1];
        }

        if (score < (totalScore * .6))
        {
            resultSprite.sprite = results[2];
            descriptionText.text = evaluations[0];
        }

        StartCoroutine("Results");
    }

    IEnumerator Results()
    {
        yield return new WaitForSeconds(1);

        speakerText.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Ding");

        yield return new WaitForSeconds(1);

        descriptionText.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Ding");

        yield return new WaitForSeconds(2);

        resultSprite.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Ding");
    }
}
