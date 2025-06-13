using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public TextMeshProUGUI MoneyDisplay;
    public int MoneyAmount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayMoney();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string GetMoneyString(int money)
        => $"X {money}";

    public void AddCoins(int coin)
    {
        MoneyAmount += coin;
        DisplayMoney();
    }

    void DisplayMoney()
        => MoneyDisplay.text = GetMoneyString(MoneyAmount);
}
