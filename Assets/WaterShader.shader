Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NoiseTex("Noise text", 2D) = "white" {}
        _Colour("Colour", Color) = (1,1,1,1)
        _TintWater("Add Colour Tint", Range(0,1)) = 1
        _Period("Wobble Speed", Range(0,50)) = 40
        _Magnitude("Distortion", Range(0,0.5)) = 0.05
        _Scale("Bluriness", Range(0,10)) = 0.025
        _Strength("Wave Height", Range(0,200)) = 10
        _Speed("Wave Speed", Range(-500,500)) = 125

    }
        SubShader
        {
            Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            ZWrite Off Lighting Off Cull Off Fog { Mode Off } Blend One Zero
            LOD 110

            GrabPass { "_GrabTexture" }
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _GrabTexture;
                sampler2D _MainTex;
                sampler2D _NoiseTex;
                fixed4 _Colour;
                float _TintWater;

                float  _Period;
                float  _Magnitude;
                float  _Scale;

                float4 _Color;
                float _Strength;
                float _Speed;

                struct vertexInput {
                    float4 vertex : POSITION;
                };

                struct vertexOutput {
                    float4 pos : SV_POSITION;
                };

                struct vin_vct
                {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f_vct
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;

                    float4 worldPos : TEXCOORD1;
                    float4 uvgrab : TEXCOORD2;
                };

                // Vertex function 
                v2f_vct vert(vin_vct v)
                {
                    v2f_vct o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.color = v.color;
                    o.texcoord = v.texcoord;

                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                    o.uvgrab = ComputeGrabScreenPos(o.vertex);
                
                    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                    float displacement = cos(worldPos.y) + (cos(worldPos.x + (_Speed * _Time)));
                    float displacementTwo = cos(worldPos.y*0.75) + (cos(worldPos.x*0.85 - (_Speed * _Time*1.2)));
                    displacement += displacementTwo;
                    float high = v.vertex.y >1;
                    float off = 0;
                    if (high) {
                        off = displacement * (_Strength/100);
                    }
                    worldPos.y = worldPos.y + off;

                    o.vertex = mul(UNITY_MATRIX_VP, worldPos);

                    return o;
                }

                // Fragment function
                fixed4 frag(v2f_vct i) : COLOR
                {
                    float sinT = sin(_Time.w / _Period);
                    float2 distortion = float2
                    (tex2D(_NoiseTex, i.worldPos.xy / _Scale + float2(sinT, 0)).r - 0.5,
                        tex2D(_NoiseTex, i.worldPos.xy / _Scale + float2(0, sinT)).r - 0.5
                        );

                    i.uvgrab.xy += distortion * _Magnitude;

                    fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                    return (col  * _Colour) + (_TintWater * _Colour);
                    
                }

                ENDCG
            }           
        }
    FallBack "Diffuse"
}
