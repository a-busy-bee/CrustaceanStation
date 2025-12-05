using System;
using UnityEngine;

public class Emotion : MonoBehaviour
{
    [SerializeField] private GameObject depressed;
    [SerializeField] private GameObject angry;
    [SerializeField] private GameObject confused;
    [SerializeField] private GameObject sad;

    private Action[] playEmotions;

    private void Start()
    {
        depressed.SetActive(false);
        angry.SetActive(false);
        confused.SetActive(false);
        sad.SetActive(false);

        playEmotions = new Action[] {
            PlayDepressed,
            PlayAngry,
            PlaySad,
            PlayConfused
        };
    }

	[ContextMenu("depressed")]
    public void PlayDepressed()
    {
        depressed.SetActive(true);
        depressed.GetComponent<Animator>().Play("depressed");
    }

    [ContextMenu("angry")]
    public void PlayAngry()
    {
        angry.SetActive(true);
        angry.GetComponent<Animator>().Play("angry");
    }

    [ContextMenu("sad")]
    public void PlaySad()
    {
        sad.SetActive(true);
        sad.GetComponent<Animator>().Play("sad");
    }
    
    [ContextMenu("confused")]
    public void PlayConfused()
    {
        confused.SetActive(true);
        confused.GetComponent<Animator>().Play("confused");
    }

    public void PlayEmotion(string emotion)
    {
        if (emotion == "any")
        {
            playEmotions[UnityEngine.Random.Range(0, 3)]();
        }
        else if (emotion == "any and confused") // confused only if correct crab but wrongly rejected
        {
            playEmotions[UnityEngine.Random.Range(0, 4)]();
        }
    }

}
