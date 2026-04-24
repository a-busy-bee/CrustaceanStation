using TMPro;
using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private Rigidbody2D rb;
    //[SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Clicked()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(200, 400), ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-150f, 150f));

        StartCoroutine(Falling());
    }


    private IEnumerator Falling()
    {
        yield return new WaitForSeconds(1.5f);
        //PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
        //coinText.text = PlayerPrefs.GetInt("coins").ToString();

        gameObject.SetActive(false);
    }
}
