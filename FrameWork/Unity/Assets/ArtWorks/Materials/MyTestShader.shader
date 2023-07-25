//shader路径/名称
Shader "Unlit/MyTestShader"
{
	//shader的属性在此处声明，此处声明的属性会被序列化到材质的inspector面板上，可用编辑器修改
	Properties
	{
		//shader属性的声明，以_MainTex为例，_MainTex为属性名，主纹理为显示在材质inspector面板上的名字，2D为数据类型， = 后面为默认值。
		_MainTex("主纹理", 2D) = "white" {}
		_High("高光范围",Int) = 30
    }
		//子shader，一个shader会有多个子shader 以供在不同平台使用。
		//系统每次只会执行一个子着色器，选择子着色器的标准就是根据子着色器所设置的LOD的值来进行选择
    SubShader
    {
			LOD 100


		//渲染通道，一次渲染会执行一次通道，一个子shader可以有多个pass，一个物体可以多次渲染，用多次渲染的结果混合出最终效果
        Pass
        {
			//定义此pass的光照模式，ForwardBase模式下只考虑平行光，所以这个shader没有考虑光照衰减。
			//如果我们要考虑多光源，就要再写一个pass，定义光照模式为ForwardAdd，该pass就会考虑额外的光源，再做混合
			Tags{"LightMode" = "ForwardBase"}

            CGPROGRAM              //shader语法  CG语句的开始

            #pragma vertex vert          //shader方法的声明    #pragma vertex 声明一个顶点着色器方法  vert为方法名
            #pragma fragment frag           //  fragment为片元着色器方法    frag为方法名

            #include "UnityCG.cginc"          //include unity的库以使用API
			#include "Lighting.cginc"         

            struct appdata           //结构体的定义，此结构体用作传入vert 的参数
            {
			//传递到顶点着色器和片元着色器的数据，我们需要定义语义，因为这是模型数据，我们需要告诉编译器我们需要什么模型数据
				//数据类型  变量名   语义
                float4 vertex : POSITION;     //POSITION语义表示顶点坐标
				float3 normal : NORMAL;       //NORMAL语义表示顶点法线
				float2 uv : TEXCOORD0;        //TEXCOORD0语义表示模型第一个贴图的UV坐标
            };

            struct v2f  //此结构体用作vert的返回值以及传入的传入frag的参数，此结构体经过顶点着色器vert处理之后传入光栅化阶段插值之后再传入片元着色器frag
            {
				//在片元着色器中使用的变量，我们定义的时候就不需要严格按照语义的意思来定义了，因为这是我们自己使用的数据
				//但我们仍需要语义，因为这些数据需要在光栅化被插值
                float4 vertex : SV_POSITION;        //SV_POSITION 表示裁剪空间的坐标，传到frag中会被插值为片元坐标
				float3 normal : TEXCOORD2;           //顶点法线
				float2 uv:TEXCOORD0;                //UV坐标
				float3 worldPos : TEXCOORD1;        //在世界中的坐标
            };
			 
			sampler2D _MainTex;   //在Properties中声明的属性，需要再定义一遍，保持相同的名称就能获取到属性中的值
			int _High;

            v2f vert (appdata v)
            {
                v2f o;

				//等价于o.vertex = mul(UNITY_MATRIX_MVP, v.vertex)，此处将模型空间变换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);

				//等价于o.normal = mul((transpose(unityWorldToObject)),v.normal)，此处将模型空间的法线变换为世界空间中的法线
				o.normal = UnityObjectToWorldNormal(v.normal);
				//此处将模型空间坐标变换到世界空间坐标
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.uv = v.uv;
				//此处返回的o会传入光栅化阶段，插值之后传入片元着色器
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//我们在片元着色器做shading，因为该渲染通道使用的是平行光，所以不会有衰减
				//使用uv坐标从贴图中获取该像素点的颜色
				float4 _MainColor = tex2D(_MainTex,i.uv);
				//获取环境光
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				//获取光照方向并将其标准化为单位向量
				float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				//标准化法向量
				float3 normal = normalize(i.normal);
				//使用兰伯特漫反射模型求出漫反射
				float3 diffuse = _LightColor0.rgb*_MainColor.rgb*max(0,dot(normal, lightDir));
				//获取观察方向并标准化
				float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				//求出半程向量
				float3 centerDir = normalize(viewDir + lightDir);
				//使用布林冯模型求出高光
				float3 highlight = _LightColor0.rgb*_MainColor.rgb*pow(max(0, dot(centerDir, normal)), _High);
				//结果为 高光 + 漫反射 + 环境光
				float4 res = float4(diffuse + ambient + highlight, 1.0);

				return res; 
            }
            ENDCG  //CG语句的结束
        }
    }
			FallBack "Diffuse"  //如果该shader中所有subshader都无法使用，尝试使用FallBack指向的shader
}