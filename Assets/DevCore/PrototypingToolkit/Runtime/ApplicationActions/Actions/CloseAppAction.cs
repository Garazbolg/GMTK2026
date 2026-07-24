using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("Quit Application")]
    public class CloseAppAction : ApplicationActionComponent
    {
        protected override void Execute() {
            AppCore.Quit();
        }
    }
}
