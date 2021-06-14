using UnityEngine;
using System.Collections;

// For manually adding units to the overlay. Mostly for testing.
public class RegisterWithUnitOverlay : MonoBehaviour
{
    public UnitOverlayUi overlayUi;

    // Use this for initialization
    void Start()
    {
        overlayUi = GameObject.Find("UIController").GetComponent<UnitOverlayUi>();
        overlayUi.RegisterUnit(this.gameObject);

    }

}
