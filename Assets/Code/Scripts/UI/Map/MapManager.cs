using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
public class MapManager : MonoBehaviour
{
    public static MapManager instance { get; private set; }
    [SerializeField] private MenuButtons menuButtons;
    [SerializeField] private GameObject screen;
    [SerializeField] private MapStationIcon[] stationIcons;
    [SerializeField] private GameObject[] miniMapSelectedIcons;
    private MapStationIcon curr;

    [Header("HoverCard")]
    [SerializeField] private GameObject hoverCard;
    [SerializeField] private TextMeshProUGUI locationTitle;
    [SerializeField] private TextMeshProUGUI stat1Text;
    [SerializeField] private TextMeshProUGUI stat2Text;
    [SerializeField] private TextMeshProUGUI stat3Text;
    [SerializeField] private TextMeshProUGUI stat1;
    [SerializeField] private TextMeshProUGUI stat2;
    [SerializeField] private TextMeshProUGUI stat3;

    private Dictionary<MapStationIcon.Station, Vector2> hoverPositions = new Dictionary<MapStationIcon.Station, Vector2>
    {
        {MapStationIcon.Station.Base,       new Vector2(-108,     -139)},
        {MapStationIcon.Station.Tropic,     new Vector2(165f,     -167)},
        {MapStationIcon.Station.Shallow,    new Vector2(271,       127)},
        {MapStationIcon.Station.Metro,      new Vector2(-463,     -139.3f)},
        {MapStationIcon.Station.Desert,     new Vector2(-214,     -177.5f)},
        {MapStationIcon.Station.Deep,       new Vector2(-40f,      0)},
        {MapStationIcon.Station.Antarctic,  new Vector2(0,         153)},
    };
    private Dictionary<MapStationIcon.Station, string[]> statsText = new Dictionary<MapStationIcon.Station, string[]>
    {
        {MapStationIcon.Station.Base,       new string[3] {"Passengers seated", "Trains sent",      "Agents stopped"}},
        {MapStationIcon.Station.Tropic,     new string[3] {"Storms survived",   "Tourists seen",    "Agents stopped"}},
        {MapStationIcon.Station.Shallow,    new string[3] {"Passengers seated", "Trains sent",      "Agents stopped"}},
        {MapStationIcon.Station.Metro,      new string[3] {"Money made",        "Trash recycled",   "Agents stopped"}},
        {MapStationIcon.Station.Desert,     new string[3] {"Mirages seen",      "Trains sent",      "Agents stopped"}},
        {MapStationIcon.Station.Deep,       new string[3] {"Trains sent",       "Jellyfish seen",   "Agents stopped"}},
        {MapStationIcon.Station.Antarctic,  new string[3] {"Blizzards seen",    "Snowcrabs made",   "Agents stopped"}},
    };
    //TODO: update image based on station location

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

	private void Start()
	{
        curr = stationIcons[PlayerPrefs.GetInt("Location")];
        if (curr == null)
        {
            curr = stationIcons[0];
        }
        miniMapSelectedIcons[(int)curr.GetStation()].SetActive(true);

        screen.SetActive(false);
        hoverCard.SetActive(false);
	}

    public void Dropdown()
    {
        // TODO: animate dropdown
        screen.SetActive(true);
        screen.GetComponent<Animator>().Play("Down");

        StartCoroutine(WaitForIconInit());
    }

    private IEnumerator WaitForIconInit() {
        yield return new WaitForSeconds(0.1f);

        curr.SetSelectionCircleActive();
    }

    public void BringUp()
    {
        // TODO: animate bringing up

        screen.GetComponent<Animator>().Play("Up");
        PlayerPrefs.SetInt("Location", (int)curr.GetStation());
        //screen.SetActive(false);
    }

    public void ShowHoverInfo(MapStationIcon.Station station)
    {
        hoverCard.SetActive(true);
        hoverCard.GetComponent<RectTransform>().anchoredPosition = hoverPositions[station];
        locationTitle.text = stationIcons[(int)station].GetNameString();
        stat1Text.text = statsText[station][0];
        stat2Text.text = statsText[station][1];
        stat3Text.text = statsText[station][2];

        stat1.text = PlayerPrefs.GetInt(stationIcons[(int)station].GetNameString() + "stat1").ToString();
        stat2.text = PlayerPrefs.GetInt(stationIcons[(int)station].GetNameString() + "stat2").ToString();
        stat3.text = PlayerPrefs.GetInt(stationIcons[(int)station].GetNameString() + "stat3").ToString();
    }

    public void HideHoverInfo()
    {
        hoverCard.SetActive(false);
    }

    public void ClickedStation(MapStationIcon.Station station)
    {
        if (curr.GetStation() != station)
        {
            curr.HideSelectionCircle();
            miniMapSelectedIcons[(int)curr.GetStation()].SetActive(false);

            curr = stationIcons[(int)station];
            miniMapSelectedIcons[(int)station].SetActive(true);
        }
    }

    public void StartDay()
    {
        menuButtons.startDay(curr.GetNameString());
    }

}
