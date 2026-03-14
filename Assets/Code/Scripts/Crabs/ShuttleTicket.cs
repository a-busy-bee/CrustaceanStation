using UnityEngine;

public class ShuttleTicket : Ticket
{
    override public void BringForward()
    {
        // rotate
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        // move position
        rectTransform.anchoredPosition = new Vector3(-384, -450.8f, 44.85482f);

        // remove blur
        blur.SetActive(false);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetAsLastSibling();

        id.PushBack();
    }

    
}
