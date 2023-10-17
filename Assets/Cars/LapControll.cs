using UnityEngine;

public class LapControll : MonoBehaviour
{
    private WalletSystem _walletSystem => InstantcesContainer.Instance.WalletSystem;
    [SerializeField] private ParticleSystem _goldVFX;
    public void LapFinished(CarData cardata)
    {
        _walletSystem.Income(cardata.LapReward);
        //_particleSystem.Play();
    }
}


