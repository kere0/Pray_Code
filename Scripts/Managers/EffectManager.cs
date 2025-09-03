using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public List<GameObject> effectPrefabs;
    void Start()
    {
        Instance = this;
        CombatSystem.Instance.effectEvent -= PlayEffect;
        CombatSystem.Instance.effectEvent += PlayEffect;
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/ImpactLightning"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/Blood4"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/Blood6"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/Blood7"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/Blood12"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/DimensionReaper"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/IonStorm"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/PulseFlare"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/PlasmaSurge"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/VolcanicBurst"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/FlamewaveBurst"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/FinishEvent"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/FinishParticle"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/FireFoot"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/Meteor"));
        effectPrefabs.Add(Resources.Load<GameObject>("Effects/PlayerSkill/CountAttackSkill"));
        effectPrefabs.Add(Resources.Load<GameObject>("BlinkCircle"));
        effectPrefabs.Add(Resources.Load<GameObject>("ExecuteCircle"));
        effectPrefabs.Add(Resources.Load<GameObject>("StingParticle"));

        for (int i = 0; i < effectPrefabs.Count; i++)
        {
            // Pool 세팅

            Managers.PoolManager.ObjInit(effectPrefabs[i],3);
        
        }
    }
    public void PlayEffect(CombatEvent ev)
    {
        if (ev.EffectName == "Blood12")
        {
            GameObject go = Managers.PoolManager.ObjPop(ev.EffectName, Vector3.zero);
            go.transform.parent = ev.Receiver.GameObject.transform;
            go.transform.position = ev.EffectPosition;
            StartCoroutine(EffectSet(go));
            //go.transform.rotation = ev.Receiver.GameObject.transform.rotation;
            go.transform.rotation =Quaternion.LookRotation(-ev.HitNormal, Vector3.up);
        }
        else
        {
            GameObject go = Managers.PoolManager.ObjPop(ev.EffectName,ev.EffectPosition);
            go.transform.parent = ev.Receiver.GameObject.transform;
            //go.transform.position = ev.EffectPosition;
            go.transform.rotation =Quaternion.LookRotation(-ev.HitNormal, Vector3.up);
        }
    }

    IEnumerator EffectSet(GameObject go)
    {
        yield return new WaitForSeconds(0.7f);
        if (go != null)
        {
            go.transform.parent = null;
        }
    }
}
