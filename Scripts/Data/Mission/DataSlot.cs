using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MemoryRestoreData memoryRestoreData;
    private TextMeshProUGUI  title;
    private GameObject memoryImage;

    private void Start()
    {
        title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        memoryImage = transform.parent.GetChild(8).gameObject;
    }

    private void Update()
    {
        if (memoryRestoreData != null)
        {
            title.text = memoryRestoreData.Title;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (memoryRestoreData.Title == "") return;

        if (memoryRestoreData != null)
        {
            memoryImage.SetActive(true);
            MemoryUIController.Instance.dataName.text = memoryRestoreData.Title;
            MemoryUIController.Instance.dataDescription.text = memoryRestoreData.Description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (memoryRestoreData != null)
        {
            memoryImage.SetActive(false);
            MemoryUIController.Instance.dataName.text = "";
            MemoryUIController.Instance.dataDescription.text = "";
        }
    }
}
