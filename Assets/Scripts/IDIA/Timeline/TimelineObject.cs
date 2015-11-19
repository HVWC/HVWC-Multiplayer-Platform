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
