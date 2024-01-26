using System;
using UnityEngine;

public class PlayerUpgradeTask : MonoBehaviour, IGameTask
{
    [SerializeField] private PlayerUpgrade player;
    [SerializeField] private int levelNeed;

    [Tooltip("0 - speed, 1 - damage, 2 - caapcity")]
    [SerializeField] private int upgradeIndx;

    public event Action TaskDone;

    public void StartTask()
    {
        switch (upgradeIndx)
        {
            case 0:
                player.OnSpeedUpgrade += CheckUpgradeLevel;
                break;

            case 1:
                player.OnDamageUpgrade += CheckUpgradeLevel;
                break;

            case 2:
                player.OnCapacityUpgrade += CheckUpgradeLevel;
                break;

        }

    }
    private void CheckUpgradeLevel(int currentLevel)
    {
        if (currentLevel >= levelNeed)
        {
            UnSubscribe();
            TaskDone?.Invoke();

        }

        void UnSubscribe()
        {
            switch (upgradeIndx)
            {
                case 0:
                    player.OnSpeedUpgrade -= CheckUpgradeLevel;
                    break;

                case 1:
                    player.OnDamageUpgrade -= CheckUpgradeLevel;
                    break;

                case 2:
                    player.OnCapacityUpgrade -= CheckUpgradeLevel;
                    break;

            }
        }
    }
}

