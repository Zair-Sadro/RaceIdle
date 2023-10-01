using UnityEngine;

public class TopMergeDetect : MergeDetect
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(reqTag))
        {
            _mergeMaster.SetTopCar(this);
        }
    }
}
