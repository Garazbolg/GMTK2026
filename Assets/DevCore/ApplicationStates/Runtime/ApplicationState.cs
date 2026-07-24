using System.Collections;
using UnityEngine;

namespace DevCore.ApplicationStates {
    /// <summary>
    /// Apply a behaviour once this one is added or removed of the Application Stack. 
    /// Could also override values with <see cref="OnApplyOverrides"/> method
    /// </summary>
    public abstract class ApplicationState {
        #region Properties
        /// <summary>State execution group, see <see cref="StateCategory" execution order/></summary>
        public virtual StateCategory category => StateCategory.None;

        /// <summary>Add a priority offset to the final priority result</summary>
        public virtual int priorityOffset => 0;

        /// <summary>State final execution priority</summary>
        public int priority => (int)category + priorityOffset;

        /// <summary>Could the state being the only one to exist in the stack</summary>
        public virtual bool mustBeUnique => false;

        /// <summary>Does the state update must be called even if this one is not prior</summary>
        public virtual bool updateIfPaused => false;

        /// <summary>Does start and pause must wait for initization completion to be called</summary>
        public virtual bool startPauseWaitForInitialization => false;

        public string name => m_Name;

        /// <summary>Does the state initialization is complete</summary>
        public bool initialized => m_IsInitialized;

        /// <summary>Is the state paused, trus once the state priotirty is lost</summary>
        public bool paused {
            get => m_IsPaused;
            internal set { m_IsPaused = value; }
        }

        /// <summary>Is the state ended and not executed anymore</summary>
        public bool ended => m_IsPaused;

        /// <summary>Retuns true if the state update is called</summary>
        public bool isUpdating => m_canUpdate;
        #endregion


        #region Contruction
        protected ApplicationState() {
        }
        #endregion


        #region Currents
        internal string m_Name = string.Empty;
        private bool m_IsPaused = false;
        private bool m_IsEnded = false;
        private bool m_IsInitialized = false;
        internal Coroutine m_CurrentInitCoroutine = null;
        private bool m_canUpdate = false;
        #endregion


        #region Overrides
        internal void ApplyOverrides(ref ApplicationStateOverrides overrides) {
            OnApplyOverrides(ref overrides);
        }

        protected virtual void OnApplyOverrides(ref ApplicationStateOverrides overrides) {
        }
        #endregion


        #region External Callbacks
        internal void StartOrResumeState() {
            m_IsPaused = false;

            if (m_canUpdate == false) {
                m_canUpdate = true;
            }

            OnStartOrResume();
        }

        internal void PauseState() {
            m_IsPaused = true;

            if (!updateIfPaused) {
                m_canUpdate = false;
            }

            OnStatePaused();
        }

        public void EndState() {
            if (m_IsEnded) return;

            ApplicationStack.RemoveState(this);

            //Stop initialization if this one is not complete before state end
            if (!m_IsInitialized) {
                ApplicationStackManager.m_Instance.StopCoroutine(m_CurrentInitCoroutine);
            }

            m_IsEnded = true;
            m_canUpdate = false;
            m_IsInitialized = false;
            OnStateEnded();
        }

        internal void Initialize() {
            OnStateAddedOnStack();
            if (!startPauseWaitForInitialization) {
                CompleteStateInitialization();
            }

            m_CurrentInitCoroutine = ApplicationStackManager.m_Instance.StartCoroutine(Initialize_Internal());
        }

        private IEnumerator Initialize_Internal() {
            yield return OnStateInitializing();

            m_CurrentInitCoroutine = null;
            m_IsInitialized = true;

            if (startPauseWaitForInitialization) {
                CompleteStateInitialization();
            }
        }

        private void CompleteStateInitialization() {
            if (m_IsPaused) {
                if (updateIfPaused && !m_canUpdate) {
                    m_canUpdate = true;
                }
                PauseState();
            } else {
                StartOrResumeState();
            }
        }

        internal void Update() {
            if (m_canUpdate) {
                OnStateUpdate();
            }
        }
        #endregion


        #region Internal Callbacks
        /// <summary>
        /// Called once the state is added on the stack, after AddState() or AddStateWithSetting()
        /// </summary>
        protected virtual void OnStateAddedOnStack() {
        }

        /// <summary>
        /// Called after the state initialization (on the next frame of the AddState if <see cref="startPauseWaitForInitialization"/>)
        /// is set to true. 
        /// Also called once the state becomes prior in the stack
        /// </summary>
        protected virtual void OnStartOrResume() {
        }


        /// <summary>
        /// Called when a state is added over this one if it was prior
        /// </summary>
        protected virtual void OnStatePaused() {
        }

        /// <summary>
        /// Called after EndState, could be called only once, it is recommended to dispose the state instance
        /// after calling this method
        /// </summary>
        protected virtual void OnStateEnded() {
        }

        /// <summary>
        /// Called every frame while the state is initialized and not paused (except if the state ignore pause)
        /// </summary>
        protected virtual void OnStateUpdate() {
        }

        /// <summary>
        /// Called on the next frame when the state is added on the stack
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator OnStateInitializing() {
            yield break;
        }
        #endregion
    }

    /// <summary>
    /// Defines the group and the priority of each state in this order
    /// Debug (300) -> Application (200) -> Gameplay (100) -> None (0)
    /// </summary>
    public enum StateCategory {
        None = 0,
        Gameplay = 100,
        Application = 200,
        Debug = 300
    }
}