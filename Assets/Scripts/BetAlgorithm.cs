using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetAlgorithm : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TextMeshProUGUI betText;
    private float betValue = 1.00f; // Initial bet value
    private bool isIncrementing = false;
    [SerializeField] private float incrementRate = 0.1f;
    private float elapsedTime = 5f;

    [SerializeField] private Button betButton1;
    [SerializeField] private Button betButton2;

    void Awake()
    {
        betText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        betText.text = betValue.ToString("F2") + "x";
        elapsedTime = Random.Range(1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isIncrementing)
        {
            StopIncrement();
            betButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Check Out";
            betButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Check Out";
        }else{
            betButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Bet";
            betButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Bet"; 
        }
    }
    public void StartBet()
    {
        betValue = 1.00f;
        if(isIncrementing){
            isIncrementing = false;
            betText.color = Color.green;
        }else{
            elapsedTime = Random.Range(1f, 10f); // Initialize elapsedTime
            isIncrementing = true; // Set isIncrementing to true
        }
    }


    private void IncrementBet(float increment)
    {
        betValue += increment * Time.deltaTime; // Increment the bet value over time
        if (betText != null)
        {
            betText.text = betValue.ToString("F2") + "x"; // Update the UI Text component
        }
    }

    private void StopIncrement()
    {
        elapsedTime -= Time.deltaTime;
        if (elapsedTime <= 0)
        {
            isIncrementing = false;
        }
        if (isIncrementing)
        {
            IncrementBet(incrementRate);
            betText.color = Color.white;
        }
        else
        {
            betText.color = Color.red;
        }
    }
}
