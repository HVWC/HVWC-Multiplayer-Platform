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
