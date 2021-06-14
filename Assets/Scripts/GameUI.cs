using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    public Text savedCargoText;

    private int haulerCount = 0;
    public Text haulerText;
    public Text savedHaulerText;
    public Text missileText;

    private UnityAction playerDamageListener;
    private UnityAction playerMissileFiredListener;
    private bool countCargo = false;
    private bool countHaulers = false;

    private GameObject player;
    private PlayerThruster playerThruster;
    private PlayerController playerController;
    public Image boostRing;
    public Image zoomDurationRing;
    public Image zoomCooldownRing;
    public Image hullRing;
    public Image haulerHullRing;
  

    public RectTransform healthBar;

    private Hull playerHull;
    protected Camera playercamera;

    public GameObject ExitButton;

    void Awake()
    {
        playerDamageListener = new UnityAction(UpdateHullRing);
        playerMissileFiredListener = new UnityAction(UpdateMissileText);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        EventManager.StartListening("playerDamage", playerDamageListener);
        EventManager.StartListening("missileFired", playerMissileFiredListener);

        player = GameObject.Find("PlayerShip");
        if (player!=null)
        {
            playerThruster = player.GetComponent<PlayerThruster>();
            playerController = player.GetComponent<PlayerController>();
            playerHull = player.GetComponent<Hull>();
        }
        if (GameObject.Find("PlayerCamera"))
        {
            playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        }
        boostRing.fillAmount = 0f;
        zoomDurationRing.fillAmount = 0f;
        zoomCooldownRing.fillAmount = 0f;
        UpdateSaveCargoText(0);
        UpdateSaveHaulerText(0);
        UpdateMissileText();

        ExitButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ExitButton.SetActive(!ExitButton.activeSelf);
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked: CursorLockMode.None;
        }
        UpdateBoostRing();
        UpdateZoomDurationRing();
        UpdateZoomCooldownRing();
        //UnitStatusUI();
    }

    public void AddHaulerRing()
    {
        Image newRing = Instantiate(haulerHullRing, Vector3.zero, Quaternion.identity) as Image;
        //haulerHullRings.Add(newRing);
    }

    bool UnitStatusUI()
    {
        if (playercamera == null) return false;
        // Health bars for haulers and enemies

        GameObject[] haulers = GameObject.FindGameObjectsWithTag("Hauler");

        Vector3 pos = haulers[0].transform.position;  // get the game object position
        Vector2 viewportPoint = playercamera.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        healthBar.anchorMin = viewportPoint;
        healthBar.anchorMax = viewportPoint;
        healthBar.GetComponent<Image>().fillAmount = haulers[0].GetComponent<Hull>().GetHullPercentage();

        return true;
    }
    void OnDisable()
    {
        StopListeners();
    }

    private void OnDestroy()
    {
        StopListeners();
    }

    private void StopListeners()
    {
        EventManager.StopListening("playerDamage", playerDamageListener);
        EventManager.StopListening("missileFired", playerMissileFiredListener);
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

    private void UpdateMissileText()
    {
        if (missileText)
        {
            missileText.text = "MISSILES: " + GameSingleton.Instance.Missiles.ToString();
        }
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

    public void UpdateSaveCargoText(int savedCargo)
    {
        if (savedCargoText)
        {
            savedCargoText.text = "CARGO DELIVERED: " + savedCargo.ToString();
        }

    }

    public void UpdateSaveHaulerText(int savedHaulers)
    {
        if (savedHaulerText)
        {
            savedHaulerText.text = "HAULERS ESCAPED: " + savedHaulers.ToString();
        }

    }

    public void UpdateHaulerCountText(int hC)
    {
        if (haulerText)
        {
            haulerText.text = "HAULERS REMAINING: " + hC.ToString();
        }

    }

    public void UpdateHullRing()
    {
        hullRing.fillAmount = playerHull.GetHullCurrent() / playerHull.hullBase;
    }


    public void ExitGame()
    {
        SceneManager.LoadScene("Intermission", LoadSceneMode.Single);
        //Application.Quit();
    }
}
