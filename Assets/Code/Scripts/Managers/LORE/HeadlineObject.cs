using TMPro;
using UnityEngine;

public class HeadlineObject : MonoBehaviour
{
    [SerializeField] private GameObject headlineObject;
    [SerializeField] private TextMeshProUGUI headlineText;

	private void Start()
	{
        headlineObject.SetActive(false);
	}
    public void SetText(float fontSize, string text)
    {
        headlineObject.SetActive(true);
        headlineText.text = text;
        headlineText.fontSize = fontSize;
    }
}
