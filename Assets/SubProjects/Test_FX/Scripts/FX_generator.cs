using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FX_generator : MonoBehaviour {

	private List<QuadTransitionFX> quadList ;
	private int _numFX= 20;
	
	void Start () {
		// we only need to load our reference Prefab once
		// the same quad will then be instanced many times
		GameObject fxRef = Resources.Load ("QuadTransitionFX") as GameObject;

		// we create a list to store our FXs
		quadList = new List<QuadTransitionFX> ();
		for (int i = 0; i < _numFX; ++i) {
			// for each FX we provide the prefab to clone and a random position 
			QuadTransitionFX newFX = new QuadTransitionFX( fxRef, new Vector3( Random.value*20-10,Random.value*10-5,Random.value*20));
			// each FX is stored in the list
			quadList.Add(newFX);
		}
	}

	void Update () {
		// on every frame we update our QuadTransitionFXs
		for (int i = 0; i < _numFX; ++i) {
			quadList[i].Update();
		}
	}
}
