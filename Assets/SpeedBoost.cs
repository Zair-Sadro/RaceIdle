using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerUpgrade playerUpgrade;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Player")
        player.SetSpeed(17.1f);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            if (player.MaxSpeed >= 17.5f)
                return;
            player.SetSpeed(playerUpgrade.SpeedValue);
        }
          
    }

}

