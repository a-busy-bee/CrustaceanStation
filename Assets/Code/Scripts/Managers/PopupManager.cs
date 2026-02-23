using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance { get; private set; }

    [SerializeField] private GameObject standardPopup;
    [SerializeField] private GameObject economyPopup;
    [SerializeField] private GameObject background;

    [Header("Blur")]
    [SerializeField] private GameObject blurParent;
    [SerializeField] private GameObject blurLeft;
    [SerializeField] private GameObject blurRight;

    // CURR
    private int currRailNumber = -1;
    private bool currActive = false;
    private (int, Cart.Type) hoveredTrain = (-1, Cart.Type.Null);

    public List<Mini> miniAssets;

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

        economyPopup.SetActive(false);
        standardPopup.SetActive(false);
        background.SetActive(false);
        blurParent.SetActive(false);
    }

    IEnumerator Start()
    {
        miniAssets.Clear();
        var miniHandle = Addressables.LoadAssetsAsync<Mini>("Minis", null);
        yield return miniHandle;

        miniAssets = new List<Mini>(miniHandle.Result);

        yield return null;
    }

    public List<Mini> GetMinis()
    {
        return miniAssets;
    }

    public int DepartTrain(int railNumber, Cart.Type cartType)
    {
        if (cartType == Cart.Type.Economy)
        {
            return economyPopup.GetComponent<CartPopup>().DepartTrain(railNumber);
        }
        else if (cartType == Cart.Type.Standard)
        {
            return standardPopup.GetComponent<CartPopup>().DepartTrain(railNumber);
        }
        return 0;
    }

    public void ShowPopup(int railNumber, Cart.Type cartType)
    {
        if (currActive)
        {
            currActive = false;
            economyPopup.SetActive(false);
            standardPopup.SetActive(false);
        }

        currRailNumber = railNumber;

        if (cartType == Cart.Type.Economy)
        {
            economyPopup.SetActive(true);
            economyPopup.GetComponent<CartPopup>().Show(currRailNumber);

            if (railNumber == 0)
            {
                // show arrow to the left
                economyPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-300.4f, -290.4f);
                economyPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);

                // move cart to the right
                economyPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(618, -87.56f);
            }
            else if (railNumber == 1)
            {
                // show arrow to the right
                economyPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(302, 298.1f);
                economyPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 180);

                // move cart to the left
                economyPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(373, -87.56f);
            }
        }
        else if (cartType == Cart.Type.Standard)
        {
            standardPopup.SetActive(true);
            standardPopup.GetComponent<CartPopup>().Show(currRailNumber);

            if (railNumber == 0)
            {
                // show arrow to the left
                standardPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-300.4f, -147.9f);
                standardPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);

                // move cart to the right
                standardPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(618, -87.56f);
            }
            else if (railNumber == 1)
            {
                // show arrow to the right
                standardPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(302, 155.9f);
                standardPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 180);

                // move cart to the left
                standardPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(373, -87.56f);
            }
        }

        currActive = true;
        background.SetActive(true);
    }

    public void OnClickOff()
    {
        if (hoveredTrain.Item1 == -1)
        {
            Close();
        }
        else
        {
            ShowPopup(hoveredTrain.Item1, hoveredTrain.Item2);
        }
    }

    public void Close()
    {
        hoveredTrain = (-1, Cart.Type.Null);
        currActive = false;

        economyPopup.SetActive(false);
        standardPopup.SetActive(false);
        background.SetActive(false);
        blurParent.SetActive(false);
    }

    public void SetHoveredTrain(int railNumber, Cart.Type cartType)
    {

        hoveredTrain = (railNumber, cartType);

    }

    public void ResetHoveredTrain()
    {
        hoveredTrain = (-1, Cart.Type.Null);
    }

}
