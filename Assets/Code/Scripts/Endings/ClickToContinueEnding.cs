using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToContinueEnding : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CutscenePlayer_Dialogue cutscenePlayer_Good;
    public void OnPointerClick(PointerEventData data)
    {
        cutscenePlayer_Good.ClickToContinue();
    }
}
