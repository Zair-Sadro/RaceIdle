using UnityEngine;

public class BottomMergeDetect : MergeDetect
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(reqTag))
        {
            _mergeMaster.SetBottomCar(this);
          
        }
    }
}
