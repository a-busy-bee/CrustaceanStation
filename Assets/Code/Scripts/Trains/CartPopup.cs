using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using miniPair = System.Tuple<Mini.Type, Mini.Strength>;
using Unity.VisualScripting;

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
        shrimp,
        nautilus,
        family
    }
    private Mini currMini;
    private Cart.Type type;
    private int currID;

    private Dictionary<int, Mini[,]> seatDictionary = new Dictionary<int, Mini[,]>(); // <rail number, array of seats>

    private CartSeat[,] seatObjects = new CartSeat[3, 4];   // the actual grid I use for the seats
    [SerializeField] private List<CartSeat> seatsAll;       // so I can drag all the seats into the inspector

    [SerializeField] private Mini defaultEmpty;

    private bool initialized = false;

    // HOW POORLY THE INTERACTIONS WENT
    private int badness = 0;
    private int currHowBad = 0;
    private int[,] predXprey = { { 2, 1, 0 }, { 3, 2, 1 }, { 4, 3, 2 } };
    private int[,] sameType = { { 0, 2, 4 }, { 2, 0, 2 }, { 4, 2, 0 } };
    private Dictionary<miniPair, int> indexOfPredXPrey = new Dictionary<miniPair, int>()
    {
        {new miniPair(Mini.Type.predator, Mini.Strength.strong),    2},
        {new miniPair(Mini.Type.predator, Mini.Strength.average),   1},
        {new miniPair(Mini.Type.predator, Mini.Strength.weak),      0},
        {new miniPair(Mini.Type.prey, Mini.Strength.strong),        2},
        {new miniPair(Mini.Type.prey, Mini.Strength.average),       1},
        {new miniPair(Mini.Type.prey, Mini.Strength.weak),          0}
    };

    private Dictionary<Mini.Strength, int> indexOfSameType = new Dictionary<Mini.Strength, int>()
    {
        {Mini.Strength.strong,      2},
        {Mini.Strength.average,     1},
        {Mini.Strength.weak,        0},
    };

    private Dictionary<int, int> seatPairs = new Dictionary<int, int>() // ie A1 and A2 go together, A3 and A4 go together
    {
        { 0, 1 },
        { 1, 0 },
        { 2, 3 },
        { 3, 2 }
    };

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
                if (minis[row, col].isMultiple)
                {
                    seatObjects[row, col].Populate(minis[row, col]);
                    seatObjects[row, col].SetCharacter(currMini);
                    seatObjects[row, col].HasSelected(false);

                    if (col == 0 || col == 2)
                    {
                        seatObjects[row, col + 1].PopulateForMultiple(minis[row, col].multSprite);
                        col++; // skip next one
                    }
                    else
                    {
                        seatObjects[row, col - 1].PopulateForMultiple(minis[row, col].multSprite);
                    }
                    
                }
                else
                {
                    seatObjects[row, col].Populate(minis[row, col]);
                    seatObjects[row, col].SetCharacter(currMini);
                    seatObjects[row, col].HasSelected(false);
                }
                
            }
        }
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

        // check pred/prey relationships
        badness += currHowBad;

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
                    coins += 2;
                    minis[row, col] = defaultEmpty;
                }
            }
        }

        coins -= badness;

        return coins;
    }

    private IEnumerator WaitThenClose()
    {
        yield return new WaitForSeconds(1);
        // close popup
        PopupManager.instance.Close();
    }

    public void CheckRelationship(int row, int col)
    {
        Mini otherMini = seatDictionary[currID][row, seatPairs[col]];
        CartSeat currSeat = seatObjects[row, col];
        CartSeat otherSeat = seatObjects[row, seatPairs[col]];

        int howBad = 0;

        if (otherMini.miniType == MiniType.empty)
        {
            return;
        }
        else if (otherMini.type == Mini.Type.toxic && currMini.type == Mini.Type.toxic)
        {
            howBad = 0;
        }
        else if (otherMini.type == Mini.Type.toxic || currMini.type == Mini.Type.toxic)
        {
            howBad = 5;
        }
        else
        {
            // figure out how bad that interaction was
            if (otherMini.type == currMini.type)
            {
                howBad = sameType[indexOfSameType[currMini.strength],
                                  indexOfSameType[otherMini.strength]];
            }
            else
            {
                if (currMini.type == Mini.Type.predator)
                {
                    howBad = predXprey[indexOfPredXPrey[new miniPair(currMini.type, currMini.strength)],
                                       indexOfPredXPrey[new miniPair(otherMini.type, otherMini.strength)]];
                }
                else
                {
                    howBad = predXprey[indexOfPredXPrey[new miniPair(otherMini.type, otherMini.strength)],
                                       indexOfPredXPrey[new miniPair(currMini.type, currMini.strength)]];
                }

            }
        }

        PlayAnims(howBad, currMini, otherMini, currSeat, otherSeat);
    }

    private void PlayAnims(int howBad, Mini curr, Mini other, CartSeat currSeat, CartSeat otherSeat)
    {
        //Debug.Log(curr.type + ", " + curr.strength + "  x  " + other.type + ", " + other.strength + "  =  " + howBad);
        if (howBad == 5) // TOXIC
        {
            if (curr.type == Mini.Type.toxic)
            {
                // curr play toxic
                currSeat.PlayAnim(CartSeat.ReactionType.toxicSelf);
                // other play death
                otherSeat.PlayAnim(CartSeat.ReactionType.toxicOther);
            }
            else
            {
                // other play toxic
                otherSeat.PlayAnim(CartSeat.ReactionType.toxicSelf);
                // curr play death
                currSeat.PlayAnim(CartSeat.ReactionType.toxicOther);
            }
            currHowBad = 3;
        }
        else if (howBad == 0 || howBad == 1)
        {
            // prey x prey or toxic x toxic
            if (curr.type == other.type && (curr.type == Mini.Type.prey || curr.type == Mini.Type.toxic))
            {
                // curr play happy
                currSeat.PlayAnim(CartSeat.ReactionType.happy);
                // other play happy
                otherSeat.PlayAnim(CartSeat.ReactionType.happy);
            }
            // pred x pred
            else if (curr.type == other.type && curr.type == Mini.Type.predator)
            {
                // curr play comrade
                currSeat.PlayAnim(CartSeat.ReactionType.comrade);
                // other play comrade
                otherSeat.PlayAnim(CartSeat.ReactionType.comrade);
            }
            // pred x prey
            else
            {
                // curr play neutral
                currSeat.PlayAnim(CartSeat.ReactionType.neutral);
                // pred play neutral
                otherSeat.PlayAnim(CartSeat.ReactionType.neutral);
            }

            currHowBad = 0;
        }
        else if (howBad == 2 || howBad == 3)
        {
            // prey x prey
            if (curr.type == other.type && curr.type == Mini.Type.prey)
            {
                // curr play happy
                currSeat.PlayAnim(CartSeat.ReactionType.happy);
                // other play happy
                otherSeat.PlayAnim(CartSeat.ReactionType.happy);
            }
            // pred x pred
            else if (curr.type == other.type && curr.type == Mini.Type.predator)
            {
                // curr play neutral
                currSeat.PlayAnim(CartSeat.ReactionType.neutral);
                // other play neutral
                otherSeat.PlayAnim(CartSeat.ReactionType.neutral);
            }
            // pred x prey
            else
            {
                if (curr.type == Mini.Type.predator)
                {
                    // curr play yummy
                    currSeat.PlayAnim(CartSeat.ReactionType.yummy);
                    // other play fear
                    otherSeat.PlayAnim(CartSeat.ReactionType.fear);
                }
                else
                {
                    // curr play fear
                    currSeat.PlayAnim(CartSeat.ReactionType.fear);
                    // other play yummy
                    otherSeat.PlayAnim(CartSeat.ReactionType.yummy);
                }
            }

            currHowBad = 1;
        }
        else if (howBad == 4)
        {
            // prey x prey
            if (curr.type == other.type && curr.type == Mini.Type.prey)
            {
                // curr play neutral
                currSeat.PlayAnim(CartSeat.ReactionType.neutral);
                // other play neutral
                otherSeat.PlayAnim(CartSeat.ReactionType.neutral);
            }
            // pred x pred
            else if (curr.type == other.type && curr.type == Mini.Type.predator)
            {
                if (curr.strength == Mini.Strength.strong)
                {
                    // curr play swords
                    currSeat.PlayAnim(CartSeat.ReactionType.swords);
                    // other play fear
                    otherSeat.PlayAnim(CartSeat.ReactionType.fear);
                }
                else
                {
                    // curr play fear
                    currSeat.PlayAnim(CartSeat.ReactionType.fear);
                    // other play swords
                    otherSeat.PlayAnim(CartSeat.ReactionType.swords);
                }
            }
            // pred x prey
            else
            {
                if (curr.type == Mini.Type.predator)
                {
                    // curr play yummy
                    currSeat.PlayAnim(CartSeat.ReactionType.yummy);
                    // other play fear
                    otherSeat.PlayAnim(CartSeat.ReactionType.fear);
                }
                else
                {
                    // curr play fear
                    currSeat.PlayAnim(CartSeat.ReactionType.fear);
                    // other play yummy
                    otherSeat.PlayAnim(CartSeat.ReactionType.yummy);
                }
            }

            currHowBad = 2;
        }
    }

    public void TurnOffAnims(int row, int col)
    {
        seatObjects[row, seatPairs[col]].StopAnim();
    }

    public bool CheckIfBothSeatsAreOpen(int row, int col)
    {
        return !seatObjects[row, seatPairs[col]].IsTaken();
    }

    public void ShowGhostMultiple(Sprite mult, int row, int col)
    {
        seatObjects[row, seatPairs[col]].ShowGhostSpriteForMultiple(mult);
    }

    public void HideGhostMultiple(int row, int col)
    {
        seatObjects[row, seatPairs[col]].HideGhostSpriteForMultiple();
    }

    public void PlayGhostAnim(int row, int col)
    {
        seatObjects[row, seatPairs[col]].PlayAnim(CartSeat.ReactionType.happy);
    }

    public void SeatMultiple(Sprite mult, int row, int col)
    {
        seatObjects[row, seatPairs[col]].SetSpriteForMultiple(mult);
        seatDictionary[currID][row, col] = currMini;
        for (int rowIdx = 0; rowIdx < 3; rowIdx++)
        {
            for (int colIdx = 0; colIdx < 4; colIdx++)
            {
                seatObjects[rowIdx, colIdx].HasSelected(true);
            }
        }
    }
}
