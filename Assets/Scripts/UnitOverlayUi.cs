using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Overlay game units with health bars and/or labels in the UI canvas
public class UnitOverlayUi : MonoBehaviour
{

    public RectTransform healthGraphic;// Overlay to display health status
    protected Camera playercamera;
    List<GameObject> units = new List<GameObject>();// Game units to track in the overlay
    List<RectTransform> healthBars = new List<RectTransform>();
    public Canvas uicanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Set camera
        playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        UnitStatusUI();
    }

    // Register a unit to track in the UI
    public void RegisterUnit(GameObject newUnit)
    {
        units.Add(newUnit);
        // Create a new healthbar
        RectTransform newHealthbar = Instantiate(healthGraphic, Vector3.zero, Quaternion.identity);
        newHealthbar.transform.SetParent(uicanvas.transform,false);
        healthBars.Add(newHealthbar);
    }

    private bool UnitStatusUI()
    {
        if (playercamera == null) return false;
        // Health bars for haulers and enemies

        //GameObject[] haulers = GameObject.FindGameObjectsWithTag("Hauler");

        for (int i = 0; i < healthBars.Count; i++)
        {
            if (!healthBars[i]) continue;
            if (!units[i] || !units[i].activeInHierarchy)
            {
                healthBars[i].gameObject.SetActive(false);
            }
            else
            {
                Vector3 pos = units[i].transform.position;  // get the game object position
                Vector3 viewportPoint = playercamera.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
                if (viewportPoint.z < 0)
                {
                    // do not draw
                    healthBars[i].gameObject.SetActive(false);
                }
                else
                {
                    healthBars[i].gameObject.SetActive(true);
                    healthBars[i].anchorMin = viewportPoint;
                    healthBars[i].anchorMax = viewportPoint;
                    healthBars[i].GetComponent<Image>().fillAmount = units[i].GetComponent<Hull>().GetHullPercentage();
                }
            }
        }


 
        return true;
    }

}
