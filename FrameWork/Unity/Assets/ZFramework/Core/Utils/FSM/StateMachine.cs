using System;
using System.Collections.Generic;

namespace ZFramework.FSM
{
    public class StateMachine <T>
    {
        #region Public Variable
        public BaseState<T> m_curState ;
        protected Dictionary<Type, BaseState<T>> _states = new Dictionary<Type, BaseState<T>>();
        #endregion

        #region Protected Variable
        protected bool m_isFinisInit = false;
        #endregion

        #region Main Methods     
        public void InitDefaultState(BaseState<T> _DefaultState)
        {
            m_curState = _DefaultState;
            m_curState.Enter();
            m_isFinisInit = true;
        }
        public void Execute(float _DeltaTime)
        {
            if (!m_isFinisInit)
            {
                return;
            }
            if (m_curState != null)
            {
                m_curState.Execute(_DeltaTime);
            }
        }
        #endregion

        #region Utility Methods
        public U AddState<U>() where U : BaseState<T>
        {
            var type = typeof(U);
            var ins = Activator.CreateInstance(type, this) as U;
            _states.Add(type, ins);
            return ins;
        }    

        public void ChangeTo(BaseState<T> _State)
        {
            if (m_curState != null)
            {
                m_curState.Exit();
            }
            m_curState = _State;
            m_curState.Enter();
        }
        #endregion
    }
}