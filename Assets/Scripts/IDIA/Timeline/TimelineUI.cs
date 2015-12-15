// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
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
