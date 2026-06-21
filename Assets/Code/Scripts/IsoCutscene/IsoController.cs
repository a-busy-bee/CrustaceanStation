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

    // physics
    private Rigidbody2D rb;
    private RectTransform rectTransform;
    private Vector2 targetPos;
    private float walkSpeed;
    private Vector2 walkSpeedRange = new Vector2(100f, 500f);
    private float walkProgress;
    private Vector2 currPos;

    // sprites
    private Color color;
    [SerializeField] private GameObject rollingSprite;
    [SerializeField] private GameObject walkingSprite;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);

        currState = IsoState.walking;

        // walk forward a bit, then let loose
        Walk(true);
    }

    public void SetState(IsoState newState)
    {
        prevState = currState;
        currState = newState;
        switch (currState)
        {
            case IsoState.rolling:
                {
                    Debug.Log("rolling");
                    Roll();
                }
                break;
            case IsoState.walking:
                {
                    Debug.Log("walking");
                    Walk();
                }
                break;
            case IsoState.still:
                {
                    Debug.Log("still");
                    Still();
                }
                break;
        }
    }

    public void Roll()
    {
        // configure rolling rigidbody
        rb.freezeRotation = false;
        rb.angularDamping = 0.5f;

        rollingSprite.SetActive(true);
        walkingSprite.SetActive(false); //TODO: add transition anim

        //float currAngle = (walkingSprite.GetComponent<RectTransform>().eulerAngles.y > 90f) ? Mathf.PI : 0f;
        //float randAngle = currAngle + Random.Range(-0.75f, 0.75f);
        float randAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
        float magnitude = Random.Range(500, 1000);
        Vector2 direction = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * magnitude;

        rb.AddForce(direction);

        float rollDirection = Mathf.Sign(direction.x);
        rb.AddTorque(-rollDirection * magnitude * 0.5f);
    }

    public void Walk(bool xAxisOnly = false)
    {
        rb.freezeRotation = true;
        rectTransform.eulerAngles = new Vector3(0, 0, 0);

        rollingSprite.SetActive(false);
        walkingSprite.SetActive(true); //TODO: add transition anim

        // walk using transform (choose rand coord in square, walk to it)
        float targetX = Random.Range(-1394, 160);
        float targetY = Random.Range(-49, 704);
        if (xAxisOnly) targetY = rectTransform.anchoredPosition.y;
        targetPos = new Vector2(targetX, targetY);
        currPos = rectTransform.anchoredPosition;
        walkProgress = 0f;

        if (targetX > rectTransform.anchoredPosition.x)
        {
            walkingSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            walkingSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void Still()
    {
        // TODO: maybe do a lil idle anim
        rb.angularDamping = 0.5f;
        StartCoroutine(WaitThenSwitchStates());
    }

    private void FixedUpdate()
    {
        // tell ball to stop rolling
        if (currState == IsoState.rolling && rb.linearVelocity.magnitude < 0.5f)
        {
            SetState(IsoState.still);
        }
        // stuff while it's rolling
        else if (currState == IsoState.rolling)
        {
            float radius = 85.7f;
            float rollDirection = Mathf.Sign(rb.linearVelocity.x);
            float targetAngular = -rollDirection * (rb.linearVelocity.magnitude / radius) * Mathf.Rad2Deg * 10f;
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, targetAngular, 0.2f);
        }
        // stuff while it's walking
        else if (currState == IsoState.walking)
        {
            walkSpeed = Random.Range(walkSpeedRange.x, walkSpeedRange.y);
            float dist = Vector2.Distance(currPos, targetPos);
            float duration = Mathf.Max(dist / walkSpeed, 0.01f);
            walkProgress += Time.fixedDeltaTime / duration;

            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(walkProgress));
            rectTransform.anchoredPosition = Vector2.Lerp(currPos, targetPos, t);

            if (walkProgress >= 1f)
            {
                SetState(IsoState.still);
            }
        }
    }

    private IEnumerator WaitThenSwitchStates()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 5f));

        float rollChance = (prevState == IsoState.rolling) ? 0.5f : 0.25f;
        IsoState newState = (Random.Range(0.0f, 1.0f) < rollChance) ? IsoState.rolling : IsoState.walking;
        SetState(newState);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ouchie");
    }

    public void SetColor(Color color)
    {
        rollingSprite.GetComponent<Image>().color = color;
        walkingSprite.GetComponent<Image>().color = color;
    }
}
