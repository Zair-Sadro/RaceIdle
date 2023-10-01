using UnityEngine;

public class BottomMergeDetect : MergeDetect
{
    [SerializeField] private CarAI _carAi;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(reqTag))
        {
            _mergeMaster.SetBottomCar(this,_carAi.CurrentPoint);
          
        }
    }
}
