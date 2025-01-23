using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetAlgorithm : MonoBehaviour
{
    public static BetAlgorithm Instance;
    private float betValue = 1.00f; // Initial bet value
    private bool isBetting = false;
    private TextMeshProUGUI betText;
    private float incrementRate ;
    private float elapsedTime = 5f;

    [SerializeField] private Button startButton;
    [SerializeField] private Button betButton1;
    [SerializeField] private Button betButton2;
    [SerializeField] private Button autoButton1;
    [SerializeField] private Button autoButton2;
    [SerializeField] private TextMeshProUGUI outText1;
    [SerializeField] private TextMeshProUGUI outText2;

    void Awake()
    {
        Instance = this;
        betText = GetComponent<TextMeshProUGUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        betText.text = betValue.ToString("F2") + "x";
        elapsedTime = Random.Range(1f, 10f);
        incrementRate = Random.Range(0.05f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (isBetting)
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
        if(isBetting){
            isBetting = false;
            betText.color = Color.green;
        }else{
            elapsedTime = Random.Range(1f, 10f); // Initialize elapsedTime
            isBetting = true; // Set isIncrementing to true
            StartCoroutine(UpdateIncrementRate());
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

     private IEnumerator UpdateIncrementRate()
    {
        while (isBetting)
        {
            incrementRate = Random.Range(0.05f, 0.5f); // Set incrementRate to a random value between 0.05 and 0.2
            yield return new WaitForSeconds(Random.Range(0.1f, 2f)); // Wait for 1 to 2 seconds
        }
    }
    
    private void StopIncrement()
    {
        elapsedTime -= Time.deltaTime;
        if (elapsedTime <= 0)
        {
            isBetting = false;
        }
        if (isBetting)
        {
            IncrementBet(incrementRate);
            betText.color = Color.white;
        }
        else
        {
            betText.color = Color.red;
        }
    }

    public float GetBetValue(){
        return betValue;
    }
    public void SetBetValue(float value){
        betValue = value;
    }
    public bool IsBetting(){
        return isBetting;
    }
}

