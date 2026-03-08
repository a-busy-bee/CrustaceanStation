using UnityEngine;
using UnityEngine.UI;

public class DialogueSizing : MonoBehaviour
{
    private float minWidth = 118.36f;
    private float maxWidth = 653.07f;

    [ContextMenu("SizeChange")]
    public void OnSizeChange()
    {
        if (GetComponent<RectTransform>().sizeDelta.x > maxWidth)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, GetComponent<RectTransform>().sizeDelta.y);
        }
    }
}


//TODO: have two dialogue bubbles: 1 for short text (shrink horizontally), 1 for long text (grow vertically, use preferred width)