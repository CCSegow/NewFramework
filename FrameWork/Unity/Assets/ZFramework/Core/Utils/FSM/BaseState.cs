using System.Collections.Generic;

namespace ZFramework.FSM
{
    public abstract class BaseState <T>
    {
        #region Public Variable
        #endregion

        #region Protected Variable
        protected T m_stateMachine;
        protected int m_stateID;

        protected List<StateTransition> m_stateTransitions;
        #endregion

        #region MainFunction
        public BaseState(T stateMachine)
        {
            m_stateMachine = stateMachine;
            m_stateTransitions = new List<StateTransition>();
        }
        public abstract void Enter();
        public void Execute(float deltaTime)
        {
            for (int i = 0; i < m_stateTransitions.Count; i++)
            {
                var transition = m_stateTransitions[i];
                if (transition.m_condition())
                {
                    (m_stateMachine as StateMachine<T>).ChangeTo(transition.m_targetState);
                    return;
                }
            }
            OnExecute(deltaTime);
        }
        protected virtual void OnExecute(float deltaTime)
        {

        }

        public virtual void Exit() { }

        public delegate bool Condition();

        protected class StateTransition
        {
            public BaseState<T> m_targetState;
            public Condition m_condition;
        }
        public void AddTransition(BaseState<T> state, Condition condition)
        {
            m_stateTransitions.Add(new StateTransition { m_targetState = state, m_condition = condition });
        }
        #endregion

        #region Utility Methods
        #endregion
    }
}