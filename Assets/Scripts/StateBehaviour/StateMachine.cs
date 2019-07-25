using UnityEngine;

namespace StateBehaviour
{
    public interface IStateMachine
    {
        void UpdateStateMachine();
        void ChangeState(State newState);
    }

    [SerializeField]
    public abstract class StateMachine : IStateMachine
    {
        protected State _state;
        public State State => _state;

        protected MonoBehaviour _actor;
       // public MonoBehaviour Actor => _actor;

        public StateMachine(MonoBehaviour actor)
        {
            this._actor = actor;
        }

        public void ChangeState(State newState)
        {
            _state?.Exit();
            _state = newState;
            _state?.Enter();
        }

        public void UpdateStateMachine()
        {
            _state?.Execute();
        }

        public T GetActor<T>() where T : MonoBehaviour
        {
            return _actor as T;
        }
    }

    public abstract class StateMachine<T> : StateMachine where T : MonoBehaviour
    {
        public new State<T> State => _state as State<T>;
        
        public StateMachine(MonoBehaviour actor) : base(actor)
        {
        }

        public T GetActor()
        {
            return _actor as T;
        }
    }
}