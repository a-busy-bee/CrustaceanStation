using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class IsoMinigameManager : MonoBehaviour
{
    [SerializeField] private Color[] isoColors;
    [SerializeField] private GameObject isoPrefab;
    [SerializeField] private GameObject isoParent;
    private List<GameObject> isopods = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SummonOneByOne());
    }

    private IEnumerator SummonOneByOne()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject iso = Instantiate(isoPrefab, isoParent.GetComponent<RectTransform>());
            iso.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            iso.GetComponent<IsoController>().SetColor(isoColors[i]);
            isopods.Add(iso);
            yield return new WaitForSeconds(1);
        }
    }
}
