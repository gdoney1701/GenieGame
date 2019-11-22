Shader "Custom/Grayscale"
{



	HLSLINCLUDE


#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Blend;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
		color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
		//color.rgb = 1 - color;
		//color.a = 0;
		return color;
	}


		

		ENDHLSL

		SubShader
	{

		//stencil operation
		Stencil {
			Ref 1
			Comp NotEqual
		}

		Cull Off ZWrite Off ZTest Always

			Pass
		{
			//stencil operation
			Stencil{
			Ref 1
			Comp Equal
		}

			HLSLPROGRAM


#pragma vertex VertDefault
#pragma fragment Frag

			ENDHLSL
		}
	}
}

