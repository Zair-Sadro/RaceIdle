using UnityEngine;

public class LapControll : MonoBehaviour
{
    private WalletSystem _walletSystem => InstantcesContainer.Instance.WalletSystem;
    [SerializeField] private ParticleSystem _goldVFX;
    public void LapFinished(CarData cardata)
    {
        if (cardata.LapReward > 0)
        {
            _walletSystem.Income(cardata.LapReward);

            InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.FINISHRACE);
        }

        //_particleSystem.Play();
    }
}


