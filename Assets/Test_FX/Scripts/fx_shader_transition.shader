Shader "AlkemiShaders/fx_shader_transition" {
	Properties {
		_Emission ("Emission", Float) = 3
		_Color ("Main Color", Color) = (1,1,1,1)
        _RampMap ("Ramp Map", 2D) = "white" {}
		_EffectMap ("Effect Map", 2D) = "white" {}
		_RampUV ( "RampUV", Vector) = (0,0,0,0)
		_LifeColor ("Life Color", Color) = (1,1,1,1)
		
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		Cull Off
		LOD 400
	CGPROGRAM
	#pragma surface surf Unlit alpha noambient nolightmap nodirlightmap novertexlights noforwardadd noshadow
	#pragma target 3.0 
	
	// No lighting needed
    half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
        half4 c;
        c.rgb = s.Albedo;
        c.a = s.Alpha;
        return c;
    }
    
	struct Input {
		float2 uv_EffectMap;
		float4 color : COLOR;
	};
	
	float4 _Color;
	sampler2D _RampMap;
	sampler2D _EffectMap;
	float4 _RampUV;
	float4 _LifeColor;
	float _Emission;
	
	void surf (Input IN, inout SurfaceOutput o) {
		
		// Fetch the correct pixel for the current fragment on the animation texture
		half4 texEffect = tex2D(_EffectMap, IN.uv_EffectMap);
		
		// _LifeColor is passed through the MaterialPropertyBlock by QuadTransitionFX
		// r stores the fade in ratio
		// g stores the fade out ratio
		// b stores the blend ratio
		
		// fadeInFactor is based on the fade in ratio and the R channel of the animation texture
    	half fadeInFactor = saturate(_LifeColor.r - (1 - texEffect.r));
    	// fadeOutFactor is based on the fade out ratio and the G channel of the animation texture
    	half fadeOutFactor = saturate((1-_LifeColor.g) - (1 - texEffect.g));
    	
    	// _RampUV is passed through the MaterialPropertyBlock by QuadTransitionFX
    	// it stores the position of our 2D color ramp on an Atlas
    	
    	// Here we determine what are the correct UVs to sample the 2D ramp for the appearance phase
    	// based on fade in ratio for U (time). Remember that the fade in phase take the left half of the 2D ramp.
    	// and the FadeInFactor for V (how much is this part of the texture actually visible)
    	float2 rampUV = half2(_RampUV[0]+ _LifeColor.r * 0.5f * _RampUV[1], _RampUV[2] + fadeInFactor * _RampUV[3]);
        // We store the color for this fragment linked to the appearance phase
        half4 texIN = tex2D(_RampMap, rampUV);
        
        // Here we determine what are the correct UVs to sample the 2D ramp for the appearance phase
        // based on fade out ratio for U (time). Remember that the fade out phase take the right half of the 2D ramp.
    	// and the FadeOutFactor for V or actually more 1-fadeOutFactor (how much is this part of the texture actually visible)
        rampUV = half2(_RampUV[0]+ (0.5f + _LifeColor.g * 0.5f) * _RampUV[1], _RampUV[2] + (1-fadeOutFactor) * _RampUV[3]);
        // We store the color for this fragment linked to the disappearance phase
        half4 texOUT= tex2D(_RampMap, rampUV);
        
        // We blend the previous results to avoid a hiccup between fade in and fade out
        half4 texBLEND = lerp(texIN, texOUT, _LifeColor.b);
        
        // We multiply the resulting color by a material color and by an emission factor
        // note that this emission factor could also be transmitted individually by the MaterialPropertyBlock
        // Do note forget that we use a HDR camera with a Bloom image effect so we're completely cool with values over 1 !
		o.Albedo =  _Color.rgb * texBLEND.rgb * _Emission;
		
		// The resulting alpha influenced by :
		// - the animation texture (texBLEND.a)
		// - the R channel of the animation texture with a 'level' effect ponderated by the blue channel : saturate(fadeInFactor *15*(1-texEffect.b))
		// - the G channel of the animation texture with a 'level' effect ponderated by the blue channel : saturate(fadeOutFactor *15*(1-texEffect.b))
		// - the material color (_Color.a)
		o.Alpha = texBLEND.a * saturate(fadeInFactor *15*(1-texEffect.b)) * saturate(fadeOutFactor *15*(1-texEffect.b)) * _Color.a;
	}
	ENDCG
	}
	FallBack "Self-Illumin/Specular"
}