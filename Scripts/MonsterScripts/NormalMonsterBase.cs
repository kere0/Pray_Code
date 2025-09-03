using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NormalMonsterBase : MonoBehaviour, IDamageAble, IQuestCheck
{
    public enum MonsterState
    {
        Idle,
        Move,
        Attack,
        TakeDamage,
        Death
    }
    public TargetID TargetID => targetID;
    public Collider Collider => ICollider;

    public GameObject TargetPoint => targetPoint;
    public GameObject GameObject => gameObject;
    public MeshParts MeshParts => meshParts;
    bool IQuestCheck.QuestCheck => isQuestObject;
    public float Hp => currentHp;
    public bool IsDie => isDie;
    public bool NormalMonster => normalMonster;
    public Define.CreatureType CreatureType => creatureType;
    public bool ExecutionOn => execution;
    public Dictionary<SkinnedMeshRenderer, Material> OriginMaterial => originMaterialDict;
    public Collider LeftHand;
    public Collider RightHand;

    public MonsterState state;
    
    public GameObject targetPoint;
    public AnimatorStateInfo stateInfo;
    [FormerlySerializedAs("forceReciever")] public ForceReceiver forceReceiver;
    public AnimStateEventListener animStateEventListener;
    public TargetID targetID;
    public LayerMask layerMask;
    public bool execution = false;
    public bool normalMonster = true;
    public Define.CreatureType creatureType = Define.CreatureType.Normal;
    public float speed = 3f;
    public bool isAttacking = false;
    public float rand;
    public float maxHp = 10;
    public float currentHp;
    public NavMeshAgent agent;
    public MeshParts meshParts;
    public Player player;
    public Animator animator;
    public CharacterController cc;
    public float detectRange;
    public bool isDie = false;
    public bool isQuestObject = false;
    public bool isCircle = false;
    public GameObject attackCircle;
    public bool ExecutionInvoke = false;
    public float attackRange = 2.5f;

    public Collider ICollider;
    public Vector3 startPosition;
    public Dictionary<SkinnedMeshRenderer, Material> originMaterialDict = new Dictionary<SkinnedMeshRenderer, Material>();
    private Coroutine restoreCoroutine;
    protected virtual void Awake()
    {
        TryGetComponent(out agent);
        TryGetComponent(out meshParts);
        TryGetComponent(out forceReceiver);
        TryGetComponent(out animStateEventListener);
        TryGetComponent(out animator);
        TryGetComponent(out cc);
        agent.updateRotation = false;
        agent.updatePosition = false;
        layerMask = LayerMask.GetMask("Player");
    }
    protected virtual void Start()
    {
        // GameManager.Instance.GameStartAction -= Init;
        // GameManager.Instance.GameStartAction += Init;
        startPosition = transform.position;
        SkinnedMeshRenderer[] renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (originMaterialDict.ContainsKey(renderer) == false)
            {
                originMaterialDict.Add(renderer, renderer.material);
            }
        }
    }
    public void SetMaterial(Material mat)
    {
        
        foreach (var originMaterial in originMaterialDict)
        {
            originMaterial.Key.material = mat;
        }

        if (restoreCoroutine != null)
        {
            StopCoroutine(restoreCoroutine);
        }
        restoreCoroutine = StartCoroutine(BackUpMaterialCoroutine());
    }

    IEnumerator BackUpMaterialCoroutine()
    {
        yield return new WaitForSeconds(5);
        foreach (var originMaterial in originMaterialDict)
        {
            originMaterial.Key.material = originMaterial.Value;
        }
        restoreCoroutine  = null;
    }
    public void Detect()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, detectRange, layerMask);
        if (collider.Length > 0)
        {
            state = MonsterState.Move;
        }
    }
    public void Move()
    {
        if (GameManager.Instance.isBossBattle == true)
        {
            agent.SetDestination(startPosition);
            Vector3 direction = agent.desiredVelocity.normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            cc.Move((direction + forceReceiver.Movement) * speed * Time.deltaTime);
            agent.nextPosition = transform.position;
            if (1f >= Vector3.Distance(startPosition, transform.position))
            {
                state = MonsterState.Idle;
            }
        }
        else
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, detectRange, layerMask);
            if (collider.Length == 0)
            {
                state = MonsterState.Idle;
            }
            else
            {
                player = collider[0].gameObject.GetComponent<Player>();
                if (state == MonsterState.Move)
                {
                    agent.SetDestination(player.transform.position);
                    Vector3 direction = agent.desiredVelocity.normalized;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                    }
                    //Vector3 moveDir = agent.nextPosition - transform.position;
                    direction.y = 0;
                    
                    cc.Move((direction + forceReceiver.Movement) * speed * Time.deltaTime);
                    //Debug.Log($"[Monster] isOnNavMesh:{agent.isOnNavMesh}, hasPath:{agent.hasPath}, status:{agent.pathStatus}, areaMask:{agent.areaMask}");
                    agent.nextPosition = transform.position;
                    if (attackRange >= Vector3.Distance(player.transform.position, transform.position))
                    {
                        state = MonsterState.Attack;
                    }
                }
            }
        }
    }

    public void QuestCheck()
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
    public void AnimPlay(string animName) //AnimStopCheck
    {
        animator.Play(animName);
        if (stateInfo.normalizedTime >= 1.0f)
        {
            agent.enabled = true;
            state = MonsterState.Move;
            isAttacking = false;
            isCircle = false;
        }
    }
    public virtual void ExecutionCheck()
    {
        if (isDie == false)
        {
            if (ExecutionInvoke == false)
            {
                if ((currentHp / maxHp) <= 0.3f)
                {
                    execution = true;
                    CircleManager.Instance.ExecuteCircleAdd(cc);
                    CircleManager.Instance.ExecutionAction.Invoke(cc, targetPoint.transform.position);
                    ExecutionInvoke = true;
                }
            }
        }
    }
    public void TakeDamage(int Damage, bool OnDamage, bool isGuardIgnore)
    {
        if (currentHp > 0)
        {
            currentHp -= Damage;
            Debug.Log(currentHp);
            if (normalMonster == true || isGuardIgnore == true)
            {
                state =  MonsterState.TakeDamage;
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
    public IEnumerator CircleListRemove(Collider collider)
    {
        yield return new WaitForSeconds(0.4f);
        if (CircleManager.Instance.circleMonsterList.Contains(collider))
        {
            CircleManager.Instance.CircleMonsterRemove(collider);
        }
    }
}
