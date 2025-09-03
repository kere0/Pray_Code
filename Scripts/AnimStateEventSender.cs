using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimStateEventSender : StateMachineBehaviour
{
    [System.Serializable]
    public class AnimStateEvent
    {
        public string stateName;
        public float time;
        public bool IsComplete { get; set; }
    }
    public AnimStateEvent[] eventDatas; // 한 애니메이션에서 발생시킬 이벤트들
    //public List<AnimStateEvent> eventDatas = new List<AnimStateEvent>();
    private AnimStateEventListener animStateEventListener;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < eventDatas.Length; i++)
        {
            eventDatas[i].IsComplete = false;
        }
        if (animStateEventListener == null)
        {
            animStateEventListener = animator.gameObject.GetComponent<AnimStateEventListener>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < eventDatas.Length; i++)
        {
            AnimStateEvent eventData = eventDatas[i];
            if(eventData.IsComplete) continue;
            if(eventData.time > stateInfo.normalizedTime*stateInfo.length) continue;
            
            eventData.IsComplete = true;
            animStateEventListener.OccursAnimStateEvent(eventData.stateName);
        }
    }
}
