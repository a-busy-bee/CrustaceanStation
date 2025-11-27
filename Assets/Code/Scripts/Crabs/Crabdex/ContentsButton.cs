using UnityEngine;
using UnityEngine.UI;

public class ContentsButton : MonoBehaviour
{
    private int id;

    public void SetID(int newId)
    {
        id = newId;
        GetComponent<Button>().onClick.AddListener(() => Crabdex.instance.OnCrabEntryPress(id));
    }


}
