using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CrabdexPageBuilder : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Image generalSprite;
    [SerializeField] private TextMeshProUGUI generalName;
    [SerializeField] private TextMeshProUGUI scientificName;
    [SerializeField] private TextMeshProUGUI description;

    [Header("Is Crustacean?")]
    [SerializeField] private GameObject yes;
    [SerializeField] private GameObject no;

    [Header("Variants")]
    [SerializeField] private GameObject[] variantBlocks;
    private Image[] variantSprites = new Image[6];
    private TextMeshProUGUI[] variantNames = new TextMeshProUGUI[6];

    [Header("Misc")]
    [SerializeField] private Sprite undiscovered;

	private void Awake()
	{
        for (int i = 0; i < 6; i++)
        {
            variantSprites[i] = variantBlocks[i].transform.Find("Background").transform.Find("Image").GetComponent<Image>();
            variantNames[i] = variantBlocks[i].transform.Find("Name").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }
	}

	public void Build(CrabdexEntry entry)
    {
        if (entry.generalVariantDiscovered)
        {
            generalSprite.sprite = entry.typicalSprite;
            generalName.text = entry.formalName;
            scientificName.text = entry.scientificName;
            description.text = entry.description;

            if (entry.isCrustacean)
            {
                yes.SetActive(true);
                no.SetActive(false);
            }
            else
            {
                yes.SetActive(false);
                no.SetActive(true);
            }
        }
        else
        {
            generalSprite.sprite = undiscovered;
            generalName.text = "???";
            scientificName.text = "???";
            description.text = "???";

            yes.SetActive(false);
            no.SetActive(false);
        }

        for (int i = 0; i < variantNames.Length; i++)
        {
            
            if (i >= entry.variants.Length)
            {
                Debug.Log(entry.variants.Length);
                variantBlocks[i].SetActive(false);
            }
            else
            {
                variantBlocks[i].SetActive(true);
                if (entry.variants[i].hasBeenDiscovered)
                {
                    variantSprites[i].sprite = entry.variants[i].sprite;
                    variantNames[i].text = entry.variants[i].formalVariantName;
                }
                else
                {
                    variantSprites[i].sprite = undiscovered;
                    variantNames[i].text = "???";
                }
            }
        }
    }
}
