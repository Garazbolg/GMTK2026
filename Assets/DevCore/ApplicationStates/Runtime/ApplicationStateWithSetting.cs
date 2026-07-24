using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.ApplicationStates {
    /// <summary>
    /// A base application state that can take settings has inputs values
    /// </summary>
    public abstract class ApplicationStateWithSetting<T> : ApplicationState, IApplicationStateWithSetting where T : ApplicationStateSetting, new() {
        internal T m_settings = null;

        public T settings => m_settings;

        public void SetDefaultSetting() {
            m_settings = new T();
        }
    }

    /// <summary>
    /// An interface to apply default setting on each application state instanced
    /// </summary>
    public interface IApplicationStateWithSetting {
        public void SetDefaultSetting();
    }
}