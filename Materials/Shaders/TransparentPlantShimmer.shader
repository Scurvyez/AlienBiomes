Shader "Unlit/TransparentPlantShimmer"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // the main texture
        _MainTex_ST ("Texture tiling and offset", Vector) = (1, 1, 0, 0)
        _HashOffset ("Thing Hash", Float) = 0
    }

    SubShader
    {
        Tags 
		{ 
			"IgnoreProjector" = "true" 
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

        Pass
        {
            Tags 
		    { 
			    "IgnoreProjector" = "true" 
			    "Queue" = "Transparent"
			    "RenderType" = "Transparent"
		    }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};

			float _SwayHead; // some shit in c# that makes no sense to me :D
			sampler2D _MainTex; // declares the main texture
            float _GameSeconds; // declares passage of time in-game, in seconds
            float _HashOffset;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0.xy = v.vertex.xz * float2(4.0, 4.0);
                tmp0.xy = _SwayHead.xx * float2(0.15, 0.15) + tmp0.xy;
                tmp0.xy = sin(tmp0.xy);
                tmp0.xz = tmp0.xy * float2(0.1, 0.1);
                tmp0.y = 0.0;
                tmp0.xyz = tmp0.xyz * v.color.www + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord.xyz = v.texcoord.xyz;
                return o;
			}

			fout frag(v2f inp)
			{
                fout o;
                float2 uv = inp.texcoord.xy;

                // Calculate the brightness adjustment factor based on time and vertical position
                float brightness = sin((_GameSeconds + (_HashOffset % 1)) * 7.0/*speed*/ + uv.y * 7.5/*shimmer length*/) * 0.25 + 0.75;

                // Get the original color from the texture
                fixed4 col = tex2D(_MainTex, uv);

                // Apply the brightness adjustment factor to the RGB values of the color
                col.rgb *= brightness;

                // Calculate the color to be added for the glow effect
                fixed4 glowColor = col * 0.5 * brightness;

                // Add the glow effect to the final color
                col += glowColor;

                o.sv_target = col;
                return o;
			}
			ENDCG
        }
    }
    FallBack "Diffuse" // specifies that the default Unity "Diffuse" shader should be used if this one fails to compile or is not supported
}