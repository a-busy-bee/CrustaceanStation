using UnityEngine;
using UnityEngine.UI;

public class TankSceneManager : MonoBehaviour
{
    public static TankSceneManager instance { get; private set; }

    [SerializeField] private GameObject meds;
    [SerializeField] private GameObject medsShadow;
    [SerializeField] private GameObject food;

    [SerializeField] private GameObject vetButton;

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
