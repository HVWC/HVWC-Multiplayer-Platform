// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;

public class AnimationController : MonoBehaviour{
	
	private Animator animator;
	private string[] animations = new string[]{"idle", "walk", "run", "jump", "sit", "fly"};
	public string[] Animations {
		get {
			return animations;
		}
		set {
			animations = value;
		}
	}
	private string currentAnimation;
	public string CurrentAnimation {
		get {
			return currentAnimation;
		}
		set {
			currentAnimation = value;
		}
	}

	void Start(){
		animator = GetComponent<Animator>();
	}

	public void ActivateAnimation(string animationToActivate){
		string target = "";
		foreach(string a in animations){
			if(a==animationToActivate){
				target = a;	
			}else{
				animator.SetBool(a,false);	
			}
		}
		animator.SetBool(target,true);
		currentAnimation = target;
	}
	
	public void ActivateAnimation(int index){
		for(int i=0;i<animations.Length;i++){
			if(i!=index){
				animator.SetBool(animations[i],false);
			}
		}
		animator.SetBool(animations[index],true);
		currentAnimation = animations[index];
	}
	
}