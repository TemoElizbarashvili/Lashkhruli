using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public static Market Instance { get; private set; }

    #region Public Properties

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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateMarketUI();
    }

    public void BuyDamageUpgrade()
    {
        if (MarketManager.CurrentDamageIndex >= MarketManager.Prices.Length ||
            EconomyManager.MoneyAmount < MarketManager.Prices[MarketManager.CurrentDamageIndex])
            return;

        Helpers.PlayAudioSafely(PurchaseSound);
        EconomyManager.SpendMoney(MarketManager.Prices[MarketManager.CurrentDamageIndex]);
        GameEvents.RaiseAttackDamageIncreased(MarketManager.DamageAdditions[MarketManager.CurrentDamageIndex]);
        MarketManager.CurrentDamageIndex++;
        UpdateMarketUI();
    }

    public void BuyHealthUpgrade()
    {
        if (MarketManager.CurrentHealthIndex >= MarketManager.Prices.Length ||
            EconomyManager.MoneyAmount < MarketManager.Prices[MarketManager.CurrentHealthIndex])
            return;

        Helpers.PlayAudioSafely(PurchaseSound);
        EconomyManager.SpendMoney(MarketManager.Prices[MarketManager.CurrentHealthIndex]);
        MarketManager.CurrentHealthIndex++;
        UpdateMarketUI();
        GameEvents.RaiseHealthIncreased(MarketManager.HealthAdditions[MarketManager.CurrentHealthIndex]);
    }

    public void BuySpeedNJumpUpgrade()
    {
        if (MarketManager.CurrentSpeedNJumpIndex >= MarketManager.Prices.Length ||
            EconomyManager.MoneyAmount < MarketManager.Prices[MarketManager.CurrentSpeedNJumpIndex])
            return;

        Helpers.PlayAudioSafely(PurchaseSound);
        EconomyManager.SpendMoney(MarketManager.Prices[MarketManager.CurrentSpeedNJumpIndex]);
        GameEvents.RaiseSpeedIncreased(MarketManager.SpeedNJumpMultipliers[MarketManager.CurrentSpeedNJumpIndex]);
        MarketManager.CurrentSpeedNJumpIndex++;
        UpdateMarketUI();
    }

    public void UpdateMarketUI()
    {
        DamagePriceText.text = $"{MarketManager.Prices[MarketManager.CurrentDamageIndex]}";
        var shouldDisable = (MarketManager.CurrentSpeedNJumpIndex >= MarketManager.Prices.Length ||
                             EconomyManager.MoneyAmount < MarketManager.Prices[MarketManager.CurrentDamageIndex]);
        DamagePriceText.color = shouldDisable
            ? Color.gray
            : Color.white;
        DamageUpgradeButton.interactable = !shouldDisable;

        HealthPriceText.text = $"{MarketManager.Prices[MarketManager.CurrentHealthIndex]}";
        shouldDisable = (MarketManager.CurrentSpeedNJumpIndex >= MarketManager.Prices.Length ||
                         EconomyManager.MoneyAmount < MarketManager.Prices[MarketManager.CurrentHealthIndex]);
        HealthPriceText.color = shouldDisable
            ? Color.gray
            : Color.white;
        HealthUpgradeButton.interactable = !shouldDisable;

        SpeedNJumpPriceText.text = $"{MarketManager.Prices[MarketManager.CurrentSpeedNJumpIndex]}";
        shouldDisable = (MarketManager.CurrentSpeedNJumpIndex >= MarketManager.Prices.Length ||
                         EconomyManager.MoneyAmount < MarketManager.Prices[MarketManager.CurrentSpeedNJumpIndex]);
        SpeedNJumpPriceText.color = shouldDisable
            ? Color.gray
            : Color.white;
        SpeedNJumpUpgradeButton.interactable = !shouldDisable;

        DamageLevelText.text = $"{MarketManager.CurrentDamageIndex + 1}";
        HealthLevelText.text = $"{MarketManager.CurrentHealthIndex + 1}";
        SpeedNJumpLevelText.text = $"{MarketManager.CurrentSpeedNJumpIndex + 1}";

        GoldText.text = $"{EconomyManager.MoneyAmount}";
    }
}
