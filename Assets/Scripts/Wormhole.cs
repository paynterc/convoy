using UnityEngine;

public class Wormhole : MonoBehaviour
{

    public bool playerCanEnter = false;

    void OnTriggerEnter(Collider collision)
    {

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Player")
        {
            if (playerCanEnter)
            {
                // Picked up by LevelController
                EventManager.TriggerEvent("playerExitedLevel");
            }
        }
        else if(collision.gameObject.tag == "Cargo")
        {
            EventManager.TriggerEvent("cargoEnteredWormhole");
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Hauler")
        {
            EventManager.TriggerEvent("haulerEnteredWormhole");
            collision.gameObject.SetActive(false);
        }


    }
}
