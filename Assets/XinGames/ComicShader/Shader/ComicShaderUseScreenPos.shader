// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/ComicShaderUseScreenPos" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_LineWidth("Line Width", Float) = 0.01
		_LineColor("Line Color", Color) = (0.1,0.1,0.1,1.0)
		_Tone0Threshold("Tone 0 Threshold", Float) = 0.3
		_Tone1Threshold("Tone 1 Threshold", Float) = 0.1
		_Tone0Tex("Tone 0 (RGB)", 2D) = "white" {}
		_Tone1Tex("Tone 1 (RGB)", 2D) = "white" {}
		_Tone2Tex("Tone 2 (RGB)", 2D) = "black" {}
		_Tone0Tiling("Tone 0 Tiling", Float) = 1
		_Tone1Tiling("Tone 1 Tiling", Float) = 1
		_Tone2Tiling("Tone 2 Tiling", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Front

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Unlit vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct appdata {
			float4 texcoord : TEXCOORD0;
		};
		
		struct Input {
			float2 uv_MainTex;
		};

		float _LineWidth;
		fixed4 _LineColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			v.vertex.xyz += float4(v.normal * _LineWidth, 0);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = _LineColor.rgb;
			o.Alpha = 1.0f;
		}

		fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		ENDCG

		Cull Back

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Toon

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Tone0Tex;
		sampler2D _Tone1Tex;
		sampler2D _Tone2Tex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Tone0Tex;
			float2 uv_Tone1Tex;
			float2 uv_Tone2Tex;
			float4 screenPos;
		};

		struct SurfaceOutputComic {
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed Alpha;
			fixed3 Origin;
			fixed3 Tone0;
			fixed3 Tone1;
			fixed3 Tone2;
		};

		float _Tone0Threshold;
		float _Tone1Threshold;
		float _UseScreenPos;
		float _Tone0Tiling;
		float _Tone1Tiling;
		float _Tone2Tiling;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputComic o) {
			o.Albedo = fixed4(0.0f, 0.0f, 0.0f, 1.0f);
			o.Alpha = 1.0f;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Origin = c.rgb;

			float2 uv = IN.screenPos.xy / IN.screenPos.w;
			uv.y *= _ScreenParams.y / _ScreenParams.x;
			o.Tone0 = tex2D(_Tone0Tex, uv * _Tone0Tiling);
			o.Tone1 = tex2D(_Tone1Tex, uv * _Tone1Tiling);
			o.Tone2 = tex2D(_Tone2Tex, uv * _Tone2Tiling);
		}

		fixed4 LightingToon(SurfaceOutputComic s, fixed3 lightDir, fixed atten)
		{
			half d = dot(s.Normal, lightDir) * 0.5f + 0.5f;
			fixed4 c;
			c.rgb = s.Origin *_LightColor0.rgb * d;
			c.a = 0;
			fixed4 c2;
			float gray = 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;
			if (gray <= _Tone1Threshold)
			{
				c2.rgb = s.Tone2;
			}
			else if (gray < _Tone0Threshold)
			{
				c2.rgb = s.Tone1;
			}
			else
			{
				c2.rgb = s.Tone0;
			}
			c2.a = 0;
			return c2;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
