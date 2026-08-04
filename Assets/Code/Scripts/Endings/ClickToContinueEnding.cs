using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToContinueEnding : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CutscenePlayer_Dialogue cutscenePlayer_Dialogue;
    public void OnPointerClick(PointerEventData data)
    {
        cutscenePlayer_Dialogue.ClickToContinue();
    }
}
