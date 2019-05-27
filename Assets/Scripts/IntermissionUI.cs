using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntermissionUI : MonoBehaviour
{

    public Text cargoDelivered;
    public Text totalDelivered;
     
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        cargoDelivered.text = GameSingleton.Instance.SavedCargo.ToString();
        totalDelivered.text = GameSingleton.Instance.TotalSavedCargo.ToString();
    }

    public void GoToNextLevel()
    {
        GameSingleton.Instance.GameLevel++;
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
}
