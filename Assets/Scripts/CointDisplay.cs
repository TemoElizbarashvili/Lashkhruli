using UnityEngine;
using TMPro;

public class CointDisplay : MonoBehaviour
{
    public TextMeshProUGUI MoneyDisplay;

    void Start()
    {
        EconomyManager.LoadEconomy();
        DisplayMoney();
    }

    void DisplayMoney()
    {
        MoneyDisplay.text = EconomyManager.GetMoneyString();
    }

    private void OnApplicationQuit()
        => EconomyManager.SaveEconomy();

    public void OnEnable()
    {
        GameEvents.OnMoneyChanged += DisplayMoney;
        DisplayMoney();
    }
  
    public void OnDisable()
    {
        GameEvents.OnMoneyChanged -= DisplayMoney;
    }
}
