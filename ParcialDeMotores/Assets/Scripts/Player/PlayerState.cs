using UnityEngine;

namespace Player
{
    public class PlayerState : IState
    {
        protected readonly PlayerStateMachine StateMachine;
        protected readonly Player Player;
        protected float XInput;
        protected float YInput;
        protected float StateTimer;
        protected bool TriggerCalled;
        protected readonly string AnimBoolName;

        public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
        {
            Player = player;
            StateMachine = stateMachine;
            AnimBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            Player.Animator.SetBool(AnimBoolName, true);
        }

        public virtual void Update() { }

        public virtual void Exit()
        {
            Player.Animator.SetBool(AnimBoolName, false);
        }
        
        protected void HandleDirectionalStateChange()
        {
            if (Input.GetKey(KeyCode.W) && StateMachine.CurrentState != Player.MoveState)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (Input.GetKey(KeyCode.S) && StateMachine.CurrentState != Player.MoveBackwardsState)
            {
                StateMachine.ChangeState(Player.MoveBackwardsState);
            }
            else if (Input.GetKey(KeyCode.A) && StateMachine.CurrentState != Player.MoveLeftState)
            {
                StateMachine.ChangeState(Player.MoveLeftState);
            }
            else if (Input.GetKey(KeyCode.D) && StateMachine.CurrentState != Player.MoveRightState)
            {
                StateMachine.ChangeState(Player.MoveRightState);
            }
        }

        public void SetIdleState(Vector3 input)
        {
            if (input == Vector3.zero)
            {
                StateMachine.ChangeState(Player.IdleState);
                return;
            }
        }

        public virtual void AnimationFinishTrigger() { }
    }
}