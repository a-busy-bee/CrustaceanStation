using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button adoptButton;
    private string isoName;
    private string isoBirthMonth;
    private int isoBirthDay;
    private string isoColor;


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

        isoColor = color.ToHexString();

        //StartCoroutine(WaitBeforeHidingMinigameScreen());
    }
    private IEnumerator WaitBeforeHidingMinigameScreen()
    {
        yield return new WaitForSeconds(0.5f);

        minigameScreen.SetActive(false);
    }

    public void ReleaseIso()
    {
        caughtIsoSprite.gameObject.GetComponent<IsoCaught>().GetReleased();
        minigameScreen.SetActive(true);

        StartCoroutine(WaitBeforeHidingCaughtScreen());
    }
    private IEnumerator WaitBeforeHidingCaughtScreen()
    {
        yield return new WaitForSeconds(0.5f);

        caughtBkgMovement.Move(new Vector2(0, 1234), 0.25f);
        caughtForegroundMovement.Move(new Vector2(0, -767), 0.25f);

        //yield return new WaitForSeconds(1f);

        //caughtScreen.SetActive(false);
    }

    public void AdoptIso()
    {
        caughtIsoSprite.gameObject.GetComponent<IsoCaught>().GetAdopted();
        minigameScreen.SetActive(false);

        StartCoroutine(WaitBeforeContinuing(true));
    }

    private IEnumerator WaitBeforeContinuing(bool isAdopted = false)
    {
        yield return new WaitForSeconds(1f);

        adoptButton.interactable = false;
        adoptionScreen.SetActive(true);

        adoptIsoSprite.color = caughtIsoSprite.color;
        GenerateIsoBirthdayText();

        adoptBkgMovement.Move(new Vector2(0, 0), 0.25f);
        adoptForegroundMovement.Move(new Vector2(0, 0), 0.5f);

        yield return new WaitForSeconds(1f);
        
        caughtScreen.SetActive(false);
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
        int day = Random.Range(1, date.days + 1);
        birthday.text = date.month + " " + day.ToString();

        isoBirthMonth = date.month;
        isoBirthDay = day;
    }

    public void OnValueChanged()
    {
        if (nameField.text != "") adoptButton.interactable = true;
        else adoptButton.interactable = false;
    }

    public void SaveInfo()
    {
        isoName = nameField.text;
        // save iso name and birthday and color
        PlayerPrefs.SetString("IsoName", isoName);
        PlayerPrefs.SetString("IsoBirthdayMonth", isoBirthMonth);
        PlayerPrefs.SetInt("IsoBirthdayDay", isoBirthDay);
        PlayerPrefs.SetString("IsoColor", isoColor);

        Debug.Log(PlayerPrefs.GetString("IsoName"));
        Debug.Log(PlayerPrefs.GetString("IsoColor"));

        CutsceneManager.instance.SetCertificateShown();
        CutsceneManager.instance.ProgressScene();
    }
}
