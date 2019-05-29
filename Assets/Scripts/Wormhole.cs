using UnityEngine;

public class Wormhole : MonoBehaviour
{

    public bool playerCanEnter = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "PlayerShip")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");
            if (playerCanEnter)
            {
                // Picked up by LevelController
                EventManager.TriggerEvent("playerExitedLevel");
            }
        }


    }
}
