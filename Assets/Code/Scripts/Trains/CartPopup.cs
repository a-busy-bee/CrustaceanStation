using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CartPopup : MonoBehaviour
{
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
        shrimp
    }
    private Mini currMini;
    private Cart.Type type;
    private int currID;

    private Dictionary<int, Mini[,]> seatDictionary = new Dictionary<int, Mini[,]>(); // <rail number, array of seats>
    private CartSeat[,] seatObjects = new CartSeat[3, 4];   // the actual grid I use for the seats
    [SerializeField] private List<CartSeat> seatsAll;       // so I can drag all the seats into the inspector

    [SerializeField] private Mini defaultEmpty;


    private bool initialized = false;
    private void Start()
    {
        InitPopup();
    }

    private void InitPopup()
    {
        // convert long seatsAll list to neat little grid for seatObjects
        for (int i = 0; i < seatsAll.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;
            seatObjects[row, col] = seatsAll[i];
        }

        // INIT SEAT DICTIONARY
        int rails = LevelManager.instance.GetNumberOfRails();

        for (int i = 0; i < rails; i++)
        {
            seatDictionary[i] = new Mini[3, 4];

            // INIT SEATS
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    seatDictionary[i][row, col] = defaultEmpty;
                    seatObjects[row, col].InitSeat(this, row, col);
                }
            }
        }

        initialized = true;
    }

    public void Show(int railNumber)
    {
        if (!initialized) InitPopup();

        // get crabinfo from kiosk
        CrabInfo info = Kiosk.instance.GetCrabInfo();
        currMini = info.mini;
        currID = railNumber;

        Mini[,] minis = seatDictionary[railNumber];

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                seatObjects[row, col].Populate(minis[row, col]);
                seatObjects[row, col].SetCharacter(currMini);
                seatObjects[row, col].HasSelected(false);
            }
        }
    }

    public void PlayAnim(int row, int column)
    {
        // play anim of the mini beside the placed one, if possible
        Debug.Log("play anim for " + row + " and " + column);
    }

    public void SeatCharacter(int row, int column)
    {
        seatDictionary[currID][row, column] = currMini;
        for (int rowIdx = 0; rowIdx < 3; rowIdx++)
        {
            for (int colIdx = 0; colIdx < 4; colIdx++)
            {
                seatObjects[rowIdx, colIdx].HasSelected(true);
            }
        }

        PlayAnim(row, column);

        // if ticket info was wrong
        Cart.Type ticketCartType = Kiosk.instance.GetCurrentCrabTicket();
        if (ticketCartType != type && Kiosk.instance.IsCrabValid()) // if the crab is otherwise valid, but chose the wrong cart
        {
            if (ticketCartType == Cart.Type.Economy)
            {
                // chosen cart was an upgrade, no rating complaints
                Kiosk.instance.UpgradedCart();
            }
            else
            {
                Kiosk.instance.DowngradedCart(); // chosen cart was a downgrade, rating goes down
            }
        }

        // tell kiosk to wait then summon new crab
        Kiosk.instance.SetState(Kiosk.KioskState.CrabLeaving);
        LevelManager.instance.SetTrainsClickable(false);

        StartCoroutine(WaitThenClose());
    }
    public int DepartTrain(int railNumber)
    {
        if (!initialized) InitPopup();

        // figure out how many coins to give the player
        int coins = 0;

        // figure out if there are any bonuses earned
        // if so, award them

        // clear the dictionary idx for this train
        Mini[,] minis = seatDictionary[railNumber];

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (minis[row, col] != defaultEmpty)
                {
                    // TODO: here's where we add the emotion-specific bonuses/debuffs
                    coins += 3;
                    minis[row, col] = defaultEmpty;
                }
            }
        }

        return coins;
    }

    private IEnumerator WaitThenClose()
    {
        yield return new WaitForSeconds(1);
        // close popup
        PopupManager.instance.Close();
    }
}
