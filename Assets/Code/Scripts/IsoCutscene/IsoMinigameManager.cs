using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IsoMinigameManager : MonoBehaviour
{
    public static IsoMinigameManager instance { get; private set; }

    [Header("Minigame")]
    [SerializeField] private GameObject minigameScreen;
    [SerializeField] private Color[] isoColors;
    [SerializeField] private GameObject[] isos;
    [SerializeField] private GameObject isoParent;

    [Header("Iso Caught")]
    [SerializeField] private GameObject caughtScreen;
    [SerializeField] private SmoothLerp caughtBkgMovement;
    [SerializeField] private SmoothLerp caughtForegroundMovement;
    [SerializeField] private Image caughtIsoSprite;

    [Header("Iso Adopted")]
    [SerializeField] private GameObject adoptionScreen;
    [SerializeField] private SmoothLerp adoptBkgMovement;
    [SerializeField] private SmoothLerp adoptForegroundMovement;
    [SerializeField] private Image adoptIsoSprite;
    [SerializeField] private TextMeshProUGUI birthday;


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
        caughtScreen.SetActive(false);
        adoptionScreen.SetActive(false);

        for (int i = 0; i < isos.Length; i++)
        {
            isos[i].GetComponent<IsoController>().SetColor(isoColors[i]);
        }
    }

    public void IsopodSelected(Color color)
    {
        caughtScreen.SetActive(true);
        caughtIsoSprite.color = color;

        caughtBkgMovement.Move(new Vector2(0, 0), 0.25f);
        caughtForegroundMovement.Move(new Vector2(0, 0), 0.5f);

        StartCoroutine(WaitBeforeHidingScreen(minigameScreen));
    }

    public void ReleaseIso()
    {
        caughtIsoSprite.gameObject.GetComponent<IsoCaught>().GetReleased();
        minigameScreen.SetActive(true);

        StartCoroutine(WaitBeforeMoving());
    }

    public void AdoptIso()
    {
        caughtIsoSprite.gameObject.GetComponent<IsoCaught>().GetAdopted();
        StartCoroutine(WaitBeforeContinuing(true));
    }

    private IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(0.5f);

        caughtBkgMovement.Move(new Vector2(0, 1234), 0.25f);
        caughtForegroundMovement.Move(new Vector2(0, -767), 0.25f);

        StartCoroutine(WaitBeforeContinuing());
    }

    private IEnumerator WaitBeforeContinuing(bool isAdopted = false)
    {
        yield return new WaitForSeconds(1f);

        caughtScreen.SetActive(false);
        if (isAdopted)
        {
            adoptionScreen.SetActive(true);

            adoptIsoSprite.color = caughtIsoSprite.color;
            GenerateIsoBirthdayText();

            adoptBkgMovement.Move(new Vector2(0, 0), 0.25f);
            adoptForegroundMovement.Move(new Vector2(0, 0), 0.5f);
        }

    }

    private IEnumerator WaitBeforeHidingScreen(GameObject screen)
    {
        yield return new WaitForSeconds(0.5f);

        screen.SetActive(false);
    }

    private void GenerateIsoBirthdayText()
    {
        (string month, int days)[] year = {
            ("January", 31),
            ("February", 28),
            ("March", 31),
            ("April", 30),
            ("May", 31),
            ("June", 30),
            ("July", 31),
            ("August", 31),
            ("September", 30),
            ("October", 31),
            ("November", 30),
            ("December", 31),
        };

        (string month, int days) date = year[Random.Range(0, 12)];
        birthday.text = date.month + " " + Random.Range(1, date.days + 1).ToString();
    }

    public void SaveInfo()
    {
        // save iso name and birthday and color
    }
}
