using System.Collections;
using UnityEngine;

public class LevelPopup : MonoBehaviour
{
    // each of these needs to have a SmoothLerp component
    [Header("Kiosk")]
    [SerializeField] private GameObject kioskCluster;
    [SerializeField] private GameObject clockCluster;
    [SerializeField] private GameObject kioskPanelCluster;
    [SerializeField] private GameObject kioskPanelOverlayCluster; // covers performance bar as it moves down
    [SerializeField] private GameObject kioskPanelShadowCluster;
    [SerializeField] private GameObject performanceBarCluster;

    [Header("Everything else")]
    [SerializeField] private GameObject trainCluster;
    [SerializeField] private GameObject sandCluster;
    [SerializeField] private GameObject trainBoardCluster;


    private Vector2 kioskTargetPos = new Vector2(0, 0);
    private Vector2 clockTargetPos = new Vector2(0, 0);
    private Vector2 kioskPanelTargetPos = new Vector2(52, 0);
    private Vector2 kioskPanelOverlayTargetPos = new Vector2(52, 957.76f);
    private Vector2 kioskPanelShadowTargetPos = new Vector2(52, 9);
    private Vector2 performanceBarTargetPos = new Vector2(-88.7409f, 261.69f);

    private Vector2 trainTargetPos = new Vector2(479.8f, 9);
    private Vector2 sandTargetPos = new Vector2(0, 0); 
    private Vector2 trainBoardTargetPos = new Vector2(-48, 12.79199f);

    private float kioskSpeed = 0.5f;
    private float clockSpeed = 0.5f;
    private float kioskPanelSpeed = 0.5f;
    private float kioskPanelOverlaySpeed = 0.5f;
    private float kioskPanelShadowSpeed = 0.5f;
    private float performanceBarSpeed = 0.5f;

    private float trainSpeed = 0.5f;
    private float sandSpeed = 0.5f;
    private float trainBoardSpeed = 0.5f;

    // starting pos
    private Vector2 kioskStartPos = new Vector2(0, -1248);
    private Vector2 clockStartPos = new Vector2(0, -1129);
    private Vector2 kioskPanelStartPos = new Vector2(52, 414);
    private Vector2 kioskPanelOverlayStartPos = new Vector2(52, 1264);
    private Vector2 kioskPanelShadowStartPos = new Vector2(52, 50);
    private Vector2 performanceBarStartPos = new Vector2(-88.7409f, 572);

    private Vector2 trainStartPos = new Vector2(1572, 9);
    private Vector2 sandStartPos = new Vector2(1095, 0);
    private Vector2 trainBoardStartPos = new Vector2(-48, 235);

    private void Start()
    {
        kioskCluster.GetComponent<RectTransform>().anchoredPosition = kioskStartPos;
        clockCluster.GetComponent<RectTransform>().anchoredPosition = clockStartPos;
        kioskPanelCluster.GetComponent<RectTransform>().anchoredPosition = kioskPanelStartPos;
        kioskPanelOverlayCluster.GetComponent<RectTransform>().anchoredPosition = kioskPanelOverlayStartPos;
        kioskPanelShadowCluster.GetComponent<RectTransform>().anchoredPosition = kioskPanelShadowStartPos;
        performanceBarCluster.GetComponent<RectTransform>().anchoredPosition = performanceBarStartPos;
        kioskPanelCluster.SetActive(false);
        kioskPanelOverlayCluster.SetActive(false);
        kioskPanelShadowCluster.SetActive(false);
        performanceBarCluster.SetActive(false);

        trainCluster.GetComponent<RectTransform>().anchoredPosition = trainStartPos;
        sandCluster.GetComponent<RectTransform>().anchoredPosition = sandStartPos;
        trainBoardCluster.GetComponent<RectTransform>().anchoredPosition = trainBoardStartPos;
    }

    public void BeginPopup() // only called by level manager
    {
        StartCoroutine(Popup());
    }

    private IEnumerator Popup()
    {
        // kiosk
        kioskCluster.GetComponent<SmoothLerp>().Move(kioskTargetPos, kioskSpeed);
        clockCluster.GetComponent<SmoothLerp>().Move(clockTargetPos, clockSpeed);
        yield return new WaitForSeconds(1);

        // kiosk panel
        kioskPanelCluster.SetActive(true);
        kioskPanelOverlayCluster.SetActive(true);
        kioskPanelCluster.GetComponent<SmoothLerp>().Move(kioskPanelTargetPos, kioskPanelSpeed);
        kioskPanelOverlayCluster.GetComponent<SmoothLerp>().Move(kioskPanelOverlayTargetPos, kioskPanelOverlaySpeed);
        yield return new WaitForSeconds(0.5f);
        kioskPanelShadowCluster.SetActive(true);
        kioskPanelShadowCluster.GetComponent<SmoothLerp>().Move(kioskPanelShadowTargetPos, kioskPanelShadowSpeed);
        yield return new WaitForSeconds(0.5f);

        // performance bar
        performanceBarCluster.SetActive(true);
        performanceBarCluster.GetComponent<SmoothLerp>().Move(performanceBarTargetPos, performanceBarSpeed);

        // sand
        sandCluster.GetComponent<SmoothLerp>().Move(sandTargetPos, sandSpeed);
        trainCluster.GetComponent<SmoothLerp>().Move(trainTargetPos, trainSpeed);
        yield return new WaitForSeconds(0.75f);

        // train board
        trainBoardCluster.GetComponent<SmoothLerp>().Move(trainBoardTargetPos, trainBoardSpeed);
        yield return new WaitForSeconds(1);


        // summon crab
        LevelManagerBase.instance.Begin();
    }
}
