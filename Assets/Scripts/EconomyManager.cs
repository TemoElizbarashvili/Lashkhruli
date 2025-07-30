using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public static class EconomyManager
{
    public static int MoneyAmount = 0;

    public static string GetMoneyString()
        => $"X {MoneyAmount}";

    public static void LoadEconomy()
    {
        MoneyAmount = PlayerPrefs.GetInt("Money", 0);
        GameEvents.RaiseMoneyChanged();
    }

    public static void SaveEconomy()
    {
        PlayerPrefs.SetInt("Money", EconomyManager.MoneyAmount);
        PlayerPrefs.Save();
    }

    public static void SpendMoney(int amount)
    {
        if (MoneyAmount < amount)
            return;

        MoneyAmount -= amount;
        Debug.Log($"Spent {amount} coins. Remaining: {MoneyAmount}");
        GameEvents.RaiseMoneyChanged();
        SaveEconomy();
    }

    public static void AddCoins(int coin)
    {
        MoneyAmount += coin;
        GameEvents.RaiseMoneyChanged();
        SaveEconomy();
    }

}
