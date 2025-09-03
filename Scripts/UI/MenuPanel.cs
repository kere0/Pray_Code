using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPanel : MonoBehaviour, IPointerClickHandler
{
    public int menuNum;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        MenuController.menuNum = menuNum;
    }
}
