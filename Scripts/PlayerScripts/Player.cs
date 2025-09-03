using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerState
{
    FallState,
    GuardState,
    JUmpState,
    MoveState,
    SwordMoveState,
    SwordJumpState,
    SwordAttackState
}
[DefaultExecutionOrder(-100)]
public class Player : MonoBehaviour, IDamageAble, ISaveable
{
    public GameObject GameObject => gameObject;
    public MeshParts MeshParts => meshParts;
    public TargetID TargetID => targetID;
    public Collider Collider => cc;
    public GameObject TargetPoint => targetPoint;
    float IDamageAble.Hp => Hp;

    public float Hp => currentHp;
    public bool IsDie => isDie;
    public bool NormalMonster => normalMonster;
    public Define.CreatureType CreatureType => creatureType;
    public bool ExecutionOn => execution;
    public Dictionary<SkinnedMeshRenderer, Material> OriginMaterial { get; }
    public static Player Instance { get; private set; }
    public TargetID targetID;
    public MeshParts meshParts;
    private PlayerBaseState currentState;
    public Animator animator;
    public CharacterController cc;
    public GameObject targetPoint;
    public Camera camera;
    public AnimatorStateInfo stateInfo;
    public PlayerState playerState;
    public PlayerData playerData;
    public ForceReceiver forceReceiver;
    public bool isAttack = false;
    public Sword_Weapon sword;
    public Transform handSkillPoint;
    public Transform lookPosition;
    public bool isSword = false;
    public bool onDamage = false;
    public bool onDodge = false;
    public bool onDamageDelay = false;
    public AnimStateEventListener animStateEventListener;
    private bool stateStart = false;
    public Transform target;
    public bool isTarget;
    public bool onLeftDodge = false;
    public bool onRightDodge = false;
    public Inventory inventory;
    public float currentHp;
    public float attackDamage;
    public float attackSpeed;
    public bool isDie = false;
    public bool execution = false;

    public bool isGuardOn = false;
    public bool isSkillUse = false;
    private bool dieStateOn = false;

    private bool normalMonster = false;
    private Define.CreatureType creatureType = Define.CreatureType.Player;
    public ParticleSystem blinkParticle;

    public Vector3 playerPos;
    public bool staggerResistance = false;
    private void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TryGetComponent(out meshParts);
        TryGetComponent(out forceReceiver);
        TryGetComponent(out animStateEventListener);
        TryGetComponent(out animator);
        TryGetComponent(out cc);
        TryGetComponent(out inventory);
        targetID = TargetID.Player;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        RegisterToSaveManager();
        CombatSystem.Instance.RegisterCreature(cc, this);
        cc.enabled = false;
    }
    private void Start()
    {
        currentState = new MoveState(this);
        currentState.Enter();  
        playerState = PlayerState.MoveState;
        LoadData();
    }
    private void Update()
    {
        currentState.Execute();
        CurrentAnimState();
    }
    public void ChangeState(PlayerBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void CurrentAnimState()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    public void HpHealing(int healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0, playerData.Hp);
    }
    public void TakeDamage(int damage, bool onDamage, bool isGuardIgnore)
    {
        if (currentHp > 0)
        {
            if (onDodge == false && onDamageDelay == false && isSkillUse == false)
            {
                this.onDamage = onDamage;
                if (isGuardOn == false)
                {
                    GetDamage(damage);
                    if (staggerResistance == false)
                    {
                        ChangeState(new PlayerTakeDamageState(this));
                    }
                }
                Debug.Log(currentHp);
            }
        }
        if(isDie == true)
        {
            if (dieStateOn == false)
            {
                dieStateOn = true;
                ChangeState(new PlayerDeathState(this));
            }
        }
    }
    public void TakeKnockback(Vector3 knockbackForce, float duration)
    {
        if (onDodge == false && onDamageDelay == false && isGuardOn == false)
        {
            forceReceiver.AddForce(knockbackForce, duration);
        }
    }
    public void CoroutineController(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
        //Coroutine c = StartCoroutine(coroutine);
        //StopCoroutine(c);
    }
    public void GetDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, playerData.Hp);
        PlayerHpBar.Instance.SetHPAction?.Invoke();
        Debug.Log(currentHp + "내체력~~~~~~~~~~~~~~~~~~");
        if (currentHp <= 0)
        {
            isDie = true;
        }
    }
    public void SaveData()
    {
        SaveSnapshot.Instance.saveData.playerData = playerData;
        //SaveSnapshot.Instance.saveData.currentHp = currentHp;
    }
    public void LoadData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                CanvasController.Instance.HUDPanel.SetActive(true);
                InitData();
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    Debug.Log($"세이브 파일이 없음: {Managers.SaveManager.SavePath}");
                    CanvasController.Instance.HUDPanel.SetActive(true);
                    InitData();
                    return;
                }
                CanvasController.Instance.HUDPanel.SetActive(true);
                playerData = SaveSnapshot.Instance.saveData.playerData; 
                Debug.Log(playerData.Hp + "진짜 체력");
                transform.SetPositionAndRotation(SaveSnapshot.Instance.saveData.savePos, SaveSnapshot.Instance.saveData.saveRot);
                //currentHp = SaveSnapshot.Instance.saveData.currentHp;
                currentHp = playerData.Hp;
                PlayerHpBar.Instance.Init();
                PlayerHpBar.Instance.SetHPAction?.Invoke();
                transform.position = SaveSnapshot.Instance.saveData.savePos;
                transform.rotation = SaveSnapshot.Instance.saveData.saveRot;
                cc.enabled = true;
                break;
        }
    }
    private void InitData()
    {
        playerData = Managers.DataManager.Get<PlayerData>("P01");   
        currentHp = playerData.Hp;
        PlayerHpBar.Instance.Init();
        PlayerHpBar.Instance.SetHPAction?.Invoke();
        Debug.Log("데이터 초기값");
        cc.enabled = true;
    }
    public void RegisterToSaveManager()
    {
        Managers.SaveManager.Register(this);
    }
}
