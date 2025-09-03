using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class KnightBoss : MonoBehaviour, IDamageAble, IQuestCheck
{
    public GameObject GameObject => gameObject;
    public MeshParts MeshParts => meshParts;
    public TargetID TargetID => targetID;
    public Collider Collider { get; }
    public GameObject TargetPoint => targetPoint;
    public float Hp => currentHp;
    public bool IsDie => isDie;
    public bool NormalMonster => normalMonster;
    public Define.CreatureType CreatureType => creatureType;
    public bool ExecutionOn => execution;
    public Dictionary<SkinnedMeshRenderer, Material> OriginMaterial => originMaterial;
    public bool QuestCheck => isQuestObject;
    
    private TargetID targetID;
    public MeshParts meshParts;
    public Animator animator;
    public NavMeshAgent agent;
    private BossBaseState currentState;
    public CharacterController knightBossController;
    public ForceReceiver forceReceiver;
    public GameObject targetPoint;
    private BossStatus status;
    public Transform target;
    public AnimStateEventListener animStateEventListener;
    public KnightBoss_Weapon KnightBossWeapon;
    public AnimatorStateInfo stateInfo;
    public bool onDamage = false;
    private bool stateStart = false;
    private bool isDie = false;
    private float currentHp;
    private bool execution = false;
    private bool dieStateOn = false;
    private bool normalMonster = false;
    private Define.CreatureType creatureType = Define.CreatureType.Boss;
    public bool isQuestObject = false;
    public KnightBossCutSceneController KnightBossCutSceneController;
    public GameObject knightBossBlock;
    public GameObject knightBossBlock2;
    public Dictionary<SkinnedMeshRenderer, Material> originMaterial = new Dictionary<SkinnedMeshRenderer, Material>();
    void Start()
    {
        targetID = TargetID.DeathKnight;
        status = new BossStatus(100);
        TryGetComponent(out meshParts);
        TryGetComponent(out animator);
        TryGetComponent(out agent);
        TryGetComponent(out knightBossController);
        TryGetComponent(out forceReceiver);
        TryGetComponent(out animStateEventListener);
        CombatSystem.Instance.RegisterCreature(knightBossController, this);
        currentState = new KnightBossDetectState(this);
        agent.updatePosition = false;
        agent.updateRotation = false;
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
        ExecutionCheck();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        currentState.Execute();
        OnQuestCheck();
    }
    void ExecutionCheck()
    {
        if ((status.CurrentHp / status.MaxHp) < 0.3f)
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
    public void ChangeState(BossBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void CoroutineController(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    public void TakeDamage(int damage, bool onDamage, bool isGuardIgnore)
    {
        if (status.CurrentHp > 0)
        {
            KnightBossWeapon.GetComponent<Collider>().enabled = false;
            status.GetDamage(damage);
            this.onDamage = onDamage;
        }
        if(status.IsDie == true)
        {
            if (dieStateOn == false)
            {
                KnightBossWeapon.GetComponent<Collider>().enabled = false;
                dieStateOn = true;
                isDie = status.IsDie;
                Memory_Mission_Controller.Instance.Notify(targetID);
                ChangeState(new KnightBossDeathState(this));
            }
        }
    }
    public void TakeKnockback(Vector3 knockbackForce, float duration)
    {
        if (isDie == false)
        {
            forceReceiver.AddForce(knockbackForce, duration);
            
        }
    }
}
