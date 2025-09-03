using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;
    public GameObject Canvas_HUD;
    public GameObject Canvas_MenuShop;
    public GameObject HUDPanel;
    void Awake()
    {
        Instance = this;
    }
}
