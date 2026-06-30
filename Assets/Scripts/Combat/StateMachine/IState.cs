using System;

namespace Combat.StateMachine
{
    public interface IState
    {
        // 根据概率选择下一个状态
        IState GetNextState();

        void SetNextState(IState nextState);

        void OnEnter();

        void OnExit();
    }

    public static class StateExtensions
    {
        public static IState MoveToNextState(this IState state)
        {
            IState nextState = state.GetNextState();
            state.OnExit();
            if (nextState != null)
            {
                nextState.OnEnter();
                return nextState;
            }
            throw new InvalidOperationException("下个状态不能为空");
        }
    }
}