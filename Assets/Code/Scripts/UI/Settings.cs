using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private GameObject backgroundDisplay;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject areYouSurePanel;

    [SerializeField] private RectTransform rectTransform;
    private Vector2 onPos = new Vector2(16, -81);
    private Vector2 offPos = new Vector2(16, 1198);
    private Vector2 currVelocity;
    private bool moving;
    private bool displayed;
    private bool hideAnimRunning = false;

    private void Start()
    {
        displayed = false;
        moving = false;
        rectTransform.anchoredPosition = offPos;
    }
    public void OnReturn()
    {
        //Assert.IsTrue(displayed);
        //gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "BaseArea") HideThing(backgroundDisplay);

        if (areYouSurePanel != null && areYouSurePanel.activeInHierarchy) HideThing(areYouSurePanel);

        moving = true;
    }

    private void HideThing(GameObject thing)
    {
        //print((thing.activeInHierarchy));
        if ((thing != null) && thing.activeInHierarchy)
        {
            hideAnimRunning = false;
            Animator animator = thing.GetComponent<Animator>();
            animator.enabled = true;
            StartCoroutine(WaitForAnim(thing, animator));
        }
    }

    private IEnumerator WaitForAnim(GameObject thing, Animator animator)
    {

        if (!hideAnimRunning) animator.SetTrigger("Hide");
        hideAnimRunning = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        thing.SetActive(false);
    }

    public void Show()
    {
        moving = true;
        displayed = false;
    }

    public bool IsDisplayed()
    {
        return displayed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && displayed) OnReturn();

        if (moving)
        {
            if (displayed)
            {
                rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, offPos, ref currVelocity, 0.25f, Mathf.Infinity, Time.unscaledDeltaTime);
                if (Vector2.Distance(rectTransform.anchoredPosition, offPos) < 10f)
                {
                    moving = false;
                    displayed = false;

                }
            }
            // bring the screen up
            else
            {
                rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, onPos, ref currVelocity, 0.25f, Mathf.Infinity, Time.unscaledDeltaTime);
                if (Vector2.Distance(rectTransform.anchoredPosition, onPos) < 10f)
                {
                    moving = false;
                    displayed = true;
                }

            }
        }
    }

    public void OnTutorial()
    {
        loadingScreenPanel.SetActive(true);
        loadingScreenPanel.GetComponent<LoadingScreen>().PlayLoad("Tutorial");
    }

    public void Reset()
    {
        areYouSurePanel.SetActive(true);
    }

    public void OnResetYes()
    {
        SaveManager.instance.ResetData();

        HideThing(areYouSurePanel);
    }

    public void OnResetNo()
    {
        HideThing(areYouSurePanel);
    }
}
