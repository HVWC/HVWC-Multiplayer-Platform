using UnityEngine;
using System;
using System.Collections;

public class CustomAvatarAnimation : MonoBehaviour
{
	#region Fields
	/// <summary>
	///  An instance of the Animator component on the player.
	/// </summary>
	Animator animator;

	Vector3 lastPos, currentPos;
	
	/// <summary>
	///  An array of the names of the animations we have on our player.
	/// </summary>
	string[] animations = new string[]{"idle", "walk", "run", "jump", "sit", "fly"};
	
	/// <summary>
	///  The currently playing animation.
	/// </summary>
	string currentAnimation;
	#endregion
	
	#region Properties
	/// <summary>
	///  A property to get/set the animations we have on our player.
	/// </summary>
	public string[] Animations {
		get {
			return animations;
		}
		set {
			animations = value;
		}
	}
	
	/// <summary>
	///  A property to get/set the currently playing animation.
	/// </summary>
	public string CurrentAnimation {
		get {
			return currentAnimation;
		}
		private set {
			currentAnimation = value;
		}
	}
	#endregion
	
	#region Unity Messages
	/// <summary>
	/// A message called when the script is enabled just before Update is called.
	/// </summary>
	void Start(){
		animator = GetComponent<Animator>(); //Get the Animator component
	}
	#endregion
	
	#region Methods
	/// <summary>
	///  A method to set activate an animation on our player.
	/// </summary>
	/// <param name="animationToActivate">
	/// The name of the animation to activate.
	/// </param>
	public void ActivateAnimation(string animationToActivate){
		string target = "";
		foreach(string a in Animations){ //First, deactivate every animation on our player, except for the one we'd like to activate
			if(a==animationToActivate){
				target = a;	
			}else{
				animator.SetBool(a,false);	
			}
		}
		animator.SetBool(target,true); //Then activate our desired animation
		CurrentAnimation = target;
	}
	
	/// <summary>
	///  A method to set activate an animation on our player.
	/// </summary>
	/// <param name="index">
	/// The index of the animation to activate.
	/// </param>
	public void ActivateAnimation(int index){
		for(int i=0;i<Animations.Length;i++){ //First, deactivate every animation on our player, except for the one we'd like to activate
			if(i!=index){
				animator.SetBool(Animations[i],false);
			}
		}
		animator.SetBool(Animations[index],true); //Then activate our desired animation
		CurrentAnimation = Animations[index];
	}
	
	void OnEnable()
	{
		lastPos = currentPos = transform.position;
		
		/*if(_animation == null)
		{
			_animation = GetComponent<Animation>();
			if(_animation == null)
			{
				_animation = GetComponentInChildren<Animation>();
				if(_animation == null)
				{
					enabled = false;
					return;
				}
			}
		}*/
	}
	
	public void SetPosition(Vector3 position)
	{
		lastPos = currentPos = transform.position = position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentPos = transform.position;
		Vector3 diff = currentPos - lastPos;
		diff.y = 0.0f;
		float speed = diff.magnitude / Time.deltaTime;
		lastPos = currentPos;
		
		//Update animation
		if(speed < 0.1f)
		{
			ActivateAnimation("idle");
		}
		else if (speed < 2.0f)
		{
			//animator["walk"].speed = speed;
			ActivateAnimation("walk");
		}
		else
		{
			//_animation["run"].speed = speed/3.0f;
			ActivateAnimation("run");
		}
		
		/*if (lastAnimation != currentAnimation)
		{
			lastAnimation = currentAnimation;
			_animation.CrossFade(currentAnimation.ToString());
		}*/
		
		if (transform.position.y < -20.0f)
		{
			Vector3 pos = new Vector3(0.0f, 100000.0f, 0.0f);
			
			RaycastHit hit;
			
			if (Physics.Raycast(pos, Vector3.down, out hit))
				transform.position = hit.point;
		}
	}
	#endregion
}
