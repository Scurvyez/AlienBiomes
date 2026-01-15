Shader "Unlit/MoteGlowDistortedVertex"
{
    Properties
    {
        _MainTex ("Main texture", 2D) = "white" {}
        _DistortionTex ("Distortion texture", 2D) = "black" {}
        _ShimmerTex ("Shimmer Texture (Noise)", 2D) = "white" {}
        _ShimmerTiling ("Shimmer Tiling", Vector) = (1, 1, 0, 0)
        _Color ("Color", Color) = (1, 1, 1, 1)
        _AlphaFactor ("Alpha Factor", Float) = 1.0
        _DistortionScrollSpeed ("DistortionScrollSpeed", Vector) = (0.15, 0.15, 0, 0)
        _ShimmerScrollSpeed ("Shimmer Scroll Speed", Vector) = (0.1, 0.1, 0, 0)
        _ShimmerDistortionWeight ("Shimmer Distortion Weight", Float) = 0.5
        _DistortionScale ("DistortionScale", Float) = 0.2
        _DistortionIntensity ("DistortionIntensity", Float) = 0.3
        _WordSpaceDistortionToggle ("WordSpaceDistortionToggle", Float) = 0
        _TextureRepeatAmount ("TextureRepeatScale", Vector) = (0, 0, 0, 0)
        
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Int) = 5 // SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Int) = 1 // One
    }
    SubShader
    {
        Tags {
            "IGNOREPROJECTOR"="true"
            "QUEUE"="Transparent-100"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend  [_BlendSrc] [_BlendDst]
            ZClip On
            ZWrite Off
            
            Tags {
                "IGNOREPROJECTOR"="true"
                "QUEUE"="Transparent-100"
                "RenderType"="Transparent"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _DistortionTex;
            sampler2D _ShimmerTex;
            float4 _ShimmerTiling;

            float4 _Color;
            float _AlphaFactor;
            float4 _DistortionScrollSpeed;
            float4 _ShimmerScrollSpeed;
            float _ShimmerDistortionWeight;
            float _DistortionScale;
            float _DistortionIntensity;
            float _WordSpaceDistortionToggle;
            float4 _TextureRepeatAmount;
            float _GameSeconds;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float2 objS  : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                float3 basisX = float3(unity_ObjectToWorld._m00, unity_ObjectToWorld._m10, unity_ObjectToWorld._m20);
                float3 basisZ = float3(unity_ObjectToWorld._m02, unity_ObjectToWorld._m12, unity_ObjectToWorld._m22);
                o.objS.x = length(basisX);
                o.objS.y = length(basisZ);

                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 distScale = (_WordSpaceDistortionToggle > 0.5)
                    ? (i.objS * _DistortionScale)
                    : float2(_DistortionScale, _DistortionScale);

                float2 scroll = _DistortionScrollSpeed.xy * _GameSeconds;
                float2 duv = i.uv * distScale + scroll;

                float2 d = tex2D(_DistortionTex, duv).xy;
                float halfI = _DistortionIntensity * 0.5;
                float2 offset = d * _DistortionIntensity - float2(halfI, halfI);

                float2 shimmerScroll = _ShimmerScrollSpeed.xy * _GameSeconds;
                float2 suv = (i.uv * _ShimmerTiling.xy) + shimmerScroll;
                
                suv += offset * _ShimmerDistortionWeight;
                float shimmer = tex2D(_ShimmerTex, suv).r;
                
                float2 repeat = _TextureRepeatAmount.xy;
                bool useRepeat = any(repeat > float2(0.0, 0.0));

                float2 baseUV = useRepeat ? (i.objS * repeat) * i.uv : i.uv;
                
                fixed4 col = tex2D(_MainTex, baseUV + offset);
                col *= _Color * i.color;
                col.a *= shimmer * _AlphaFactor;
                
                return col;
            }
            ENDCG
        }
    }
}
