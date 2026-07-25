using DevCore.FeedbackEngine;
using UnityEngine;

public class LaunchFeedbackOnEnable : MonoBehaviour {
    public FeedbackAsset feedback = null;

    private void OnEnable() {
        feedback?.Play(gameObject);
    }
}
