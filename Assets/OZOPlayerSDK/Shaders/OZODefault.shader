﻿//Copyright © 2016 Nokia Corporation and/or its subsidiary(-ies). All rights reserved.
Shader "Unlit/OZODefault"
{
        Properties
        {
                _MainTex ("Video Texture", 2D) = "white" {}
                _DepthTex("Depth Texture", 2D) = "white" {}
        }
        SubShader
        {
                Tags { "RenderType"="Opaque" }
                LOD 100
                ZWrite On

                Pass
                {
                        CGPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag
                        
                        #include "UnityCG.cginc"

                        struct appdata
                        {
                                float4 vertex : POSITION;
                                float2 uv : TEXCOORD0;
                        };

                        struct v2f
                        {
                                float2 uv0 : TEXCOORD0;
                                float2 uv1 : TEXCOORD1;
                                float4 vertex : SV_POSITION;
                        };

                        struct frag_out
                        {
                                fixed4 col : SV_Target;
                                float depth : SV_Depth;
                        };
                        sampler2D _MainTex;
                        float4  _MainTex_ST;
                        sampler _DepthTex;                      
                        float4  _DepthTex_ST;
                        
                        v2f vert (appdata v)
                        {
                                v2f o;
                                o.vertex = UnityObjectToClipPos(v.vertex);
                                o.uv0 = TRANSFORM_TEX(v.uv, _MainTex);
                                o.uv1 = TRANSFORM_TEX(v.uv, _DepthTex);
#if UNITY_UV_STARTS_AT_TOP
                                o.uv0.y = 1 - o.uv0.y;
                                o.uv1.y = 1 - o.uv1.y;
#endif
                                return o;
                        }
                        
                        frag_out frag (v2f i)
                        {
                                frag_out o;
                                o.col = tex2D(_MainTex, i.uv0);
#if defined(UNITY_REVERSED_Z)
                                float d = 1.0 - tex2D(_DepthTex, i.uv1);
#else
                                float d = tex2D(_DepthTex, i.uv1);
#endif                          
                                //o.col = fixed4(d,d,d, 1.0);
                                o.depth = d;
                                return o;
                        }
                        ENDCG
                }
        }
}