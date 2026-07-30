using UnityEngine;
using UnityEngine.UI;

public class TankSceneManager : MonoBehaviour
{
    public static TankSceneManager instance { get; private set; }

    [SerializeField] private GameObject meds;
    [SerializeField] private GameObject medsShadow;
    [SerializeField] private GameObject food;

    [SerializeField] private GameObject vetButton;

    [SerializeField] private Image tankOverlay;

    private int foodsEaten = 0;
    private int goalFoodsEaten;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        vetButton.SetActive(false);

        bool medsAvailable = SaveManager.instance.GetProgression_MedsAvailable();

        if (medsAvailable)
        {
            goalFoodsEaten = 2;
            meds.SetActive(true);
            medsShadow.SetActive(true);
        }
        else
        {
            goalFoodsEaten = 1;
            meds.GetComponent<Food>().GetParticles().SetActive(false); // need this here bc Start doesn't run in Food.cs
            meds.SetActive(false);
            medsShadow.SetActive(false);
        }

        // tank overlay stuff
        Color color = tankOverlay.color;
        int currDay = SaveManager.instance.GetProgression_CurrDay();
        color.a = (currDay / 5.0f - 1.0f) * 0.25f;
        tankOverlay.color = color;
    }

    public void FoodConsumed()
    {
        foodsEaten++;

        if (foodsEaten == goalFoodsEaten)
        {
            vetButton.SetActive(true);
        }
    }
    

}
