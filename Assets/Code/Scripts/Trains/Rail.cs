using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Rail : MonoBehaviour
{
    // instantiate trains
    private TrainController currentTrain;
    [SerializeField] private GameObject trainPrefab;
    [SerializeField] private GameObject trainParent;
    public enum RailDirection
    {
        North, 
        South,
        East,
        West

        // can add more directions later (ie NW, SW, NE, NW, etc)
    }
    [SerializeField] private RailDirection railDirection;
    private int railNumber;
    [SerializeField] private GameObject standardCartPopup;
    [SerializeField] private GameObject economyCartPopup;


    // enable/disable direction arrow?
    [SerializeField] private GameObject directionArrow;

    // send off trains when switch flipped
    [SerializeField] private Switch trainSwitch;

    private void Start()
    {
        trainSwitch.SetRail(this);
    }
    public void SummonTrain() // called when train departs OR by level manager to init trains
    {
        GameObject train = Instantiate(trainPrefab, trainParent.transform);
        currentTrain = train.GetComponent<TrainController>();
        
        currentTrain.gameObject.SetActive(true);
        currentTrain.InitTrain(railDirection, this, standardCartPopup, economyCartPopup, railNumber);
    }

    public void SetTrainClickable(bool isClickable)
    {
        currentTrain.SetBoarding(isClickable);
    }

    public void SetRailNumber(int newNumber)
    {
        railNumber = newNumber;
    } 

    public bool CheckTrainValidity(string id)
    {
        return false;
    }

    public void DepartTrain()
    {
        // depart train
        currentTrain.GetComponent<TrainController>().SetState(TrainController.TrainState.Departing);
    }

    public void FlashFullAlert()
    {

    }

    /*private IEnumerator Alert()
    {
        if (isAlerting)
        {
            for (int i = 0; i < 5; i++)
            {
                alertObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                alertObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }*/
}
