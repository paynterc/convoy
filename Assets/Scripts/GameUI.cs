using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{
    // Great Vid: https://www.youtube.com/watch?v=_RIsfVOqTaE
    // Select Canvas and change Canvas Scaler to scale with screen.
    // In game window, change resolution to 16:9 (next to Display dropdown)
    // Update Canvas Scaler resolution to 1600 x 900
    // Use shift+alt to set anchors on elements
    // Be sure to include using UnityEngine.UI at the top of your script
    // Dafont for fonts. https://www.dafont.com/fr/
    // Google fonts: https://fonts.google.com/

    private int cargoCount = 0;
    public Text cargoText;

    private UnityAction cargoListener;
    private bool countCargo = false;

    private GameObject player;
    private PlayerThruster playerThruster;
    private PlayerController playerController;
    public Image boostRing;
    public Image zoomDurationRing;
    public Image zoomCooldownRing;


    void Awake()
    {
        cargoListener = new UnityAction(SetCargoCount);

    }

    void Start()
    {
        EventManager.StartListening("cargoUpdate", cargoListener);
        player = GameObject.Find("PlayerShip");
        if (player!=null)
        {
            playerThruster = player.GetComponent<PlayerThruster>();
            playerController = player.GetComponent<PlayerController>();
        }
        boostRing.fillAmount = 0f;
        zoomDurationRing.fillAmount = 0f;
        zoomCooldownRing.fillAmount = 0f;
    }

    void Update()
    {
        UpdateBoostRing();
        UpdateZoomDurationRing();
        UpdateZoomCooldownRing();

    }

    private void LateUpdate()
    {
        if (countCargo)
        {
            countCargo = false;
            cargoCount = GameObject.FindGameObjectsWithTag("Cargo").Length;
            cargoText.text = "CARGO REMAINING: " + cargoCount;
        }
    }

    void OnDisable()
    {
        EventManager.StopListening("cargoUpdate", cargoListener);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("cargoUpdate", cargoListener);
    }


    public void SetCargoCount()
    {
        countCargo = true;

    }

    private bool UpdateBoostRing()
    {
        if (playerThruster == null) return false;

        // Ring fills up then erases itself over the duration of cooldown.
        if (Time.time < playerThruster.GetBoostCooldownTime())
        {
            boostRing.fillAmount = (playerThruster.GetBoostCooldownTime() - Time.time) / playerThruster.boostRateCurr;
        }
        else
        {
            boostRing.fillAmount = 0f;
        }

        return true;
    }

    private bool UpdateZoomDurationRing()
    {
        if (playerController == null) return false;

        // Ring fills up then erases itself over the duration of cooldown.
        if (Time.time < playerController.GetZoomTimer())
        {
            zoomDurationRing.fillAmount = (playerController.GetZoomTimer() - Time.time) / playerController.zoomDuration;
        }
        else
        {
            zoomDurationRing.fillAmount = 0f;
        }

        return true;
    }

    private bool UpdateZoomCooldownRing()
    {
        if (playerController == null) return false;

        // Ring fills up then erases itself over the duration of cooldown.
        if (Time.time < playerController.GetZoomCooldownTimer())
        {
            zoomCooldownRing.fillAmount = (playerController.GetZoomCooldownTimer() - Time.time) / playerController.zoomRate;
        }
        else
        {
            zoomCooldownRing.fillAmount = 0f;
        }

        return true;
    }

}
