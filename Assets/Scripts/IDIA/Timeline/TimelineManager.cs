using UnityEngine;

public class TimelineManager : MonoBehaviour {

    public delegate void ChangedTime(float time);
    public static event ChangedTime OnChangedTime;

    public float defaultTime; 
    float time;
    
    void Start() {
        time = defaultTime;
        SetTimeline(time);
    }

    public void SetTimeline(float newTime) {
        time = newTime;
        if (OnChangedTime!=null) {
            OnChangedTime(newTime);
        }
    }

}
