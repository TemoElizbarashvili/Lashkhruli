using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    public TextMeshProUGUI MoneyDisplay;
    public int MoneyAmount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadEconomy();
    }

    void Start()
    {
        DisplayMoney();
    }

    string GetMoneyString(int money)
        => $"X {money}";

    public void AddCoins(int coin)
    {
        MoneyAmount += coin;
        DisplayMoney();
        SaveEconomy();
    }

    void DisplayMoney()
        => MoneyDisplay.text = GetMoneyString(MoneyAmount);

    public void SpendMoney(int amount)
    {
        if (MoneyAmount < amount) 
            return;

        MoneyAmount -= amount;
        SaveEconomy();
    }

    public void SaveEconomy()
    {
        PlayerPrefs.SetInt("Money", MoneyAmount);
        PlayerPrefs.Save();
    }

    public void LoadEconomy()
    {
        MoneyAmount = PlayerPrefs.GetInt("Money", 0);
    }

    public int GetCoinCount()
        => MoneyAmount;

    private void OnApplicationQuit()
    {
        SaveEconomy();
    }
}
