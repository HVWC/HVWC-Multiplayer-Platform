// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;

public class TextureSwapper : MonoBehaviour {

    public Texture2D[] textures;
    int index = 0;

    Material material;

    void Start() {
        material = GetComponent<Renderer>().material;
        SetTexture(index);
    }

    public void PreviousTexture() {
        index--;
        if(index <= 0) {
            index = textures.Length;
        }
        SetTexture(index);
    }

    public void NextTexture() {
        index++;
        if(index >= textures.Length) {
            index = 0;
        }
        SetTexture(index);
    }

    public void SetTexture(int i) {
        if(i < 0 || i > textures.Length) {
            Debug.LogWarning("Can't set texture: Index "+ i +" does not exist");
            return;
        }
        material.mainTexture = textures[i];
    }
	
}
