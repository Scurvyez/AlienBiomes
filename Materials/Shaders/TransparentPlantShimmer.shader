Shader "Unlit/TransparentPlantShimmer"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // the main texture
        _MainTex_ST ("Texture tiling and offset", Vector) = (1, 1, 0, 0)
        _WindSpeed ("Wind Speed", Range(0, 10)) = 1 // the current wind speed on the map
        _GameSeconds ("Global Seconds", Float) = 0 // the global time in seconds
        _SwayHead ("Sway Head", Range(-1, 1)) = 0.5 // the amount to sway the head of the plant
    }

    SubShader
    {
        Tags 
		{ 
			"IgnoreProjector" = "true" 
			"Queue" = "Transparent-100"
			"RenderType" = "Transparent"
		}

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // ensures alpha channels are retained in texture

            CGPROGRAM
            #pragma vertex vert // specifies the vertex shader function to use, which in this case is called
            #pragma fragment frag // specifies the fragment (pixel) shader function to use
            #include "UnityCG.cginc" // contains various macros and functions used by Unity shaders

            //  defines the input vertex format for the vertex shader
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinates
            };

            // defines the output vertex format for the vertex shader
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinates
                float4 vertex : SV_POSITION; // vertex position in clip space
            };

            sampler2D _MainTex; // declares the main texture sampler
            float4 _MainTex_ST; // declares texture coordinate scale and offset for main texture
            float4 _WindSpeed; // declares the current wind speed on the map
            float _GameSeconds; // declares passage of time in-game, in seconds
            float _SwayHead; // declares the sway height value for the plant

            // vertex shader function, input vertex format as an argument
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Calculate the y-coordinate of the vertex in texture space.
                float y = v.uv.y * _MainTex_ST.y + _MainTex_ST.w;

                // Calculate the normalized distance from the current row of pixels to the top of the texture.
                float distFromTop = y / _MainTex_ST.y;

                // Calculate the amount of movement for this row of pixels.
                float swayAmount = 0.0;

                // if (distFromTop >= _SwayHead)
                if (distFromTop >= _SwayHead)
                {
                    // Calculate the normalized distance from the _SwayHead value to the top of the texture.
                    float distFromSwayHead = (distFromTop - _SwayHead) / (1.0 - _SwayHead);

                    // Use a smoothstep function to map the distance from the _SwayHead value to a smooth curve between 0 and 1.
                    float smoothDistFromSwayHead = smoothstep(0.0, 1.0, 1.0 - distFromSwayHead);

                    // Calculate the sway amount for this row of pixels based on the smoothed distance from the _SwayHead value.
                    swayAmount = sin(_GameSeconds * _WindSpeed + smoothDistFromSwayHead * _MainTex_ST.y) * 0.2 * (1.0 + y) * (1.0 - smoothDistFromSwayHead);
                }

                // Offset the vertex along the x-axis based on the calculated swayAmount.
                o.vertex.xy += swayAmount;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // transform the texture coordinates
                return o;
            }

            // determins the color of each pixel of the rendered object
            // (i) = interpolated vertex data for the current pixel being rendered
            // SV_Target = specify the output color of the fragment shader
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // Get the original color from the texture
                col.a = tex2D(_MainTex, i.uv).a; // Set the alpha of the color to be the same as the alpha in the texture

                // Calculate the brightness adjustment factor based on time and vertical position
                float brightness = sin(_GameSeconds * 5.0/*speed*/ + i.uv.y * 7.5/*shimmer length*/) * 0.25 + 0.75;

                // Apply the brightness adjustment factor to the RGB values of the color
                col.rgb *= brightness;

                // Create vein-like pattern
                //float vein = abs(sin(i.uv.x * 200.0)) * abs(sin(i.uv.y * 200.0)) * 3.0;

                // Adjust color based on vein pattern
                //col.rgb *= vein;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // specifies that the default Unity "Diffuse" shader should be used if this one fails to compile or is not supported
}