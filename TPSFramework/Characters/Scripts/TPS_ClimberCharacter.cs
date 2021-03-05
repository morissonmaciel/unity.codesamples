using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Third Person Framework/Characters/Traversal/Climber")]
public class TPS_ClimberCharacter : MonoBehaviour
{
    public Animator PlayerAnimator;
    public RuntimeAnimatorController ClimbingAnimationController;

    private bool IsClimbing;

    public void SetIsClimbing(bool isClimbing, TPS_Climbable target)
    {
        this.IsClimbing = true;
    }

    private RuntimeAnimatorController previousAnimator;

    void Start()
    {
        if (PlayerAnimator != null)
            previousAnimator = PlayerAnimator.runtimeAnimatorController;
    }

    void Update()
    {
        if (PlayerAnimator == null && ClimbingAnimationController == null)
            return;

        if (IsClimbing)
        {
            PlayerAnimator.runtimeAnimatorController = ClimbingAnimationController;
        }
    }
}
