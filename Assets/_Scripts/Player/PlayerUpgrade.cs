using System;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour, ISaveLoad<PlayerData>
{

    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerController controller;
    [SerializeField] private TileSetter tileSetter;
    [SerializeField] private PlayerSlash playerSlash;
    [SerializeField] private PlayerUpgradeView upgradeView;

    [Header("Upgrades Number")]
    [SerializeField] private UpgradeNumbersData SpeedData;
    [SerializeField] private UpgradeNumbersData DamageData;
    [SerializeField] private UpgradeNumbersData CapacityData;

    public int SpeedLevel => SpeedData.currentLevel;

    public float SpeedValue => SpeedData.currentValue;
    public int DamageLevel => DamageData.currentLevel;
    public int CapacityLevel=> CapacityData.currentLevel;

    public event Action<int> OnSpeedUpgrade,OnDamageUpgrade,OnCapacityUpgrade;

    private void UpgradeSpeed()
    {
        if (SpeedData.IsMaxLevel)
            return;

        SpeedData.currentLevel++;
        SpeedData.currentValue += SpeedData.DeltaNumber;
        SpeedData.currentPriceValue *= SpeedData.DeltaNumberPrice;

        controller.SetSpeed(SpeedData.currentValue);

        OnSpeedUpgrade?.Invoke(SpeedData.currentLevel);
    }
    private void UpgradeCapacity()
    {
        if (CapacityData.IsMaxLevel)
            return;

        CapacityData.currentLevel++;
        CapacityData.currentValue += CapacityData.DeltaNumber;
        CapacityData.currentPriceValue *= CapacityData.DeltaNumberPrice;

        tileSetter.SetCapacity((int)CapacityData.currentValue);

        OnCapacityUpgrade?.Invoke(CapacityData.currentLevel);
    }
    private void UpgradeDamage()
    {
        if (DamageData.IsMaxLevel)
            return;

        DamageData.currentLevel++;
        DamageData.currentValue += DamageData.DeltaNumber;
        DamageData.currentPriceValue *= DamageData.DeltaNumberPrice;

        playerSlash.SetDamage((int)DamageData.currentValue);

        OnDamageUpgrade?.Invoke(DamageData.currentLevel);
    }
    public PlayerData GetData()
    {
        playerData.CapacityValue = CapacityData.currentValue;
        playerData.SpeedValue = SpeedData.currentValue;
        playerData.DamageValue = (int)DamageData.currentValue;

        playerData.CapacityLevel = CapacityData.currentLevel;
        playerData.SpeedLevel = SpeedData.currentLevel;
        playerData.DamageLevel = DamageData.currentLevel;

        playerData.CapacityPriceValue = CapacityData.currentPriceValue;
        playerData.SpeedPriceValue = SpeedData.currentPriceValue;
        playerData.DamagePriceValue = DamageData.currentPriceValue;

        return playerData;
    }

    public void Initialize(PlayerData data)
    {
        playerData = data;

        if (playerData.CapacityValue != 0) 
        {
            CapacityData.currentValue = playerData.CapacityValue;
            SpeedData.currentValue = playerData.SpeedValue;
            DamageData.currentValue = playerData.DamageValue;

            CapacityData.currentLevel = playerData.CapacityLevel;
            SpeedData.currentLevel = playerData.SpeedLevel;
            DamageData.currentLevel = playerData.DamageLevel;

            CapacityData.currentPriceValue = playerData.CapacityPriceValue;
            SpeedData.currentPriceValue = playerData.SpeedPriceValue;
            DamageData.currentPriceValue = playerData.DamagePriceValue;
        }
        if (SpeedData.currentValue < 9)
            SpeedData.currentValue = 10;

        if (CapacityData.currentValue < 9)
            CapacityData.currentValue = 10;

        upgradeView.InitSpeed(SpeedData, UpgradeSpeed);
        upgradeView.InitDamage(DamageData, UpgradeDamage);
        upgradeView.InitCapacity(CapacityData, UpgradeCapacity);


        controller.SetSpeed(SpeedData.currentValue);
        tileSetter.SetCapacity((int)CapacityData.currentValue);
        playerSlash.SetDamage((int)DamageData.currentValue);

    }


}
