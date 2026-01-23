using UnityEngine;

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
        }
        else if (cartType == Cart.Type.Standard)
        {
            Debug.Log("standard");
            standardPopup.SetActive(true);
            standardPopup.GetComponent<CartPopup>().Show(currRailNumber);
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
