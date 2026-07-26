using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToContinueEnding : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CutscenePlayer_Good cutscenePlayer_Good;
    public void OnPointerClick(PointerEventData data)
    {
        cutscenePlayer_Good.ClickToContinue();
    }
}
