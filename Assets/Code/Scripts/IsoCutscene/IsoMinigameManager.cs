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
    [SerializeField] private Sprite[] largeIsoSprites;
    [SerializeField] private Sprite[] rolledIsoSprites;
    [SerializeField] private Sprite[] walkIsoSprites;
    [SerializeField] private GameObject[] isos;
    [SerializeField] private GameObject isoParent;

    public enum IsoColors
    {
        red,
        yellow,
        green,
        blue,
        purple
    }
    private Dictionary<IsoColors, Sprite> colorToLargeSprite = new Dictionary<IsoColors, Sprite>();
    private Dictionary<IsoColors, Sprite> colorToRolledSprite = new Dictionary<IsoColors, Sprite>();    // for adopted screen
    private Dictionary<IsoColors, Sprite> colorToWalkSprite = new Dictionary<IsoColors, Sprite>();    // for cutscenes after minigame

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
    private IsoColors isoColor;
    //private string isoColor;
    //private Color isoColorColor;


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

        //init dictionary to convert color to sprite
        for (int i = 0; i < largeIsoSprites.Length; i++)
        {
            colorToLargeSprite[(IsoColors)i] = largeIsoSprites[i];
            colorToRolledSprite[(IsoColors)i] = rolledIsoSprites[i];
            colorToWalkSprite[(IsoColors)i] = walkIsoSprites[i];
        }
    }

    private void Start()
    {
        caughtScreen.SetActive(false);
        adoptionScreen.SetActive(false);
    }

    public void IsopodSelected(IsoColors color)
    {
        caughtScreen.SetActive(true);

        Sprite largeSpriteColor = colorToLargeSprite[color];
        caughtIsoSprite.sprite = largeSpriteColor;
        isoColor = color;

        caughtBkgMovement.Move(new Vector2(0, 0), 0.25f);
        caughtForegroundMovement.Move(new Vector2(0, 0), 0.5f);

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

        StartCoroutine(WaitBeforeContinuing());
    }

    private IEnumerator WaitBeforeContinuing()
    {
        yield return new WaitForSeconds(0.75f);

        adoptButton.interactable = false;
        adoptionScreen.SetActive(true);

        adoptIsoSprite.sprite = colorToRolledSprite[isoColor];
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
        SaveManager.instance.SaveIsoData(isoName, isoBirthMonth, isoBirthDay, (int)isoColor);

        CutsceneManager.instance.SetCertificateShown();
        CutsceneManager.instance.ProgressScene();
    }

    public Sprite ConvertColorToRolledSprite(IsoColors color)
    {
        return colorToRolledSprite[color];
    }

    public Sprite ConvertColorToWalkSprite(IsoColors color)
    {
        return colorToWalkSprite[color];
    }

    public Sprite ConvertColorToLargeSprite(IsoColors color)
    {
        return colorToLargeSprite[color];
    }
}
