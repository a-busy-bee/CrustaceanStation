using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IsoController : MonoBehaviour
{
    public enum IsoState
    {
        leavingHut,
        rolling,
        walking,
        still
    }
    private IsoState currState = IsoState.leavingHut;
    private IsoState prevState;

    // physics
    private const float torque = 5000;
    private const float torqueWalking = 0;
    private float currTorque = 5000;
    private const float linearDamping = 7.5f;
    private const float linearDampingWalking = 30f;
    private float currLinearDamping = 7.5f;
    private Rigidbody2D rb;
    private RectTransform rectTransform;

    // sprites
    private Color color;
    [SerializeField] private GameObject rollingSprite;
    [SerializeField] private GameObject walkingSprite;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(678, -344);

        currState = IsoState.leavingHut;

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
        currTorque = torque;
        rb.linearDamping = linearDamping;

        rollingSprite.SetActive(true);
        walkingSprite.SetActive(false); //TODO: add transition anim

        float randAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
        float magnitude = Random.Range(4000, 8000);
        Vector2 direction = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * magnitude;

        rb.AddForce(direction);
    }

    public void Walk(bool xAxisOnly = false)
    {
        // configure walking rigidbody
        
        rb.freezeRotation = true;
        currTorque = torqueWalking;
        rb.linearDamping = linearDampingWalking;
        rectTransform.eulerAngles = new Vector3(0, 0, 0);

        rollingSprite.SetActive(false);
        walkingSprite.SetActive(true);

        float randAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
        float magnitude = Random.Range(1000, 4000);

        if (randAngle > 0.5f * Mathf.PI && randAngle < 1.5f * Mathf.PI)
        {
            walkingSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            walkingSprite.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 180, 0);
        }

        Vector2 direction;
        if (xAxisOnly) direction = new Vector2(1, 0) * magnitude;
        else direction = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * magnitude;

        rb.AddForce(direction);
    }

    public void Still()
    {
        //TODO: maybe do a lil idle anim
        StartCoroutine(WaitThenSwitchStates());
    }

    private void FixedUpdate()
    {
        if (currState == IsoState.rolling)
        {
            float radius = 85.7f;
            float rollDirection = Mathf.Sign(rb.linearVelocity.x);
            rb.angularVelocity = -rollDirection * rb.linearVelocity.magnitude / radius * Mathf.Rad2Deg;
        }

        if (currState == IsoState.rolling || currState == IsoState.walking || currState == IsoState.leavingHut)
        {
            if (rb.linearVelocity.magnitude < 0.5f)
            {
                SetState(IsoState.still);
            }
        }
    }

    private IEnumerator WaitThenSwitchStates()
    {
        yield return new WaitForSeconds(Random.Range(0.3f, 3.5f));

        int newState = Random.Range(1, 3);
        SetState((IsoState)newState);
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
