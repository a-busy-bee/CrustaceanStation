using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class IsoMinigameManager : MonoBehaviour
{
    [SerializeField] private Color[] isoColors;
    [SerializeField] private GameObject[] isos;
    [SerializeField] private GameObject isoParent;


    private void Start()
    {
        for (int i = 0; i < isos.Length; i++)
        {
            isos[i].GetComponent<IsoController>().SetColor(isoColors[i]);
        }
    }


}
