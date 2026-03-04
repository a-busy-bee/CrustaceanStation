using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MapStationIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum Station
    {
        Base,
        Tropic,
        Shallow,
        Metro,
        Desert,
        Deep,
        Antarctic
    }
    [SerializeField] private Station station;
    [SerializeField] private GameObject selectionCircle;
    private Dictionary<Station, string> stationStrings = new Dictionary<Station, string>
    {
        {Station.Base,      "BaseArea"},
        {Station.Tropic,    "Tropic"},
        {Station.Shallow,   "Shallow"},
        {Station.Metro,     "Metro"},
        {Station.Desert,    "Desert"},
        {Station.Deep,      "Deep"},
        {Station.Antarctic, "Antarctic"},
    };
    private bool unlocked = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt(stationStrings[station]) == 1)
        {
            unlocked = true;
        }

        selectionCircle.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //map manager show hoverInfo(station)
        MapManager.instance.ShowHoverInfo(station);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // map manager hide hoverInfo
        MapManager.instance.HideHoverInfo();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MapManager.instance.ClickedStation(station);
        selectionCircle.SetActive(true);
    }

    public void HideSelectionCircle()
    {
        selectionCircle.SetActive(false);
    }

    public bool isUnlocked()
    {
        return unlocked;
    }

    public string GetNameString()
    {
        return stationStrings[station];
    }

    public Station GetStation()
    {
        return station;
    }

    public void SetSelectionCircleActive()
    {
        selectionCircle.SetActive(true);
    }
}
