// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.Events;

public class TimelineObject : MonoBehaviour {

    public TimelineRange[] ranges;
    //public float minTime,maxTime; //inclusive
    public UnityEvent OnEnteredTime;
    public UnityEvent OnExitedTime;

    float time;
    bool inTime;

    void Awake() {
        TimelineManager.OnChangedTime += OnChangedTime;
    }

    void Start() {
        if (InRange(time)) {
            OnEnteredTime.Invoke();
            inTime = true;
        }
        if (InRange(time)) {
            OnExitedTime.Invoke();
            inTime = false;
        }
    }

    void Update() {
        if(InRange(time) && !inTime) {
            OnEnteredTime.Invoke();
            inTime = true;
        }
        if (!InRange(time) && inTime) {
            OnExitedTime.Invoke();
            inTime = false;
        }
    }

    private void OnChangedTime(float t) {
        time = t;
    }

    bool InRange(float t) {
        foreach (TimelineRange range in ranges) {
            if (time >= range.minTime && time <= range.maxTime) {
                return true;
            }
        }
        return false;
    }
}
