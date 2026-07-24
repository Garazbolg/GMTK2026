Shader "UI/Debug/Culled"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma fragment frag
            #pragma vertex vert

            void vert()
            {
            }

            void frag()
            {
            }
            ENDCG
        }
    }
}