Shader "Triniti/Character/COL_ACOL" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex(RGB)", 2D) = "" {}
 _AColor ("Additive Color", Color) = (0,0,0,0)
}
SubShader { 
 Tags { "QUEUE"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant }
  SetTexture [_MainTex] { ConstantColor [_AColor] combine previous + constant, previous alpha }
 }
}
}