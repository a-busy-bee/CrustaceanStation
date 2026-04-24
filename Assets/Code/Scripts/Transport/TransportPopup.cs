using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;

using miniPair = System.Tuple<Mini.Type, Mini.Strength>;
using Unity.VisualScripting;
public class TransportPopup : MonoBehaviour
{
    // TODO: IF YOU ARE ADDING A MINI TYPE, UPDATE RANDOM NUM UPPER BOUND IN GenerateNewSeats INNER LOOP
     
     //TODO: use inheritance for van, shuttle, and cart
    protected Mini currMini;
    [SerializeField] protected Cart.Type type;
    protected int currID;
    protected int numRows = 3;

    protected Dictionary<int, (Mini, int)[,]> seatDictionary = new Dictionary<int, (Mini, int)[,]>(); // <rail number, array of seats>

    protected CartSeat[,] seatObjects;   // the actual grid I use for the seats
    [SerializeField] protected List<CartSeat> seatsAll;       // so I can drag all the seats into the inspector

    [SerializeField] protected Mini defaultEmpty;

    protected bool initialized = false;

    // HOW POORLY THE INTERACTIONS WENT
    protected int badness = 0;
    protected int currHowBad = 0;
    protected int[,] predXprey = { { 2, 1, 0 }, { 3, 2, 1 }, { 4, 3, 2 } };
    protected int[,] sameType = { { 0, 2, 4 }, { 2, 0, 2 }, { 4, 2, 0 } };
    protected Dictionary<miniPair, int> indexOfPredXPrey = new Dictionary<miniPair, int>()
    {
        {new miniPair(Mini.Type.predator, Mini.Strength.strong),    2},
        {new miniPair(Mini.Type.predator, Mini.Strength.average),   1},
        {new miniPair(Mini.Type.predator, Mini.Strength.weak),      0},
        {new miniPair(Mini.Type.prey, Mini.Strength.strong),        2},
        {new miniPair(Mini.Type.prey, Mini.Strength.average),       1},
        {new miniPair(Mini.Type.prey, Mini.Strength.weak),          0}
    };

    protected Dictionary<Mini.Strength, int> indexOfSameType = new Dictionary<Mini.Strength, int>()
    {
        {Mini.Strength.strong,      2},
        {Mini.Strength.average,     1},
        {Mini.Strength.weak,        0},
    };

    protected Dictionary<int, int> seatPairs = new Dictionary<int, int>() // ie A1 and A2 go together, A3 and A4 go together
    {
        { 0, 1 },
        { 1, 0 },
        { 2, 3 },
        { 3, 2 }
    };

    protected List<Mini> miniAssets;

    protected virtual void Start()
    {
        if (initialized) return;

        //seatObjects = new CartSeat[numRows, 4];
        InitPopup();
    }

    protected void InitPopup()
    {
        seatObjects = new CartSeat[numRows, 4];
        miniAssets = PopupManager.instance.GetMinis();
        // convert long seatsAll list to neat little grid for seatObjects
        for (int i = 0; i < seatsAll.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;
            seatObjects[row, col] = seatsAll[i];
        }

        // INIT SEAT DICTIONARY
        seatDictionary[0] = new (Mini, int)[numRows, 4];
        //(Mini, int)[,] minis = seatDictionary[i];

        // INIT SEATS
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                seatDictionary[0][row, col] = (defaultEmpty, 0);
                seatObjects[row, col].InitSeat(this, row, col);
            }
        }

        GenerateNewSeats();
        
        initialized = true;
    }

    public virtual void Show()
    {
        if (!initialized)
        {
            InitPopup();
        }

        // get crabinfo from kiosk
        CrabInfo info = Kiosk.instance.GetCrabInfo();
        currMini = info.mini;

        (Mini, int)[,] minis = seatDictionary[0];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (minis[row, col].Item1.isMultiple)
                {
                    seatObjects[row, col].Populate(minis[row, col].Item1);
                    seatObjects[row, col].SetCharacter(currMini);
                    seatObjects[row, col].HasSelected(false);

                    if (col == 0 || col == 2)
                    {
                        seatObjects[row, col + 1].PopulateForMultiple(minis[row, col].Item1.multSprite);
                        col++; // skip next one
                    }
                    else
                    {
                        seatObjects[row, col - 1].PopulateForMultiple(minis[row, col].Item1.multSprite);
                    }

                }
                else
                {
                    seatObjects[row, col].Populate(minis[row, col].Item1);
                    seatObjects[row, col].SetCharacter(currMini);
                    seatObjects[row, col].HasSelected(false);
                }
                
            }
        }
    }

    public virtual void SeatCharacter(int row, int column)
    {
        seatDictionary[currID][row, column].Item1 = currMini;
        seatDictionary[currID][row, column].Item2 = 3;
        for (int rowIdx = 0; rowIdx < numRows; rowIdx++)
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
        if (ticketCartType != type) // if the crab chose the wrong cart
        {
            Kiosk.instance.DowngradedCart();
        }

        // tell kiosk to wait then summon new crab
        Kiosk.instance.SetState(Kiosk.KioskState.CrabLeaving);
        LevelManager.instance.SetTrainsClickable(false);

        StartCoroutine(WaitThenClose());
    }
    public virtual int DepartTrain()
    {
        if (!initialized) InitPopup();

        // figure out how many coins to give the player
        int coins = 0;

        // figure out if there are any bonuses earned
        // if so, award them

        // clear the dictionary idx for this train
        (Mini, int)[,] minis = seatDictionary[0];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (minis[row, col].Item1 != defaultEmpty)
                {
                    coins += minis[row, col].Item2;
                    minis[row, col].Item1 = defaultEmpty;
                    minis[row, col].Item2 = 0;
                }
            }
        }

        coins -= badness;

        GenerateNewSeats();

        return coins;
    }

    protected void GenerateNewSeats()
    {
        (Mini, int)[,] minis = seatDictionary[0];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                // random chance to place char
                if (Random.Range(0.0f, 1.0f) <= 0.35f)
                {
                    int chance = Random.Range(1, 23);
                    minis[row, col].Item1 = miniAssets[chance];
                    minis[row, col].Item2 = 0;
                }

            }
        }
    }

    protected IEnumerator WaitThenClose()
    {
        yield return new WaitForSeconds(1);
        // close popup
        PopupManager.instance.Close();
    }

    public void CheckRelationship(int row, int col)
    {
        Mini otherMini = seatDictionary[currID][row, seatPairs[col]].Item1;
        CartSeat currSeat = seatObjects[row, col];
        CartSeat otherSeat = seatObjects[row, seatPairs[col]];

        int howBad = 0;

        if (otherMini.miniType == PopupManager.MiniType.empty)
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

    protected void PlayAnims(int howBad, Mini curr, Mini other, CartSeat currSeat, CartSeat otherSeat)
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
        seatDictionary[currID][row, col].Item1 = currMini;
        for (int rowIdx = 0; rowIdx < numRows; rowIdx++)
        {
            for (int colIdx = 0; colIdx < 4; colIdx++)
            {
                seatObjects[rowIdx, colIdx].HasSelected(true);
            }
        }
    }
}
