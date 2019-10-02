Shader "Hidden/Custom/Grayscale"
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
		return color;
	}

		ENDHLSL

		SubShader
	{

		

		//stencil operation
		Stencil{
			Ref 1
			Comp Equal
		}

		Cull off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

#pragma vertex VertDefault
#pragma fragment Frag

			

			ENDHLSL
		}
	}
}

//
//Shader "Hidden/Custom/Grayscale"
//{
//Properties
//{
//	_MainTex("Base (RGB)", 2D) = "white" {}
//}
//SubShader
//{
//	Stencil
//{
//	Ref 1
//	Comp Equal
//}
//
//Tags{ "RenderType" = "Opaque" }
//LOD 200
//
//CGPROGRAM
//#pragma surface surf Lambert
//
//sampler2D _MainTex;
//
//struct Input {
//	float2 uv_MainTex;
//};
//
//void surf(Input IN, inout SurfaceOutput o)
//{
//	half4 c = tex2D(_MainTex, IN.uv_MainTex);
//	o.Albedo = c.rgb * half4(1,0,0,1);
//	o.Alpha = c.a;
//}
//ENDCG
//}
//FallBack "Diffuse"
//}