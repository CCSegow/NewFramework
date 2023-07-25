//shader·��/����
Shader "Unlit/MyTestShader"
{
	//shader�������ڴ˴��������˴����������Իᱻ���л������ʵ�inspector����ϣ����ñ༭���޸�
	Properties
	{
		//shader���Ե���������_MainTexΪ����_MainTexΪ��������������Ϊ��ʾ�ڲ���inspector����ϵ����֣�2DΪ�������ͣ� = ����ΪĬ��ֵ��
		_MainTex("������", 2D) = "white" {}
		_High("�߹ⷶΧ",Int) = 30
    }
		//��shader��һ��shader���ж����shader �Թ��ڲ�ͬƽ̨ʹ�á�
		//ϵͳÿ��ֻ��ִ��һ������ɫ����ѡ������ɫ���ı�׼���Ǹ�������ɫ�������õ�LOD��ֵ������ѡ��
    SubShader
    {
			LOD 100


		//��Ⱦͨ����һ����Ⱦ��ִ��һ��ͨ����һ����shader�����ж��pass��һ��������Զ����Ⱦ���ö����Ⱦ�Ľ����ϳ�����Ч��
        Pass
        {
			//�����pass�Ĺ���ģʽ��ForwardBaseģʽ��ֻ����ƽ�й⣬�������shaderû�п��ǹ���˥����
			//�������Ҫ���Ƕ��Դ����Ҫ��дһ��pass���������ģʽΪForwardAdd����pass�ͻῼ�Ƕ���Ĺ�Դ���������
			Tags{"LightMode" = "ForwardBase"}

            CGPROGRAM              //shader�﷨  CG���Ŀ�ʼ

            #pragma vertex vert          //shader����������    #pragma vertex ����һ��������ɫ������  vertΪ������
            #pragma fragment frag           //  fragmentΪƬԪ��ɫ������    fragΪ������

            #include "UnityCG.cginc"          //include unity�Ŀ���ʹ��API
			#include "Lighting.cginc"         

            struct appdata           //�ṹ��Ķ��壬�˽ṹ����������vert �Ĳ���
            {
			//���ݵ�������ɫ����ƬԪ��ɫ�������ݣ�������Ҫ�������壬��Ϊ����ģ�����ݣ�������Ҫ���߱�����������Ҫʲôģ������
				//��������  ������   ����
                float4 vertex : POSITION;     //POSITION�����ʾ��������
				float3 normal : NORMAL;       //NORMAL�����ʾ���㷨��
				float2 uv : TEXCOORD0;        //TEXCOORD0�����ʾģ�͵�һ����ͼ��UV����
            };

            struct v2f  //�˽ṹ������vert�ķ���ֵ�Լ�����Ĵ���frag�Ĳ������˽ṹ�徭��������ɫ��vert����֮�����դ���׶β�ֵ֮���ٴ���ƬԪ��ɫ��frag
            {
				//��ƬԪ��ɫ����ʹ�õı��������Ƕ����ʱ��Ͳ���Ҫ�ϸ����������˼�������ˣ���Ϊ���������Լ�ʹ�õ�����
				//����������Ҫ���壬��Ϊ��Щ������Ҫ�ڹ�դ������ֵ
                float4 vertex : SV_POSITION;        //SV_POSITION ��ʾ�ü��ռ�����꣬����frag�лᱻ��ֵΪƬԪ����
				float3 normal : TEXCOORD2;           //���㷨��
				float2 uv:TEXCOORD0;                //UV����
				float3 worldPos : TEXCOORD1;        //�������е�����
            };
			 
			sampler2D _MainTex;   //��Properties�����������ԣ���Ҫ�ٶ���һ�飬������ͬ�����ƾ��ܻ�ȡ�������е�ֵ
			int _High;

            v2f vert (appdata v)
            {
                v2f o;

				//�ȼ���o.vertex = mul(UNITY_MATRIX_MVP, v.vertex)���˴���ģ�Ϳռ�任���ü��ռ�
                o.vertex = UnityObjectToClipPos(v.vertex);

				//�ȼ���o.normal = mul((transpose(unityWorldToObject)),v.normal)���˴���ģ�Ϳռ�ķ��߱任Ϊ����ռ��еķ���
				o.normal = UnityObjectToWorldNormal(v.normal);
				//�˴���ģ�Ϳռ�����任������ռ�����
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.uv = v.uv;
				//�˴����ص�o�ᴫ���դ���׶Σ���ֵ֮����ƬԪ��ɫ��
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//������ƬԪ��ɫ����shading����Ϊ����Ⱦͨ��ʹ�õ���ƽ�й⣬���Բ�����˥��
				//ʹ��uv�������ͼ�л�ȡ�����ص����ɫ
				float4 _MainColor = tex2D(_MainTex,i.uv);
				//��ȡ������
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				//��ȡ���շ��򲢽����׼��Ϊ��λ����
				float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				//��׼��������
				float3 normal = normalize(i.normal);
				//ʹ��������������ģ�����������
				float3 diffuse = _LightColor0.rgb*_MainColor.rgb*max(0,dot(normal, lightDir));
				//��ȡ�۲췽�򲢱�׼��
				float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				//����������
				float3 centerDir = normalize(viewDir + lightDir);
				//ʹ�ò��ַ�ģ������߹�
				float3 highlight = _LightColor0.rgb*_MainColor.rgb*pow(max(0, dot(centerDir, normal)), _High);
				//���Ϊ �߹� + ������ + ������
				float4 res = float4(diffuse + ambient + highlight, 1.0);

				return res; 
            }
            ENDCG  //CG���Ľ���
        }
    }
			FallBack "Diffuse"  //�����shader������subshader���޷�ʹ�ã�����ʹ��FallBackָ���shader
}