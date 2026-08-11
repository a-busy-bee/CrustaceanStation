using UnityEngine;

public class ShuttlePopup : TransportPopup
{
    // TODO: IF YOU ARE ADDING A MINI TYPE, UPDATE RANDOM NUM UPPER BOUND IN GenerateNewSeats INNER LOOP
	private void Awake()
	{
		type = Cart.Type.Shuttle;
	}
	override public void SeatCharacter(int row, int column, int cartID = 0)
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
        currDay = SaveManager.instance.GetProgression_CurrDay();
        if (currDay + 1 >= 6 && KioskBase.instance.GetCrabInfo().plotLevel == CrabInfo.PlotLevel.predator)
        {
            //Debug.Log("shuttled, correct, plot 1");
            SaveManager.instance.SetProgression_IncrementNonCrustiesShuttled();
            PerformanceManager.instance.Correct(true);
        }
        else if (currDay + 1 >= 11 && KioskBase.instance.GetCrabInfo().plotLevel >= CrabInfo.PlotLevel.predator)
        {
            //Debug.Log("shuttled, correct, plot 2");
            SaveManager.instance.SetProgression_IncrementNonCrustiesShuttled();
            PerformanceManager.instance.Correct(true);
        }
        else
        {
            // if ticket info was wrong
            Cart.Type ticketCartType = KioskBase.instance.GetCurrentCrabTicket();
            if (ticketCartType != type) // if the crab is otherwise valid, but chose the wrong cart
            {
                //Debug.Log("shuttled, incorrect");
                Kiosk.instance.WrongTransport();
            }
            else
            {
                //Debug.Log("shuttled, correct");
                Kiosk.instance.CorrectTransport();
            }
        }

        // tell kiosk to wait then summon new crab
        Kiosk.instance.SetState(Kiosk.KioskState.CrabLeaving);
        LevelManager.instance.SetTrainsClickable(false);

        StartCoroutine(WaitThenClose());
    }
}
