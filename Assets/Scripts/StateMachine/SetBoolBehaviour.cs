using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{   
    public string boolName;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;

    private void SetAnimatorBool(Animator animator, bool value)
    {
        if (!string.IsNullOrEmpty(boolName))
        {
            animator.SetBool(boolName, value);
        }
        else
        {
            Debug.LogWarning($"Animator boolean name is empty in {animator.gameObject.name}'s SetBoolBehaviour.");
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            SetAnimatorBool(animator, valueOnEnter);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            SetAnimatorBool(animator, valueOnExit);
        }
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
        {
            SetAnimatorBool(animator, valueOnEnter);
        }
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
        {
            SetAnimatorBool(animator, valueOnExit);
        }
    }
}
