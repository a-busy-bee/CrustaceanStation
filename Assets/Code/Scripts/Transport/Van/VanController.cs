using UnityEngine;

public class VanController : TransportController
{
    private void Awake()
    {
        startingPosArrive = new Vector3(0, 882, 0); // where the train is before it moves into the station
        startingPosDepart = new Vector3(0, 175, 0); // also endPosArrive
        endPosDepart = new Vector3(0, -1084, 0); // where the train goes to be completely offscreen

    }
    override public void SetState(TransportState newState)
    {
        TransportState prevState = transportState;
        transportState = newState;

        switch (transportState)
        {
            case TransportState.Setup:
                {
                    // do nothing, logic already in InitTrain
                }
                break;

            case TransportState.NotArrived:
                {
                    // set starting position
                    transportTransform.anchoredPosition = startingPosArrive;
                }
                break;

            case TransportState.Arriving:
                {
                    // nothing to do here, all logic in Update()
                }
                break;

            case TransportState.NotBoarding:
                {
                    SetThisClickable(false);
                }
                break;

            case TransportState.Boarding:
                {
                    SetThisClickable(true);
                }
                break;

            case TransportState.Departing:
                {
                    // give player coins
                    coins = 0;

                    foreach (TransportSelection ts in transportSelections)
                    {
                        ts.SetThisClickable(false);
                        coins += ts.Depart();
                    }
                    Kiosk.instance.GivePlayerCoins(coins);
                }
                break;

            case TransportState.Departed:
                {
                    //LevelManager.instance.RemoveCurrentTrain(this);
                    rail.Summon();

                    Destroy(gameObject);
                }
                break;
        }
    }

    override public void InitTransport(TransportPath newRail)
    {
        rail = newRail;
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);

        transportTransform.anchoredPosition = startingPosArrive;
        SetState(TransportState.Arriving);
    }
}
 