using UnityEngine;
using UnityEngine.EventSystems;

public class Switch : MonoBehaviour, IPointerClickHandler
{
    // when clicked, report to Rail to depart train

    private Rail rail;

    public void SetRail(Rail newRail)
    {
        rail = newRail;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("click");
        //play switch animation

        // depart train
        rail.DepartTrain();
    }
}
