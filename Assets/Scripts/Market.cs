using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{

    public static Market Instance { get; private set; }

    #region Public Properties

    public int DamageAddition { get; set; }
    public int HealthAddition { get; set; }
    public float SpeedNJumpMultiplayer { get; set; }

    public TMP_Text DamagePriceText;
    public TMP_Text HealthPriceText;
    public TMP_Text SpeedNJumpPriceText;

    public TMP_Text DamageLevelText;
    public TMP_Text HealthLevelText;
    public TMP_Text SpeedNJumpLevelText;
    public TMP_Text GoldText;

    public Button DamageUpgradeButton;
    public Button HealthUpgradeButton;
    public Button SpeedNJumpUpgradeButton;

    public AudioSource PurchaseSound;

    #endregion

    #region Private Properties

    private readonly int[] _prices = { 100, 200, 300, 400, 500 };
    private readonly int[] _damageAdditions = { 2, 8, 10, 15, 20 };
    private readonly int[] _healthAdditions = { 10, 20, 30, 40, 50 };
    private readonly float[] _speedNJumpMultipliers = { 1.0f, 1.1f, 1.2f, 1.3f, 1.4f };
    private int currentHealthIndex = 0;
    private int currentDamageIndex = 0;
    private int currentSpeedNJumpIndex = 0;

    #endregion


    private void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
        DamageAddition = 2;
        HealthAddition = 10;
        SpeedNJumpMultiplayer = 1.0f;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateMarketUI();
    }

    //TODO: update HUD Coin Count ! <3 
    public void BuyDamageUpgrade()
    {
        if (currentDamageIndex >= _prices.Length || EconomyManager.Instance.MoneyAmount < _prices[currentDamageIndex])
            return;

        Helpers.PlayAudioSafely(PurchaseSound);
        EconomyManager.Instance.SpendMoney(_prices[currentDamageIndex]);
        DamageAddition = _damageAdditions[currentDamageIndex];
        currentDamageIndex++;
        UpdateMarketUI();
    }

    public void BuyHealthUpgrade()
    {
        if (currentHealthIndex >= _prices.Length || EconomyManager.Instance.MoneyAmount < _prices[currentHealthIndex])
            return;

        Helpers.PlayAudioSafely(PurchaseSound);
        EconomyManager.Instance.SpendMoney(_prices[currentHealthIndex]);
        HealthAddition = _healthAdditions[currentHealthIndex];
        currentHealthIndex++;
        UpdateMarketUI();

    }

    public void BuySpeedNJumpUpgrade()
    {
        if (currentSpeedNJumpIndex >= _prices.Length || EconomyManager.Instance.MoneyAmount < _prices[currentSpeedNJumpIndex])
            return;

        Helpers.PlayAudioSafely(PurchaseSound);
        EconomyManager.Instance.SpendMoney(_prices[currentSpeedNJumpIndex]);
        SpeedNJumpMultiplayer = _speedNJumpMultipliers[currentSpeedNJumpIndex];
        currentSpeedNJumpIndex++;
        UpdateMarketUI();
    }

    public void UpdateMarketUI()
    {
        DamagePriceText.text = $"{_prices[currentDamageIndex]}";
        var shouldDisable = (currentDamageIndex >= _prices.Length || EconomyManager.Instance.MoneyAmount < _prices[currentDamageIndex]);
        DamagePriceText.color = shouldDisable
            ? Color.gray
            : Color.white;
        DamageUpgradeButton.interactable = !shouldDisable;

        HealthPriceText.text = $"{_prices[currentHealthIndex]}";
        shouldDisable = (currentHealthIndex >= _prices.Length || EconomyManager.Instance.MoneyAmount < _prices[currentHealthIndex]);
        HealthPriceText.color = shouldDisable
            ? Color.gray
            : Color.white;
        HealthUpgradeButton.interactable = !shouldDisable;

        SpeedNJumpPriceText.text = $"{_prices[currentSpeedNJumpIndex]}";
        shouldDisable = (currentSpeedNJumpIndex >= _prices.Length || EconomyManager.Instance.MoneyAmount < _prices[currentSpeedNJumpIndex]);
        SpeedNJumpPriceText.color = shouldDisable
            ? Color.gray
            : Color.white;
        SpeedNJumpUpgradeButton.interactable = !shouldDisable;

        DamageLevelText.text = $"{currentDamageIndex + 1}";
        HealthLevelText.text = $"{currentHealthIndex + 1}";
        SpeedNJumpLevelText.text = $"{currentSpeedNJumpIndex + 1}";

        GoldText.text = $"{EconomyManager.Instance.MoneyAmount}";
    }
}
