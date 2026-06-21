using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IsoController : MonoBehaviour
{
    public enum IsoState
    {
        rolling,
        walking,
        still
    }
    private IsoState currState = IsoState.walking;
    private IsoState prevState;

    // movement
    private RectTransform rectTransform;
    private Vector2 speedRange = new Vector2(100f, 500f);   // min/max speeds
    private const float rollingSpeedRangeOffset = 250.0f;
    private const float circumference = 2f * Mathf.PI * 85;     // circumference of rolled up sprite
    private const float degPerSecAnim = 60f;                    // used to calculate rolling anim speed

    // set at runtime
    private Vector2 targetPos;  
    private Vector2 currPos;
    private float currSpeed;
    private float currProgress;

    // sprites
    private Color color;
    [SerializeField] private GameObject rollingSprite;
    [SerializeField] private GameObject walkingSprite;

    // misc
    [SerializeField] private Emotion emotionRolling;
    [SerializeField] private Emotion emotionWalking;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(WaitThenSwitchStates());
    }

    public void SetState(IsoState newState)
    {
        prevState = currState;
        currState = newState;
        switch (currState)
        {
            case IsoState.rolling:
                {
                    //Debug.Log("rolling");
                    Roll();
                }
                break;
            case IsoState.walking:
                {
                    //Debug.Log("walking");
                    Walk();
                }
                break;
            case IsoState.still:
                {
                    //Debug.Log("still");
                    Still();
                }
                break;
        }
    }

    public void Roll()
    {
        rollingSprite.SetActive(true);
        walkingSprite.SetActive(false); //TODO: add transition anim

        float targetX = Move();

        rollingSprite.GetComponent<Animator>().enabled = true;
        if (targetX > rectTransform.anchoredPosition.x)
        {
            //play anim in reverse
            rollingSprite.GetComponent<Animator>().Play("RollReverse");
        }
        else
        {
            rollingSprite.GetComponent<Animator>().Play("Roll");
        }
    }

    public void Walk()
    {
        rollingSprite.SetActive(false);
        walkingSprite.SetActive(true); //TODO: add transition anim

        float targetX = Move();

        if (targetX > rectTransform.anchoredPosition.x)
        {
            walkingSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 180, 0); // flip sprite
        }
        else
        {
            walkingSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void Still()
    {
        // TODO: maybe do a lil idle anim
        if (prevState == IsoState.rolling)
        {
            rollingSprite.GetComponent<Animator>().enabled = false;
        }

        StartCoroutine(WaitThenSwitchStates());
    }

    private void FixedUpdate()
    {
        if (currState == IsoState.walking)
        {
            currSpeed = Random.Range(speedRange.x, speedRange.y); // vary speed
        }

        if (currState == IsoState.rolling || currState == IsoState.walking)
        {
            Vector2 prevPos = rectTransform.anchoredPosition;

            float distToTarget = Vector2.Distance(currPos, targetPos);
            float durationLeft = Mathf.Max(distToTarget / currSpeed, 0.01f);
            currProgress += Time.fixedDeltaTime / durationLeft;

            float step = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(currProgress));
            rectTransform.anchoredPosition = Vector2.Lerp(currPos, targetPos, step);

            if (currState == IsoState.rolling)
            {
                float speed = Vector2.Distance(prevPos, rectTransform.anchoredPosition) / Time.fixedDeltaTime;
                float instRollSpeed = speed / circumference * 360f / degPerSecAnim; // girl dont even ask how i got these numbers ;-;

                rollingSprite.GetComponent<Animator>().speed = instRollSpeed;
            }

            if (currProgress >= 1f)
            {
                SetState(IsoState.still);
            }
        }
    }

    private IEnumerator WaitThenSwitchStates()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 5f));

        float rollChance = 0.25f;
        if (prevState == IsoState.rolling) rollChance = 0.5f;

        IsoState newState = IsoState.walking;
        if (Random.Range(0.0f, 1.0f) < rollChance) newState = IsoState.rolling;

        SetState(newState);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ouchie");

        if (Random.Range(0.0f, 1.0f) < 0.7f) return;

        if (currState == IsoState.rolling)
        {
            emotionRolling.PlayEmotion("any");
        }
        else if (currState == IsoState.walking)
        {
            emotionWalking.PlayEmotion("any");
        }
    }

	public void SetColor(Color newColor)
    {
        rollingSprite.GetComponent<Image>().color = newColor;
        walkingSprite.GetComponent<Image>().color = newColor;
        color = newColor;
    }

    private float Move() // helper func for Roll and Walk
    {
        float targetX = Random.Range(-773, 810);
        float targetY = Random.Range(-378, 388);

        targetPos = new Vector2(targetX, targetY);
        currPos = rectTransform.anchoredPosition;
        currProgress = 0f;

        return targetX;
    }
}
