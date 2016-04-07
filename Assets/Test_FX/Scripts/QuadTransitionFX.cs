using UnityEngine;
using System.Collections;

public class QuadTransitionFX {

	private GameObject _go;
	private Renderer _renderer;
	public Vector3 position;
	private MaterialPropertyBlock _mpb;

	protected float FadeInDuration;
	protected float FadeOffsetDuration;
	protected float FadeOutDuration;
	protected float FadeTransitionDuration;

	protected float LifeTime;
	private Vector4 _rampUv;
	private float _rampUwidth;
	private float _rampUmin;
	private float _rampVheight;
	private float _rampVmin;
	private Color _currentRampColor;

	private static int _RampUV_ID = Shader.PropertyToID("_RampUV");
	private static int _LifeColor_ID = Shader.PropertyToID("_LifeColor");

	public QuadTransitionFX ( GameObject proto, Vector3 pos )
	{
		// we instantiate our Quad
		// ... or you could generate your own geometry around here.
		_go = GameObject.Instantiate<GameObject>(proto);

		_renderer = _go.GetComponent<Renderer> ();
		position = pos;
		_go.transform.position = position;
		_mpb = new MaterialPropertyBlock ();

		// These values should reflect the position and size of our FX on its atlas
		// It will be transmitted to the shader through the MaterialPropertyBlock
		// even if we're using 'individual' textures.
		_rampUmin = 0;
		_rampUwidth = 1 ;
		_rampVmin = 0;
		_rampVheight = 1;

		// time in seconds for the FX to complete its appearance animation
		FadeInDuration = 0.4f;
		// time in seconds for the FX to stay static at the end of its appearance before it starts to disappear
		// please note that this can be NEGATIVE.
		// And yes it can be very usefull to start the disappearance animation before the end of the appearance!
		FadeOffsetDuration = 0f;
		// time in seconds for the FX to complete its disappearance animation
		FadeOutDuration = 1f;
		// time in seconds CENTERED around the middle of the FadeOffsetDuration
		// where the gradient of the appearance blend into the gradient of the disappearance
		// this is to avoid a hiccup between the 2 phases.
		FadeTransitionDuration =0.2f;

		LifeTime = Random.value * (FadeInDuration + FadeOutDuration + FadeOffsetDuration) ;
		_currentRampColor = new Color(0, 1, 1, 1);

		updateLife ();
	}

	private void updateLife ()
	{
		float fadeOutStart = FadeInDuration + FadeOffsetDuration;

		// How much of the appearance is done ? 0 -> 1
		float FadeInRatio = LifeTime > FadeInDuration ? 1 : LifeTime / FadeInDuration;

		// How much of the disappearance is done ? 0 -> 1
		float FadeOutRatio;
		if (LifeTime < fadeOutStart)
			FadeOutRatio = 0;
		else {
			FadeOutRatio = (LifeTime - fadeOutStart) / FadeOutDuration ;
		}

		// How much of the blending between the 2 phases is done ? 0 -> 1
		float FadeTransitionStart = FadeInDuration + FadeOffsetDuration * 0.5f;
		float TransitionRatio = Mathf.Clamp ((LifeTime - FadeTransitionStart) / FadeTransitionDuration, 0f, 1f);

		// We store the results in color
		_currentRampColor.r = FadeInRatio;
		_currentRampColor.g = FadeOutRatio;
		_currentRampColor.b = TransitionRatio;

		// We store the UV informations in a Vector4
		// These are actually usefull if your FXs are packed into Atlases
		_rampUv = new Vector4(_rampUmin, _rampUwidth, _rampVmin, _rampVheight);

		// you MUST clear a MaterialPropertyBlock before you assign new values!
		// If you don't : goodbye framerate and hello memory leak !
		_mpb.Clear ();
		// It's faster to use int than string to identify shader properties
		_mpb.AddVector(_RampUV_ID, _rampUv);
		_mpb.AddVector (_LifeColor_ID, _currentRampColor);
		// Assign the new property block to the Renderer !
		_renderer.SetPropertyBlock (_mpb);
		
	}
	
	public void Update()
	{
		LifeTime += Time.deltaTime;
		// We just make looping particles here...
		if (LifeTime > FadeInDuration + FadeOutDuration + FadeOffsetDuration) {
			LifeTime = 0;
		}
		updateLife ();
	}
	
}
