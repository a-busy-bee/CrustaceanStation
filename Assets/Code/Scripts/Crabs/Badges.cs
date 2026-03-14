using UnityEngine;
using UnityEngine.UI;
public class Badges : MonoBehaviour
{
    [SerializeField] private Sprite[] labCoatBadges;
    [SerializeField] private Image labCoatBadgeBase;
    public void Show()
    {
        labCoatBadgeBase.sprite = labCoatBadges[Random.Range(0, labCoatBadges.Length)];
    }
}
