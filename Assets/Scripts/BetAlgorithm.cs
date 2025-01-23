using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class BetAlgorithm : MonoBehaviour
{
    public static BetAlgorithm Instance; // Singleton instance
    private float betValue = 1.00f; // Initial bet value
    private bool isBetting = false; // Flag to check if the bet is in progress
    public bool isAutoMode1 = false; // Flag to check if the bet is in auto mode for first slot
    public bool isAutoMode2 = false; // Flag to check if the bet is in auto mode for second slot
    private TextMeshProUGUI betText;  // UI Text component to display the bet value
    private float incrementRate ;  // Increment rate of the bet value
    private float elapsedTime = 5f;
    private float betRate1, betRate2;
    private float cashValue1, cashValue2;
    private float autoRange1, autoRange2;

    [SerializeField] private Button startButton;
    [SerializeField] private Button betButton1;
    [SerializeField] private Button betButton2;
    [SerializeField] private Button autoButton1;
    [SerializeField] private Button autoButton2;
    [SerializeField] private TextMeshProUGUI outText1;
    [SerializeField] private TextMeshProUGUI outText2;
    [SerializeField] private TextMeshProUGUI finalCashText;
    [SerializeField] private TMP_InputField cashInput1;
    [SerializeField] private TMP_InputField cashInput2;
    [SerializeField] private TMP_InputField autoInput1;
    [SerializeField] private TMP_InputField autoInput2;

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
        betRate1 = 0;
        betRate2 = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (isBetting)
        {
            RandomlyStopIncrement(); // automatically stop the bet after a certain time
            if(isAutoMode1){
                if(betValue * cashValue1 > autoRange1){
                    CashOut1();
                    autoRange1 = float.PositiveInfinity;
                }
            }
            if(isAutoMode2){
                if(betValue * cashValue2 > autoRange2){
                    CashOut2();
                    autoRange2 = float.PositiveInfinity;
                }
            }
            
        }
    }

    // start the bet (connected with start button)
    public void StartBet()
    {
        if(!isBetting){
            elapsedTime = Random.Range(1f, 10f); // Initialize elapsedTime
            isBetting = true; // Set isIncrementing to true
            betButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Check Out";
            betButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Check Out";
            startButton.enabled = false;
            if(autoButton1.enabled){
                betButton1.enabled = true;
            }
            if(autoButton2.enabled){
                betButton2.enabled = true;
            }
            cashValue1 = float.Parse(cashInput1.text);
            cashValue2 = float.Parse(cashInput2.text);
            outText1.text = "";
            outText2.text = "";
            betRate1 = 0;
            betRate2 = 0;

            if(isAutoMode1){
                autoRange1 = float.Parse(autoInput1.text);
            }
            if(isAutoMode2){
                autoRange2 = float.Parse(autoInput2.text);
            }
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
            yield return new WaitForSeconds(Random.Range(0.1f, 2f)); // Wait some seconds before updating the incrementRate
        }
    }
    
    private void RandomlyStopIncrement()
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
            // Bet ended
            betText.color = Color.red;
            betValue = 1.00f;
            if(betRate1 <= 0){
                betButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Lost";
            }
            if(betRate2 <= 0){
                betButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Lost";
            }
            finalCashText.text = (betRate1*cashValue1 + betRate2*cashValue2).ToString("F2");
            startButton.enabled = true;
            betButton1.enabled = false;
            betButton2.enabled = false;

        }
    }

    public void CashOut1()
    {
        if (isBetting)
        {
            betRate1 = betValue;
            outText1.text = betRate1.ToString("F2") + "x";
            outText1.color = Color.green;
            betButton1.enabled = false;
            betButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Congrats!";
        }
    }
    public void CashOut2()
    {
        if (isBetting)
        {
            betRate2 = betValue;
            outText2.text = betRate2.ToString("F2") + "x";
            outText2.color = Color.green;
            betButton2.enabled = false;
            betButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Congrats!";
            
        }
    }

    public void AutoMode1(){
        if(isAutoMode1){
            isAutoMode1 = false;
            autoRange1 = 0f;
        }else{
            isAutoMode1 = true;
            autoRange1 = float.Parse(autoInput1.text);
        }
    }
    public void AutoMode2(){
        if(isAutoMode2){
            isAutoMode2 = false;
            autoRange2 = 0f;
        }else{
            isAutoMode2 = true;
            autoRange2 = float.Parse(autoInput2.text);
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

