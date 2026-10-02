using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
    public string floatName;
    public bool updateOnStateEnter, updateOnStateExit;
    public bool updateOnStateMachineEnter, updateOnStateMachineExit;
    public float valueOnEnter, valueOnExit;

    private void SetAnimatorFloat(Animator animator, float value)
    {
        if (!string.IsNullOrEmpty(floatName))
        {
            animator.SetFloat(floatName, value);
        }
        else
        {
            Debug.LogWarning($"Animator float parameter name is empty in {animator.gameObject.name}'s SetFloatBehaviour.");
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateEnter)
        {
            SetAnimatorFloat(animator, valueOnEnter);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateExit)
        {
            SetAnimatorFloat(animator, valueOnExit);
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineEnter)
        {
            SetAnimatorFloat(animator, valueOnEnter);
        }
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachineExit)
        {
            SetAnimatorFloat(animator, valueOnExit);
        }
    }
}
