using UnityEngine;
using UnityEngine.UI;

public class TimelineUI : MonoBehaviour {

    public Slider temporalSlider;
    public InputField temporalInput;

    void Start() {
        SetTemporalInputText(temporalSlider.value);
    }

    public void SetTemporalInputText(float time) {
        temporalInput.text = ((int)time).ToString();
    }

    public void SetTemporalSliderValue(string timeString) {
        float time;
        float.TryParse(timeString, out time);
        time = Mathf.Clamp(time, temporalSlider.minValue, temporalSlider.maxValue);
        temporalSlider.value = time;
    }

}
