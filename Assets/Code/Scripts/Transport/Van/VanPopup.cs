using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;

using miniPair = System.Tuple<Mini.Type, Mini.Strength>;
using Unity.VisualScripting;
public class VanPopup : TransportPopup
{
    // TODO: IF YOU ARE ADDING A MINI TYPE, UPDATE RANDOM NUM UPPER BOUND IN GenerateNewSeats INNER LOOP
    private new Cart.Type type = Cart.Type.Van;
    private new int numRows = 2;

    override public void SeatCharacter(int row, int column)
    {
        seatDictionary[0][row, column].Item1 = currMini;
        seatDictionary[0][row, column].Item2 = 3;
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
        if (ticketCartType != type) // if the crab is otherwise valid, but chose the wrong cart
        {
            Kiosk.instance.WrongTransport();
        }

        // tell kiosk to wait then summon new crab
        Kiosk.instance.SetState(Kiosk.KioskState.CrabLeaving);
        LevelManager.instance.SetTrainsClickable(false);

        StartCoroutine(WaitThenClose());
    }

}
