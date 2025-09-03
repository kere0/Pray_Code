using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryUIController : MonoBehaviour
{
    public static MemoryUIController Instance;
    public DataSlot[] dataSlot;
    public TextMeshProUGUI dataName;
    public TextMeshProUGUI dataDescription;
    public int DataNum = 7;
    private int currentMemoryIndex = 0;
    void Awake()
    {
        Instance = this;
    }
    public void MemoryUpdate()
    {
        currentMemoryIndex++;
        dataSlot[currentMemoryIndex-1].memoryRestoreData = Managers.DataManager.Get<MemoryRestoreData>($"M{currentMemoryIndex:D2}");
    }
}
