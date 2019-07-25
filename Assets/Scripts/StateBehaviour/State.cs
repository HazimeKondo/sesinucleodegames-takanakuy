using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateBehaviour
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }

    [SerializeField]
    public abstract class State : IState
    {
        protected StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine;

        public State(StateMachine stateMachine)
        {
            this._stateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }

    public abstract class State<T> : State where T : MonoBehaviour
    {
        public new StateMachine<T> StateMachine => this._stateMachine as StateMachine<T>;

        public State(StateMachine stateMachine) : base(stateMachine)
        {
        }
    }
}