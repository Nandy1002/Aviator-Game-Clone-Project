using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetAlgorithm : MonoBehaviour
{
    public static BetAlgorithm Instance; // Singleton instance
    private float betValue = 1.00f; // Initial bet value
    private bool isBetting = false; // Flag to check if the bet is in progress
    private bool isAutoMode1 = false; // Flag to check if the bet is in auto mode for first slot
    private bool isAutoMode2 = false; // Flag to check if the bet is in auto mode for second slot
    private TextMeshProUGUI betText;  // UI Text component to display the bet value
    private float incrementRate ;  // Increment rate of the bet value
    private float betTimeRange = 5f; 
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
    [SerializeField] private GameObject planePrefab;

    void Awake()
    {
        Instance = this;
        betText = GetComponent<TextMeshProUGUI>();
        planePrefab.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        betText.text = betValue.ToString("F2") + "x";
        betTimeRange = Random.Range(1f, 10f);
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
        // Error handling for empty cash input
        if(cashInput1.text == "" || cashInput2.text == ""){
            Debug.Log("Please enter the value of Cash in Slot 1 and Slot 2");
            outText1.color = Color.red;
            outText1.fontSize = 10;
            outText1.text = "Please enter the value of Cash in Slot 1 and Slot 2";
            outText2.color = Color.red;
            outText2.fontSize = 10;
            outText2.text = "Please enter the value of Cash in Slot 1 and Slot 2";
            return;
        }else{
            outText1.text = "";
            outText1.fontSize = 32;
            outText1.color = Color.white;
            outText2.text = "";
            outText2.fontSize = 32;
            outText2.color = Color.white;
            cashValue1 = float.Parse(cashInput1.text);
            cashValue2 = float.Parse(cashInput2.text);
        }

        // start the bet if not already started
        if(!isBetting){
            betTimeRange = Random.Range(1f, 20f); // Initialize betting time range
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

            planePrefab.SetActive(true);

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
        betTimeRange -= Time.deltaTime;
        if (betTimeRange <= 0)
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
            planePrefab.SetActive(false);
        }
    }

    // Cash out for slot 1 (connected with cash out button for slot 1)
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

    // Cash out for slot 2 (connected with cash out button for slot 2)
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

    // Auto mode for the slot1 (connected with auto button for slot 1)
    public void AutoMode1(){
        // Error handling for empty auto input
        if(autoInput1.text == "" || cashInput1.text == "" || float.Parse(autoInput1.text) < float.Parse(cashInput1.text)){
            Debug.Log("Please enter the proper value Auto Value in Slot 1");
            outText1.color = Color.red;
            outText1.fontSize = 10;
            outText1.text = "Please enter proper the value Auto Value in Slot 1";
            autoButton1.image.color = Color.white;
            autoButton1.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            autoButton1.GetComponent<AutoActive>().onClick();
            isAutoMode1 = false;
            return;

        }else{
            outText1.text = "";
            outText1.fontSize = 32;
            outText1.color = Color.white;
        }
        
        // Toggle the auto mode
        if(isAutoMode1){
            isAutoMode1 = false;
            autoRange1 = 0f;
        }else{
            isAutoMode1 = true;
            autoRange1 = float.Parse(autoInput1.text);
        }
    }

    // Auto mode for the slot2 (connected with auto button for slot 2)
    public void AutoMode2(){
        // Error handling for empty auto input
        if(autoInput2.text == "" || cashInput2.text == "" || float.Parse(autoInput2.text) < float.Parse(cashInput2.text)){
            Debug.Log("Please enter the proper value Auto Value in Slot 2");
            outText2.color = Color.red;
            outText2.fontSize = 10;
            outText2.text = "Please enter the proper value Auto Value in Slot 2";
            autoButton2.image.color = Color.white;
            autoButton2.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            autoButton2.GetComponent<AutoActive>().onClick();
            return;
        }else{
            outText2.text = "";
            outText2.fontSize = 32;
            outText2.color = Color.white;
        }

        // Toggle the auto mode
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

