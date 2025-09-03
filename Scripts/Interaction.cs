using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public static Interaction Instance;
    public Action<ItemSO> itemActionOn;
    public Action interactActionOff;
    public Action shopActionOn;
    public Action boxActionOn;
    public Action boxOpenAction;
    public Action saveActionOn;
    public Action ShopInteractActionOn;
    
    public Action getItemActionOn;
    
    private Player player;
    private bool isBoxInteract = false;
    private bool isItemInteract = false;
    private bool isShopInteract = false;
    private bool isSaveInteract = false;
    private bool isSaveSelect = false;
    
    public bool isInteractOn = false;
    public Image endImage;
    private float fadeDuration = 0.5f;
    
    private void Awake()
    {
        TryGetComponent(out player);
        Instance = this;
    }

    private void Update()
    {
        SaveInteraction();
        ItemIneteract();
        ShopInteract();
        BoxInteraction();
        isInteractOn = (isBoxInteract || isItemInteract || isShopInteract || isSaveInteract); 
    }

    void SaveInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("SavePoint"));
        if (colliders.Length > 0)
        {
            IInteractable interactable = colliders[0].GetComponent<IInteractable>();
            if (interactable != null)
            {
                isSaveInteract = true;
                saveActionOn?.Invoke();
                if (isSaveSelect == true) return;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isSaveSelect = true;
                    interactable.Interact();
                    interactActionOff?.Invoke();
                    StartCoroutine(FadeIn());
                }
            }
        }
        else
        {
            isSaveInteract = false;
            if (isItemInteract != true && isShopInteract != true && isBoxInteract != true)
            {
                interactActionOff?.Invoke();
            }
        }
    }
    void BoxInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Box"));
        if (colliders.Length > 0)
        {
            BaseBox box = colliders[0].GetComponent<BaseBox>();
            if (box.isOpen == false)
            {
                isBoxInteract = true;
                boxActionOn?.Invoke();
                if (box != null)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        box.isOpen = true;
                        boxOpenAction?.Invoke();
                        interactActionOff?.Invoke();
                    }
                }
            }
        }
        else
        {
            isBoxInteract = false;
            if (isItemInteract != true && isShopInteract != true && isSaveInteract != true)
            {
                interactActionOff?.Invoke();
            }
        }
    }
    void ItemIneteract()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Item"));
        if (colliders.Length > 0)
        {
            GameItem go = colliders[0].GetComponent<GameItem>();
            itemActionOn?.Invoke(go.itemSo);
            isItemInteract = true;
            if (go != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    player.inventory.AddItem(go.itemSo, 1);
                    if (go.itemSo.itemType == Define.ItemType.EnergyCore)
                    {
                        Memory_Mission_Controller.Instance.Notify(TargetID.EnergyCore);
                    }
                    getItemActionOn?.Invoke();
                    Destroy(go.gameObject);
                }
            }
        }
        else
        {
            isItemInteract = false;
            if (isShopInteract != true && isBoxInteract != true && isSaveInteract != true)
            {
                interactActionOff?.Invoke();
            }
        }
    }
    void ShopInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Shop"));
        if (colliders.Length > 0)
        {
            shopActionOn?.Invoke();
            isShopInteract = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(CanvasController.Instance.Canvas_HUD.name);
                CanvasController.Instance.Canvas_HUD.SetActive(false);
                ShopInteractActionOn.Invoke();
            }
        }
        else
        {
            isShopInteract = false;
            if (isItemInteract != true && isBoxInteract != true && isSaveInteract != true)
            {
                interactActionOff?.Invoke();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        Color color = endImage.color; 
        Color targetColor = new Color(color.r, color.g, color.b, 1f); 
        
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(color.a, targetColor.a, timeElapsed / fadeDuration);
            endImage.color = new Color(color.r, color.g, color.b, alpha);

            timeElapsed += Time.unscaledDeltaTime * 0.5f;
            Debug.Log(endImage.color.a);
            yield return null; 
        }
        endImage.color = targetColor;
        Time.timeScale = 1f;
        Managers.Instance.Clear();
        GameManager.SelectMode = Define.SelectMode.LoadGame;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(currentSceneName);
    }
}
