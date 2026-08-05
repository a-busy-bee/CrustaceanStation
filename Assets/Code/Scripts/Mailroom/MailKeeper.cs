using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MailKeeper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform rectTransform;
    //private bool isMoving = false;
    //private Vector3 endPos = new Vector3(-534, 693, 0);
    //private Vector3 currentVelocity;
    private Animator animator;


    [Header("Tapped")]
    [SerializeField] private GameObject coin;
    private bool isTapped = false;

    private void Awake()
    {
        //isMoving = true;
        animator = GetComponent<Animator>();

        StartCoroutine(WaitThenSummonDialogue());
    }

    private IEnumerator WaitThenSummonDialogue()
    {
        yield return new WaitForSeconds(1);

        int currDay = SaveManager.instance.GetProgression_CurrDay();
        DialogueManager.instance.ShowMailkeeperDialogue(currDay);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetTrigger("tapped");
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Punch, true);
        if (isTapped) return;

        isTapped = true;

        coin.SetActive(true);
        coin.GetComponent<Coin>().Clicked();

        AchievementManager.instance.UnlockAchievementProgressive(AchievementManager.AchievementTypeProgressive.heLikesIt, 1);
    }
}
