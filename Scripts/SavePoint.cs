using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public SavePointSO savePointSo;
    public Transform savePointTransform;
    
    public void Interact()
    {
        SaveSnapshot.Instance.saveData.savePointId = savePointSo.id;
        SaveSnapshot.Instance.saveData.savePos = savePointTransform.position;
        SaveSnapshot.Instance.saveData.saveRot = savePointTransform.rotation;
        Managers.SaveManager.SaveAll();
        Debug.Log("세이브 ");
    }
}

