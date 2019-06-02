using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntermissionUI : MonoBehaviour
{

    public Text cargoDelivered;
    public Text cargoLost;
    public Text cargoDeliveredTotal;

    public Text haulersRemaining;
    public Text haulersLost;
    public Text haulersLostTotal;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        cargoDelivered.text = GameSingleton.Instance.SavedCargo.ToString();
        cargoLost.text = GameSingleton.Instance.LostCargo.ToString();
        cargoDeliveredTotal.text = GameSingleton.Instance.TotalSavedCargo.ToString();

        haulersRemaining.text = GameSingleton.Instance.HaulerCount.ToString();
        haulersLost.text = GameSingleton.Instance.LostHaulers.ToString();
        haulersLostTotal.text = GameSingleton.Instance.LostHaulersTotal.ToString();
    }

    public void GoToNextLevel()
    {
        GameSingleton.Instance.GameLevel++;
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
}
