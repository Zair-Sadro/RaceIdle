using System.Collections;
using UnityEngine;

public class FirstAutoRepair : AutoRepair
{
    [SerializeField] private ParticleSystem _destroyVFX;

    [SerializeField] private GameObject _counterForNextAutoRep;
    [SerializeField] private GameObject _destroyedBuilding;

    [SerializeField] private float _destroyedObjAppDelay;

    protected override IEnumerator Repair()
    {
        if (repairLevel <= 3)
            yield return base.Repair();
        else
        {
            _destroyVFX.Play();
            yield return new WaitForSeconds(_destroyedObjAppDelay);

            _destroyedBuilding.SetActive(true);
            _counterForNextAutoRep.SetActive(true);

            Destroy(this.gameObject);

        }
    }
    
}

