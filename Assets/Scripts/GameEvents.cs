using System;
using UnityEngine;

public static class GameEvents
{
    #region Skills

    public static event Action<int> OnHealthIncreased;
    public static event Action<float> OnSpeedIncreased;
    public static event Action<int> OnAttackDamageIncreased;

    public static void RaiseHealthIncreased(int amount)
        => OnHealthIncreased?.Invoke(amount);

    public static void RaiseSpeedIncreased(float amount)
        => OnSpeedIncreased?.Invoke(amount);

    public static void RaiseAttackDamageIncreased(int amount)
        => OnAttackDamageIncreased?.Invoke(amount);

    #endregion

    #region Economy

    public static event Action OnMoneyChanged;

    public static void RaiseMoneyChanged()
    {
        Debug.Log("Money Changed Rised!!!");
        OnMoneyChanged?.Invoke();
    }

    #endregion
}
