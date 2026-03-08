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
        return shuttlePopup.GetComponent<ShuttlePopup>().DepartTrain();
    }

    public int DepartVan()
    {
        return vanPopup.GetComponent<VanPopup>().DepartTrain();
    }

    public void ShowPopup(Type popupType, Cart.Type cartType = Cart.Type.Economy)
    {
        if (currActive)
        {
            currActive = false;
            economyPopup.SetActive(false);
            standardPopup.SetActive(false);
            shuttlePopup.SetActive(false);
            vanPopup.SetActive(false);
            return;
        }

        if (popupType == Type.train)
        {
                if (cartType == Cart.Type.Economy)
                {
                    economyPopup.SetActive(true);
                    economyPopup.GetComponent<CartPopup>().Show();
                }
                else
                {
                    standardPopup.SetActive(true);
                    standardPopup.GetComponent<CartPopup>().Show();
                        
                }
        }
        else if (popupType == Type.shuttle)
        {
            shuttlePopup.SetActive(true);
            shuttlePopup.GetComponent<ShuttlePopup>().Show();
        }
        else if (popupType == Type.van)
        {
            vanPopup.SetActive(true);
            vanPopup.GetComponent<VanPopup>().Show();
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
        currActive = false;

        economyPopup.SetActive(false);
        standardPopup.SetActive(false);
        shuttlePopup.SetActive(false);
        vanPopup.SetActive(false);
        background.SetActive(false);
    }

}
