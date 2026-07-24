using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using UnityEngine;

namespace DevCore.InputManagement
{
    [CreateAssetMenu(menuName = Constants.ASSET_PATH + "Input/Input Action")]
    public class ButtonInputAction : InputAction
    {
        public override bool IsStarted() {
            throw new NotImplementedException();
        }

        public override bool IsPerformed() {
            throw new NotImplementedException();
        }

        public override bool IsReleased() {
            throw new NotImplementedException();
        }

        public override bool IsHolding() {
            throw new NotImplementedException();
        }

        public override void RegisterStartAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void RegisterPerformAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void RegisterReleaseAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void RegisterHoldAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void UnregisterStartAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void UnregisterPerformAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void UnregisterReleaseAction(Action callback) {
            throw new NotImplementedException();
        }

        public override void UnregisterHoldAction(Action callback) {
            throw new NotImplementedException();
        }
    }
}
