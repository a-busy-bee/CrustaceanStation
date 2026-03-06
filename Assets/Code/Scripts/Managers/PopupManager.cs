using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance { get; private set; }

    [SerializeField] private GameObject standardPopup;
    [SerializeField] private GameObject economyPopup;
    [SerializeField] private GameObject shuttlePopup;
    [SerializeField] private GameObject vanPopup;
    [SerializeField] private GameObject background;

    // CURR
    private bool currActive = false;
    private (int, Cart.Type) hoveredTrain = (-1, Cart.Type.Null);

    public List<Mini> miniAssets;

    public enum MiniType
    {
        empty,
        axolotl,
        conch,
        crab,
        fishLarge,
        fishSmall,
        hermit,
        horseshoe,
        isopod,
        lobster,
        pillbugGang,
        pillbug,
        scopeCreep,
        seaGull,
        seaLemon,
        SeaLion,
        seaMonkies,
        seaSheep,
        shrimp,
        nautilus,
        family,
        orca,
        whale
    }// TODO: IF YOU ARE ADDING A MINI TYPE, UPDATE RANDOM NUM UPPER BOUND IN GenerateNewSeats INNER LOOP

    public enum Type
    {
        none,
        train,
        shuttle,
        van
    }
    private Type currRailNumber = Type.none;

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
        shuttlePopup.SetActive(false);
        vanPopup.SetActive(false);
        background.SetActive(false);
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

    public int DepartTrain(Cart.Type cartType)
    {
        if (cartType == Cart.Type.Economy)
        {
            return economyPopup.GetComponent<CartPopup>().DepartTrain();
        }
        else if (cartType == Cart.Type.Standard)
        {
            return standardPopup.GetComponent<CartPopup>().DepartTrain();
        }
        return 0;
    }

    public int DepartShuttle()
    {
        return shuttlePopup.GetComponent<ShuttlePopup>().DepartShuttle();
    }

    public int DepartVan()
    {
        return vanPopup.GetComponent<VanPopup>().DepartVan();
    }

    public void ShowPopup(Type popupType, Cart.Type cartType)
    {
        if (currActive)
        {
            currActive = false;
            economyPopup.SetActive(false);
            standardPopup.SetActive(false);
            shuttlePopup.SetActive(false);
            vanPopup.SetActive(false);
        }

        currRailNumber = popupType;

        if (popupType == Type.train)
        {
                if (cartType == Cart.Type.Economy)
                {
                    economyPopup.SetActive(true);
                    economyPopup.GetComponent<CartPopup>().Show();

                    // show arrow to the left
                    economyPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-300.4f, -290.4f);
                    economyPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);

                    // move cart to the right
                    economyPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(618, -87.56f);
                }
                else
                {
                    standardPopup.SetActive(true);
                    standardPopup.GetComponent<CartPopup>().Show();

                    // show arrow to the left
                    standardPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-300.4f, -147.9f);
                    standardPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);

                    // move cart to the right
                    standardPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(618, -87.56f);
                        
                }
        }
        else if (popupType == Type.shuttle)
        {
            shuttlePopup.SetActive(true);
            shuttlePopup.GetComponent<ShuttlePopup>().Show();

            // show arrow to the right
            shuttlePopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(302, 155.9f);
            shuttlePopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 180);

            // move cart to the left
            shuttlePopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(373, -87.56f);
        }
        else if (popupType == Type.van)
        {
            vanPopup.SetActive(true);
            vanPopup.GetComponent<VanPopup>().Show();

            // show arrow to the right
            vanPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(302, 155.9f);
            vanPopup.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 180);

            // move cart to the left
            vanPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(373, -87.56f);
        }
        

        currActive = true;
        background.SetActive(true);
    }

    public void OnClickOff()
    {
        Close();
    }

    public void Close()
    {
        hoveredTrain = (-1, Cart.Type.Null);
        currActive = false;

        economyPopup.SetActive(false);
        standardPopup.SetActive(false);
        shuttlePopup.SetActive(false);
        vanPopup.SetActive(false);
        background.SetActive(false);
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
