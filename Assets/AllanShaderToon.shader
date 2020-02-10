// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Allan/ToonShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		// Ambient light is applied uniformly to all surfaces on the object.
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		// Controls the size of the specular reflection.
		_Glossiness("Glossiness", Float) = 32
		
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		// Control how smoothly the rim blends when approaching unlit
		// parts of the surface.
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1		
	}
	SubShader
	{
		Pass
		{
			// Setup our pass to use Forward rendering, and only receive
			// data on the main directional light and ambient light.
			Tags
			{
				"LightMode" = "ForwardBase"
				
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// Compile multiple versions of this shader depending on lighting settings.
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma target 3.0
			#include "UnityCG.cginc"
			// Files below include macros and functions to assist
			// with lighting and shadows.
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
	
			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD3;	
				// Macro found in Autolight.cginc. Declares a vector4
				// into the TEXCOORD2 semantic with varying precision 
				// depending on platform target.
				SHADOW_COORDS(2)
				UNITY_FOG_COORDS(1)
			};
			struct appdata

			{

				float4 vertex : POSITION;				

				float4 uv : TEXCOORD0;

				float3 normal : NORMAL;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);		
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				// Defined in Autolight.cginc. Assigns the above shadow coordinate
				// by transforming the vertex from world space to shadow-map space.
				TRANSFER_SHADOW(o)
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			float4 _Color;

			float4 _AmbientColor;

			float4 _SpecularColor;
			float _Glossiness;		

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;	

			float4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);

				// Lighting below is calculated using Blinn-Phong,
				// with values thresholded to creat the "toon" look.
				// https://en.wikipedia.org/wiki/Blinn-Phong_shading_model

				// Calculate illumination from directional light.
				// _WorldSpaceLightPos0 is a vector pointing the OPPOSITE
				// direction of the main directional light.
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				// Samples the shadow map, returning a value in the 0...1 range,
				// where 0 is in the shadow, and 1 is not.
				float shadow = SHADOW_ATTENUATION(i);
				// Partition the intensity into light and dark, smoothly interpolated
				// between the two to avoid a jagged break.
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);	
				// Multiply by the main directional light's intensity and color.
				float4 light = lightIntensity * _LightColor0;

				// Calculate specular reflection.
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				// Multiply _Glossiness by itself to allow artist to use smaller
				// glossiness values in the inspector.
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;				

				// Calculate rim lighting.
				float rimDot = 1 - dot(viewDir, normal);
				// We only want rim to appear on the lit side of the surface,
				// so multiply it by NdotL, raised to a power to smoothly blend it.
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float4 sample = tex2D(_MainTex, i.uv);
				UNITY_APPLY_FOG(i.fogCoord, sample);
				fixed4 ambient ; 
				//if(0.0 == _WorldSpaceLightPos0.w){
					//ambient = _AmbientColor;
					
					//return (light + ambient + specular + rim) * _Color * sample;
				//}else{
					//ambient = UNITY_LIGHTMODEL_AMBIENT;
					return (light + _AmbientColor + specular + rim) * _Color * sample;
				//}
				
			}
			ENDCG
		}
		Pass {    
			 Tags { "LightMode" = "ForwardAdd" } 
				// pass for additional light sources
			 Blend One One // additive blending 
	 
			 CGPROGRAM
	 
			 #pragma vertex vert  
			 #pragma fragment frag 
	 
			 #include "UnityCG.cginc"
			 uniform float4 _LightColor0; 
				// color of light source (from "Lighting.cginc")
			 uniform float4x4 unity_WorldToLight; // transformation 
				// from world to light space (from Autolight.cginc)
			 uniform sampler2D _LightTexture0; 
				// cookie alpha texture map (from Autolight.cginc)
	 
			 // User-specified properties
			 uniform float4 _Color; 
			 uniform float4 _SpecColor; 
			 uniform float _Shininess;
	 
			 struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			 };
			 struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 posWorld : TEXCOORD0;
				   // position of the vertex (and fragment) in world space 
				float4 posLight : TEXCOORD1;
				   // position of the vertex (and fragment) in light space
				float3 normalDir : TEXCOORD2;
				   // surface normal vector in world space
			 };
	 
			 vertexOutput vert(vertexInput input) 
			 {
				vertexOutput output;
	 
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				output.posWorld = mul(modelMatrix, input.vertex);
				output.posLight = mul(unity_WorldToLight, output.posWorld);
				output.normalDir = normalize(
				   mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			 }
	 
			 float4 frag(vertexOutput input) : COLOR
			 {
				float3 normalDirection = normalize(input.normalDir);
	 
				float3 viewDirection = normalize(
				   _WorldSpaceCameraPos - input.posWorld.xyz);
				float3 lightDirection;
				float attenuation;
	 
				if (0.0 == _WorldSpaceLightPos0.w) // directional light?
				{
				   attenuation = 1.0; // no attenuation
				   lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				} 
				else // point or spot light
				{
				   float3 vertexToLightSource = 
					  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				   float distance = length(vertexToLightSource);
				   attenuation = 1.0 / distance; // linear attenuation 
				   lightDirection = normalize(vertexToLightSource);
				}
	 
				float3 diffuseReflection = 
				   attenuation * _LightColor0.rgb * _Color.rgb
				   * max(0.0, dot(normalDirection, lightDirection));
	 
				float3 specularReflection;
				if (dot(normalDirection, lightDirection) < 0.0) 
				   // light source on the wrong side?
				{
				   specularReflection = float3(0.0, 0.0, 0.0); 
					  // no specular reflection
				}
				else // light source on the right side
				{
				   specularReflection = attenuation * _LightColor0.rgb 
					  * _SpecColor.rgb * pow(max(0.0, dot(
					  reflect(-lightDirection, normalDirection), 
					  viewDirection)), _Shininess);
				}
	 
				float cookieAttenuation = 1.0;
				if (0.0 == _WorldSpaceLightPos0.w) // directional light?
				{
				   cookieAttenuation = tex2D(_LightTexture0, 
					  input.posLight.xy).a;
				}
				else if (1.0 != unity_WorldToLight[3][3]) 
				   // spotlight (i.e. not a point light)?
				{
				   cookieAttenuation = tex2D(_LightTexture0, 
					  input.posLight.xy / input.posLight.w 
					  + float2(0.5, 0.5)).a;
				}

				return float4(cookieAttenuation 
				   * (diffuseReflection + specularReflection), 1.0);
			 }
	 
			 ENDCG
		}
		
		// Shadow casting support.
		
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}