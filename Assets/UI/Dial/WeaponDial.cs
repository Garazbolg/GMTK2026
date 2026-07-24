using System.Collections.Generic;
using DevCore.ScriptableVariables;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDial : MonoBehaviour {
    public ScriptableFloat durationValue = null;
    public ScriptableFloat timeValue = null;
    public ScriptableFloat frequencyValue = null;
    
    
    [Space]
    public Image progressBar = null;
    public RectTransform hand = null;

    [Space]
    public RectTransform markerParent = null;
    public Image markerPrefab = null;
    

    private void OnEnable() {
        durationValue.onValueChanged += PrepareDial;
        frequencyValue.onValueChanged += PrepareDial;
        timeValue.onValueChanged += SetCurrentPercentage;
        PrepareDial();
    }

    private void OnDisable() {
        durationValue.onValueChanged -= PrepareDial;
        frequencyValue.onValueChanged -= PrepareDial;
        timeValue.onValueChanged -= SetCurrentPercentage;
    }


    private List<Image> _markers = new ();


    private void PrepareDial() {
        foreach (var marker in _markers) {
            Destroy(marker);
        } 
        _markers.Clear();

        var duration = Mathf.Max(durationValue.value, 0.001f);
        var frequency = Mathf.Max(frequencyValue.value, 0.001f);
        var period = 1f / frequency;
        
        int ammos = Mathf.FloorToInt(duration * frequency);
        float deltaAngle = (period / duration) * 360f;
        for (int i = 0; i < ammos; i++) {
            float angle = deltaAngle * (i + 1);
            var instance = Instantiate(markerPrefab, markerParent.position, Quaternion.AngleAxis(angle, Vector3.back), markerParent);
            _markers.Add(instance);
        }
        
        
        SetCurrentPercentage();
    }

    private void SetCurrentPercentage() {
        float time = timeValue.value; 
        float duration = durationValue.value;
        float t = time / duration;
        t = Mathf.Clamp01(t);

        hand.localRotation = Quaternion.AngleAxis(t * 360f, Vector3.forward);
        progressBar.fillAmount = t;
        
        
        int consumedAmmos = Mathf.FloorToInt((duration - time) * frequencyValue.value);
        consumedAmmos = Mathf.Min(consumedAmmos, _markers.Count);
        for (int i = 0; i < consumedAmmos; i++) {
            if (_markers[i].enabled) {
                _markers[i].enabled = false;
            }
        }
        
        for (int i = consumedAmmos; i < _markers.Count; i++) {
            if (!_markers[i].enabled) {
                _markers[i].enabled = true;
            }
        }
    }
}
