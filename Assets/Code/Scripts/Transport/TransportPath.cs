using UnityEngine;

public class TransportPath : MonoBehaviour
{
    // instantiate trains
    protected TransportController currentTransport;
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected GameObject parent;

    // send off trains when switch flipped
    [SerializeField] protected Switch switchBoard;

    protected void Start()
    {
        if (switchBoard == null) return;
        
        switchBoard.SetPath(this);
    }
    public void Summon() // called when train departs OR by level manager to init trains
    {
        GameObject transport = Instantiate(prefab, parent.transform);
        currentTransport = transport.GetComponent<TransportController>();
        
        currentTransport.gameObject.SetActive(true);
        currentTransport.InitTransport(this);
    }

    public void SetClickable(bool isClickable)
    {
        currentTransport.SetBoarding(isClickable);
    }

    public void Depart()
    {
        // depart train
        currentTransport.GetComponent<TransportController>().SetState(TransportController.TransportState.Departing);
    }
}
