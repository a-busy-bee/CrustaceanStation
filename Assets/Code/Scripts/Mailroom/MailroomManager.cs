using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System.Collections;

using Special = CrabInfo.SpecialCharacter;
public class MailroomManager : MonoBehaviour
{
    public static MailroomManager instance { get; private set; }

    [SerializeField] private Mailbox mailbox;
    private LettersManager lettersManager;

    [Header("Letter Types")]
    [SerializeField] private GameObject letter;
    [SerializeField] private GameObject feedbackForm;
    [SerializeField] private TextMeshProUGUI feedbackFormName;
    [SerializeField] private GameObject largeNote;
    [SerializeField] private GameObject smallNote;
    private GameObject[] notes;

    [SerializeField] private Image letterImage;
    [SerializeField] private Sprite[] letterSprites;

    [Header("UI")]
    [SerializeField] private GameObject crabdexNotif;
    [SerializeField] private GameObject hideLetterButton;
    [SerializeField] private GameObject backgroundOverlay;


    // MOVEMENT
    public enum LetterState
    {
        noLetter,
        letterMovingUp,
        letterOpen,
        letterMovingDown
    }
    private LetterState letterState;
    private int objectIdxMoving;
    private GameObject currentLetter;
    private bool isCurrentlyCrustyLetter = false;

    // POSITION
    private Vector2 letterYPos = new Vector2(-1423, 0);           // starting pos, ending pos
    private Vector2 feedbackFormYPos = new Vector2(-1259, -171);
    private Vector2 largeNoteYPos = new Vector2(-1171, 23);
    private Vector2 smallNoteYPos = new Vector2(-1463, -269);
    private Vector2[] yPos;
    private float currentVelocity;




    // JSON STUFF
    private PlotData plotData;
    private string defaultPath;
    private string savePath;

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

        // put all letter types into an array for easier preprocessing (set starting pos, turn off)
        notes = new GameObject[] { letter, feedbackForm, largeNote, smallNote };
        yPos = new Vector2[] { letterYPos, feedbackFormYPos, largeNoteYPos, smallNoteYPos };
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(notes[i].GetComponent<RectTransform>().anchoredPosition.x, yPos[i].x);
            notes[i].SetActive(false);
        }
    }

    private void Start()
    {
        lettersManager = GetComponent<LettersManager>();

        HideCrabdexNotif();
        hideLetterButton.SetActive(false);
        backgroundOverlay.SetActive(false);

        LoadJSON();
        plotData.inbox.OrderBy(m => m.timestamp).ToList();
        mailbox.SetSprite(plotData.inbox.Count);
    }

    public void SetState(LetterState newState)
    {
        LetterState prevState = letterState;
        letterState = newState;

        switch (letterState)
        {
            case LetterState.noLetter:
                // turn off button & crabdex notif
                HideCrabdexNotif();
                mailbox.SetInteractable(true);
                hideLetterButton.SetActive(false);
                backgroundOverlay.SetActive(false);
                break;

            case LetterState.letterOpen:
                // turn on button & crabdex notif
                ShowCrabdexNotif();
                hideLetterButton.SetActive(true);
                break;

            case LetterState.letterMovingUp:
                //mailbox.SetInteractable(false);
                backgroundOverlay.SetActive(true);
                if (isCurrentlyCrustyLetter) AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.Fired);
                break;

            case LetterState.letterMovingDown:
                HideCrabdexNotif();
                hideLetterButton.SetActive(false);
                backgroundOverlay.SetActive(false);

                if (isCurrentlyCrustyLetter) AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.Mailroom);
                break;
        }
    }

    private void LoadJSON()
    {
        defaultPath = Application.dataPath + "/Data/Inbox.json";
        savePath = Application.persistentDataPath + "/Inbox.json";

        if (File.Exists(savePath))
        {
            plotData = JsonUtility.FromJson<PlotData>(File.ReadAllText(savePath));
        }
        else
        {
            // load default file from resources
            string jsonText = File.ReadAllText(defaultPath);
            plotData = JsonUtility.FromJson<PlotData>(jsonText);

            File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
        }
    }

    public int GetNumRemainingLetters()
    {
        return plotData.inbox.Count();
    }


    public void SummonLetter()
    {
        // get topmost letter
        // set isRead
        // remove letter from list
        // update json file
        InboxItem inboxItem = plotData.inbox[0];
        plotData.inbox[0].isRead = true;
        isCurrentlyCrustyLetter = false;

        // save list w/o the topmost letter
        plotData.inbox.RemoveAt(0);
        File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));

        // figure out what letter type to show
        // set that object active
        // if text, access text from Letter or Feedback managers
        // if text, set text
        // animate letter coming up
        // show note that the letter/note/wtvr was saved in the crabdex
        // show button to close letter
        GameObject chosen = null;

        if (inboxItem.type == "letter")
        {
            letter.SetActive(true);
            string body = "";
            string top = "";
            string bottom = "";
            letterImage.sprite = letterSprites[0];

            switch (inboxItem.subType)
            {
                case "crustyCoDay":
                    FullStackLetterByDay letterByDay = lettersManager.GetCrustyCoLetterByDay(inboxItem.id);
                    body = letterByDay.body;
                    top = letterByDay.top;
                    bottom = letterByDay.bottom;

                    letterImage.sprite = letterSprites[0];

                    isCurrentlyCrustyLetter = true;
                    break;
                case "crustyCoDay_GoodEnding":
                    letterByDay = lettersManager.GetCrustyCoLetterByDay(inboxItem.id, true, true);
                    body = letterByDay.body;
                    top = letterByDay.top;
                    bottom = letterByDay.bottom;

                    letterImage.sprite = letterSprites[0];
                    isCurrentlyCrustyLetter = true;
                    break;
                case "crustyCoDay_BadEnding":
                    letterByDay = lettersManager.GetCrustyCoLetterByDay(inboxItem.id, true, false);
                    body = letterByDay.body;
                    top = letterByDay.top;
                    bottom = letterByDay.bottom;

                    letterImage.sprite = letterSprites[0];
                    isCurrentlyCrustyLetter = true;
                    break;
                case "crustyCoID":
                    FullStackLetterByID letterByID = lettersManager.GetCrustyCoLetterByID(inboxItem.id);
                    body = letterByID.body;
                    top = letterByID.top;
                    bottom = letterByID.bottom;

                    letterImage.sprite = letterSprites[0];
                    isCurrentlyCrustyLetter = true;
                    break;
 
                case "family":
                    FullStackLetter fullStackLetter = lettersManager.GetFamilyLetter(inboxItem.id);

                    body = fullStackLetter.body;
                    top = fullStackLetter.top;
                    bottom = fullStackLetter.bottom;

                    letterImage.sprite = letterSprites[1];
                    break;

                case "mailkeeper":
                    fullStackLetter = lettersManager.GetMailkeeperLetter(inboxItem.id);

                    body = fullStackLetter.body;
                    top = fullStackLetter.top;
                    bottom = fullStackLetter.bottom;

                    letterImage.sprite = letterSprites[1];
                    break;
            }

            letter.GetComponent<RectTransform>().GetChild(0).GetComponent<TextMeshProUGUI>().text = body;
            letter.GetComponent<RectTransform>().GetChild(1).GetComponent<TextMeshProUGUI>().text = top;
            letter.GetComponent<RectTransform>().GetChild(2).GetComponent<TextMeshProUGUI>().text = bottom;

            chosen = letter;
        }



        else if (inboxItem.type == "feedbackForm")
        {
            feedbackForm.SetActive(true);
            string text;

            if (inboxItem.subType == "generic")
            {
                text = GetComponent<FeedbackManager>().GetGenericFeedback();
                feedbackFormName.text = CrabNameGenerator.instance.GetAnyName();
            }
            else
            {
                Special special = Special.itty;
                switch (inboxItem.subType)
                {
                    case "itty":
                        special = Special.itty;
                        break;
                    case "protestorCatfish":
                        special = Special.protestorCatfish;
                        break;
                    case "horseshoe":
                        special = Special.horseshoe;
                        break;
                    case "isobelle":
                        special = Special.isobelle;
                        break;
                    case "seaStarDad":
                        special = Special.seaStarDad;
                        break;
                    case "granny":
                        special = Special.granny;
                        break;
                    case "gramps":
                        special = Special.gramps;
                        break;
                }

                text = GetComponent<FeedbackManager>().GetCharacterFeedback(special, inboxItem.id);
                feedbackFormName.text = Constants.instance.specialEnumToStringName[special];
            }
            feedbackForm.GetComponent<RectTransform>().GetChild(0).GetComponent<TextMeshProUGUI>().text = text;


            chosen = feedbackForm;
        }
        /*else if (inboxItem.type == "noteBig")
        {
            largeNote.SetActive(true);
            // replace sprite

            chosen = largeNote;
        }
        else if (inboxItem.type == "noteSmall")
        {
            smallNote.SetActive(true);
            // replace sprite

            chosen = smallNote;
        }*/

        BringUpLetter(chosen);

        // update mailbox sprite if needed
        mailbox.SetSprite(plotData.inbox.Count);
        mailbox.SetInteractable(false);

    }

    private void BringUpLetter(GameObject chosen)
    {

        if (chosen == null) return;

        if (chosen == letter) objectIdxMoving = 0;
        else if (chosen == feedbackForm) objectIdxMoving = 1;
        else if (chosen == largeNote) objectIdxMoving = 2;
        else if (chosen == smallNote) objectIdxMoving = 3;

        currentLetter = chosen;
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ticket, true);
        SetState(LetterState.letterMovingUp);
    }

    public void BringDownLetter()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ticket, true);
        SetState(LetterState.letterMovingDown);
    }

    private void Update()
    {
        switch (letterState)
        {
            case LetterState.letterMovingUp:
                RectTransform rect = currentLetter.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.SmoothDamp(rect.anchoredPosition.y, yPos[objectIdxMoving].y, ref currentVelocity, 0.25f));
                if (Mathf.Abs(yPos[objectIdxMoving].y - currentLetter.GetComponent<RectTransform>().anchoredPosition.y) < 1f)
                {
                    SetState(LetterState.letterOpen);
                }

                break;
            case LetterState.letterMovingDown:
                rect = currentLetter.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.SmoothDamp(rect.anchoredPosition.y, yPos[objectIdxMoving].x, ref currentVelocity, 0.25f));
                if (Mathf.Abs(yPos[objectIdxMoving].x - currentLetter.GetComponent<RectTransform>().anchoredPosition.y) < 150f)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, yPos[objectIdxMoving].x);
                    SetState(LetterState.noLetter);
                }
                break;
        }
    }

    private void ShowCrabdexNotif()
    {
        //TODO: add crabdex notif back in (once crabdex is set up)
        //crabdexNotif.SetActive(true);
        //crabdexNotif.GetComponent<Animator>().Play("Appear", -1, 0);
        StartCoroutine(WaitForCrabdexNotifAnimEnd());
    }

    private void HideCrabdexNotif()
    {
        //crabdexNotif.SetActive(false);
    }

    private IEnumerator WaitForCrabdexNotifAnimEnd()
    {
        yield return new WaitForSeconds(3);
        crabdexNotif.SetActive(false);
    }

    public void Leave()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Home);
    }

    public void ClickTrinketCeramic()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ceramic, true);
    }

    public void ClickTrinketSoft()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ticket, true);
    }
}
