using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class FlameBoss : MonoBehaviour, IDamageAble, IQuestCheck
{
    public GameObject GameObject => gameObject;
    public MeshParts MeshParts => meshParts;
    public TargetID TargetID => targetID;
    public Collider Collider { get; }
    public GameObject TargetPoint => targetPoint;
    public bool QuestCheck => isQuestObject;

    public float Hp => currentHp;
    public bool IsDie => isDie;
    public bool NormalMonster => normalMonster;
    public Define.CreatureType CreatureType => creatureType;
    public bool ExecutionOn => execution;
    public Dictionary<SkinnedMeshRenderer, Material> OriginMaterial => originMaterial;

    public TargetID targetID;
    public CharacterController cc;
    public Animator animator;
    public Collider collider;
    private FlameBossBaseState currentState;
    public MeshParts meshParts;
    public ForceReceiver forceReceiver;
    public AnimStateEventListener animStateEventListener;
    public GameObject targetPoint;
    public Transform target;
    private float maxHp = 100f;
    private float currentHp;
    private bool isDie = false;
    private bool execution = false;
    public AnimatorStateInfo animStateInfo;
    private bool OnActive= false;
    public bool onDamage = false;
    public HandAttack handAttack;
    public Hammer hammer;
    //public List<Collider> colliders = new List<Collider>();
    private bool normalMonster = false;
    private Define.CreatureType creatureType = Define.CreatureType.Boss;
    private bool isQuestObject = false;
    public Collider[] colliders;
    public Collider[] legColliders;
    public CinemachineImpulseSource impulseSource;
    public GameObject flameBossBlock;
    public Dictionary<SkinnedMeshRenderer, Material> originMaterial = new Dictionary<SkinnedMeshRenderer, Material>();


    void Awake()
    {
        TryGetComponent(out cc);
        TryGetComponent(out animator);
        TryGetComponent(out collider);
        TryGetComponent(out meshParts);
        TryGetComponent(out forceReceiver);
        TryGetComponent(out animStateEventListener);
        targetID = TargetID.FlameBoss;
        currentHp = maxHp;
        colliders = transform.GetComponentsInChildren<Collider>();
        TryGetComponent(out impulseSource);
    }

    void Start()
    {
        CombatSystem.Instance.RegisterCreature(collider, this);
        //var allCol = transform.GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            if (CombatSystem.Instance.creatureDic.ContainsKey(col)== false)
            {
                CombatSystem.Instance.RegisterCreature(col, this);
            }
        }
        SkinnedMeshRenderer[] renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (originMaterial.ContainsKey(renderer) == false)
            {
                originMaterial.Add(renderer, renderer.material);
            }
        }
    }
    void Update()
    {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (OnActive == false)
        {
            if (animStateInfo.IsName("Idle"))
            {
                currentState = new FlameBossIdleState(this);
                OnActive = true;
                currentState.Enter();
            }
        }
        if (OnActive == false) return;
        currentState.Execute();
        ExecutionCheck();
        OnQuestCheck();
    }

    public void ChangeState(FlameBossBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void ExecutionCheck()
    {
        if ((currentHp / maxHp) < 0.3f)
        {
            execution = true;
        }
    }
    public void OnQuestCheck()
    {
        if (targetID.ToString() == Memory_Mission_Controller.Instance.currentQuestData.TargetID)
        {
            isQuestObject = true;
        }
        else
        {
            isQuestObject = false;
        }
    }
    public void CoroutineController(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    public void TakeDamage(int damage, bool onDamage, bool isGuardIgnore)
    {
        if (currentHp > 0)
        {
            hammer.hammerCollider.enabled = false;
            handAttack.handAttackCollider.enabled = false;
            currentHp -= damage;
            this.onDamage = onDamage;
            if (damage >= 15)
            {
                ChangeState(new FlameBossHitState(this));
            }
        }
        else
        {
            hammer.hammerCollider.enabled = false;
            handAttack.handAttackCollider.enabled = false;
            currentHp = 0;
            isDie = true;
            Memory_Mission_Controller.Instance.Notify(targetID);
            ChangeState(new FlameBossDeathState(this));
        }
    }
    public void TakeKnockback(Vector3 knockbackForce, float duration)
    {
        if (isDie == false)
        {
            forceReceiver.AddForce(knockbackForce, duration);
        }
    }

    public void ShakeCamera()
    {
        Debug.Log("흔들림~~~~~~~~~~~~~~~~");
        impulseSource.GenerateImpulse();
    }
}
