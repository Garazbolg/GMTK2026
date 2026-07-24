using System.Collections.Generic;
using UnityEngine;

namespace DevCore.ApplicationStates {
    /// <summary>
    /// Allow using a state stack forcing some state or overrides
    /// </summary>
    public static class ApplicationStack {
        #region Constants
        private const int STATES_CAPACITY = 10;
        #endregion


        #region Currents
        private static ApplicationStateOverrides m_CurrentOverrides;
        internal static bool m_initialized = false;
        private static List<ApplicationState> m_CurrentStates = null;
        #endregion


        #region Properties
        public static ApplicationStateOverrides CurrentOverrides => m_CurrentOverrides;
        #endregion


        #region Events
        public delegate void StateAction(ApplicationStateOverrides lastOverrides);

        public static event StateAction onOverridesUpdated;
        #endregion


        #region Callbacks
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnRuntimeInit() {
            CheckInit();
        }

        internal static void Update() {
            for (int i = 0; i < m_CurrentStates.Count; i++) {
                m_CurrentStates[i].Update();
            }
        }
        #endregion


        #region Initialization
        private static void CheckInit() {
            if (!m_initialized) {
                Init();
            }
        }

        private static void Init() {
            m_CurrentOverrides = ApplicationStateOverrides.Default;
            m_initialized = true;
            m_CurrentStates = new List<ApplicationState>(STATES_CAPACITY);

            var go = new GameObject("[Application Stack]");
            go.AddComponent<ApplicationStackManager>();
        }
        #endregion


        #region States Management
        /// <summary>
        /// Returns the number of states in the stack
        /// </summary>
        /// <returns></returns>
        public static int GetStateCount() {
            return m_CurrentStates.Count;
        }

        public static ApplicationState GetStateByIndex(int index) {
            return m_CurrentStates[index];
        }

        /// <summary>
        /// Returns the most prior state of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetState<T>() where T : ApplicationState {
            for (int i = 0; i < m_CurrentStates.Count; i++) {
                if (m_CurrentStates[i] is T castedState) {
                    return castedState;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns an array of all current states in priority order
        /// </summary>
        /// <returns></returns>
        public static ApplicationState[] GetStates() {
            var states = new ApplicationState[m_CurrentStates.Count];
            for (int i = 0; i < states.Length; i++) {
                states[i] = m_CurrentStates[i];
            }

            return states;
        }

        /// <summary>
        /// Returns an int with the current states count and fill the array with current states
        /// </summary>
        /// <returns></returns>
        public static int GetStatesNonAlloc(ApplicationState[] states) {
            int stateCount = m_CurrentStates.Count;
            for (int i = 0; i < m_CurrentStates.Count; i++) {
                states[i] = m_CurrentStates[i];
            }

            return stateCount;
        }

        /// <summary>
        /// Returns an array of all current states in priority order
        /// </summary>
        /// <returns></returns>
        public static void GetStatesNonAlloc(List<ApplicationState> states) {
            for (int i = 0; i < m_CurrentStates.Count && i < states.Count; i++) {
                states[i] = m_CurrentStates[i];
            }

            if (states.Count < m_CurrentStates.Count) {
                for (int i = states.Count; i < m_CurrentStates.Count; i++) {
                    states.Add(m_CurrentStates[i]);
                }
            } else if (states.Count > m_CurrentStates.Count) {
                for (int i = states.Count - 1; i >= m_CurrentStates.Count; i--) {
                    states.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Return true and out the first state of type T 
        /// </summary>
        /// <param name="state"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetState<T>(out T state) where T : ApplicationState {
            for (int i = 0; i < m_CurrentStates.Count; i++) {
                if (m_CurrentStates[i] is T castedState) {
                    state = castedState;
                    return true;
                }
            }

            state = null;
            return false;
        }

        /// <summary>
        /// Returns true on the first found state of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasState<T>() where T : ApplicationState {
            for (int i = 0; i < m_CurrentStates.Count; i++) {
                if (m_CurrentStates[i] is T) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add a state on stack and applies its default setting
        /// If the state must be unique, returns the existing state if one exists in the stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T AddState<T>() where T : ApplicationState, new() {
            CheckInit();
            var state = new T();
            state.m_Name = typeof(T).Name;

            if (state is IApplicationStateWithSetting stateWithSetting) {
                stateWithSetting.SetDefaultSetting();
            }

            if (CheckUniqueness(state, out T existingState)) {
                return existingState;
            } else {
                AddStateOnStack(state);
                return state;
            }
        }

        
        /// <summary>
        /// Add a state on stack with an input setting
        /// If the state must be unique, returns the existing state if one exists in the stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TState AddStateWithSetting<TState, TSetting>(TSetting setting)
            where TState : ApplicationStateWithSetting<TSetting>, new() where TSetting : ApplicationStateSetting, new() {
            CheckInit();
            var state = new TState();
            state.m_Name = typeof(TState).Name;
            state.m_settings = setting;

            if (CheckUniqueness(state, out TState existingState)) {
                return existingState;
            } else {
                AddStateOnStack(state);
                return state;
            }
        }

        /// <summary>
        /// Add a state on the stack depending its priority
        /// </summary>
        /// <param name="state"></param>
        private static void AddStateOnStack(ApplicationState state) {
            if (m_CurrentStates.Count == 0) {
                m_CurrentStates.Add(state);
            } else {
                int inputStatePriority = state.priority;
                if (m_CurrentStates[m_CurrentStates.Count - 1].priority > inputStatePriority) {
                    m_CurrentStates.Add(state);
                } else {
                    if (inputStatePriority >= m_CurrentStates[0].priority) {
                        var priorState = m_CurrentStates[0];
                        if (priorState.startPauseWaitForInitialization && !priorState.initialized) {
                            priorState.paused = true;
                        } else {
                            priorState.PauseState();
                        }
                        m_CurrentStates.Insert(0, state);
                    } else {
                        for (int i = 1; i < m_CurrentStates.Count; i++) {
                            if (inputStatePriority >= m_CurrentStates[i].priority) {
                                m_CurrentStates.Insert(i, state);
                                state.paused = true;
                                break;
                            }
                        }
                    }
                }
            }

            //Initialize state
            state.Initialize();
            UpdateOverrides();
        }


        /// <summary>
        /// Remove the input state from the stack
        /// </summary>
        internal static void RemoveState(ApplicationState state) {
            if (m_CurrentStates.Count > 1) {
                if (m_CurrentStates[0] == state) {
                    m_CurrentStates.RemoveAt(0);
                    var priorState = m_CurrentStates[0]; 
                    if (!priorState.initialized && priorState.startPauseWaitForInitialization) {
                        priorState.paused = false;
                    } else {
                        priorState.StartOrResumeState();
                    }
                } else {
                    for (int i = 1; i < m_CurrentStates.Count; i++) {
                        if (m_CurrentStates[i] == state) {
                            m_CurrentStates.RemoveAt(i);
                        }
                    }
                }
            } else {
                m_CurrentStates.Clear();
            }

            UpdateOverrides();
        }

        /// <summary>
        /// Returns true if the input state must be unique and a state with the same type already exists
        /// </summary>
        /// <param name="state"></param>
        /// <param name="foundState"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool CheckUniqueness<T>(T state, out T foundState) where T : ApplicationState {
            if (state.mustBeUnique) {
                return TryGetState(out foundState);
            }

            foundState = null;
            return false;
        }
        #endregion


        #region Overrides
        private static void UpdateOverrides() {
            m_CurrentOverrides = ApplicationStateOverrides.Default;
            for (int i = m_CurrentStates.Count - 1; i >= 0; i--) {
                m_CurrentStates[i].ApplyOverrides(ref m_CurrentOverrides);
            }

            onOverridesUpdated?.Invoke(m_CurrentOverrides);
        }
        #endregion
    }
}