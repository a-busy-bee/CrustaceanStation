using System;
using System.Collections;
using UnityEngine;

public class Emotion : MonoBehaviour
{
    [SerializeField] private GameObject depressed;
    [SerializeField] private GameObject angry;
    [SerializeField] private GameObject confused;
    [SerializeField] private GameObject sad;

    private Action[] playEmotions;
    private bool emoting;

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
        emoting = true;
        depressed.SetActive(true);
        depressed.GetComponent<Animator>().Play("depressed");
        StartCoroutine(WaitBeforeEmotingAgain());
    }

    [ContextMenu("angry")]
    public void PlayAngry()
    {
        emoting = true;
        angry.SetActive(true);
        angry.GetComponent<Animator>().Play("angry");
        StartCoroutine(WaitBeforeEmotingAgain());
    }

    [ContextMenu("sad")]
    public void PlaySad()
    {
        emoting = true;
        sad.SetActive(true);
        sad.GetComponent<Animator>().Play("sad");
        StartCoroutine(WaitBeforeEmotingAgain());
    }

    [ContextMenu("confused")]
    public void PlayConfused()
    {
        emoting = true;
        confused.SetActive(true);
        confused.GetComponent<Animator>().Play("confused");
        StartCoroutine(WaitBeforeEmotingAgain());
    }

    public void PlayEmotion(string emotion)
    {
        if (emoting) return;

        if (emotion == "any")
        {
            playEmotions[UnityEngine.Random.Range(0, 3)]();
        }
        else if (emotion == "any and confused") // confused only if correct crab but wrongly rejected
        {
            playEmotions[UnityEngine.Random.Range(0, 4)]();
        }
    }

    private IEnumerator WaitBeforeEmotingAgain()
    {
        yield return new WaitForSeconds(1);

        depressed.SetActive(false);
        angry.SetActive(false);
        confused.SetActive(false);
        sad.SetActive(false);

        emoting = false;
    }

}
