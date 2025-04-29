using UnityEngine;

namespace Combat.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public IState initialState; // 初始状态
        private IState currentState; // 当前状态

        public IState CurrentState => currentState; // 当前状态的只读属性

        void Start()
        {
            if (initialState != null)
            {
                currentState = initialState;
                currentState.SetStateMachine(this);
                currentState.OnEnter();
            }
        }

        public void Init(IState initState) {
            initialState = initState;
            currentState = initialState;
            currentState.SetStateMachine(this);
            currentState.OnEnter();
        }

        void Update()
        {
            if (currentState != null)
                currentState.OnUpdate();
        }

        // 切换到下一个状态
        public void TransitionToNextState()
        {
            IState nextState = currentState.GetNextState();
            if (nextState == null) return;

            currentState.OnExit();
            currentState = nextState;
            currentState.SetStateMachine(this);
            currentState.OnEnter();
        }
    }
}