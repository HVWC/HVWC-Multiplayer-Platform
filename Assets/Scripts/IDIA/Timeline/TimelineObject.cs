using UnityEngine;
using UnityEngine.Events;

public class TimelineObject : MonoBehaviour {

    public float minTime,maxTime; //inclusive
    public UnityEvent OnEnteredTime;
    public UnityEvent OnExitedTime;

    float time;
    bool inTime;

    void Awake() {
        TimelineManager.OnChangedTime += OnChangedTime;
    }

    void Start() {
        if (time >= minTime && time <= maxTime) {
            OnEnteredTime.Invoke();
            inTime = true;
        }
        if ((time < minTime || time > maxTime)) {
            OnExitedTime.Invoke();
            inTime = false;
        }
    }

    void Update() {
        if(time>=minTime && time <= maxTime && !inTime) {
            OnEnteredTime.Invoke();
            inTime = true;
        }
        if ((time < minTime || time > maxTime) && inTime) {
            OnExitedTime.Invoke();
            inTime = false;
        }
    }

    private void OnChangedTime(float t) {
        time = t;
    }
}
