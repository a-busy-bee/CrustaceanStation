using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SmoothLerp : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isMoving;

    private Vector2 currVelocity;
    private Vector2 targetPos;
    private float speed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isMoving)
        {
            rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, targetPos, ref currVelocity, speed);

            if (Vector2.Distance(rectTransform.anchoredPosition, targetPos) < 1f)
            {
                isMoving = false;
            }
        }
    }

    public void Move(Vector2 newTarget, float newSpeed)
    {
        isMoving = true;
        targetPos = newTarget;
        speed = newSpeed;
    }

    public void StopMoving() {
        isMoving = false;
    }
}
