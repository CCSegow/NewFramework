using Bright.Serialization;
using cfg;
using SimpleJSON;
using UnityEngine;
using ZFramework.Core;

namespace GamePlay
{
    public class TestData
    {
        private static cfg.Tables _tables;
        public static void GetData(string fileName)
        {
            if (_tables == null)
            {
                InitTables();
            }

            var item = _tables.TbItem.Get(10001);
            Debug.Log($"{item.Name}, {item.Desc}");
            
            var weapon = _tables.TbWeapon.Get(101);
            Debug.Log($"{weapon.Name}, {weapon.Price} {weapon.Quality}");
            var weapon2 = _tables.TbWeapon.Get(201);
            Debug.Log($"{weapon2.Name}, {weapon2.Price} {weapon2.Quality}");
        }

        //一次性加载所有数据
        static void InitTables()
        {
            var tablesCtor = typeof(cfg.Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            Debug.Log($"loaderReturnType = {loaderReturnType}");
            // 根据cfg.Tables的构造函数的Loader的返回值类型决定使用json还是ByteBuf Loader
            System.Delegate loader = loaderReturnType == typeof(ByteBuf) ?
                new System.Func<string, ByteBuf>(LoadByteBuf)
                : (System.Delegate)new System.Func<string, JSONNode>(LoadJson);
            _tables = (cfg.Tables)tablesCtor.Invoke(new object[] {loader});
        }


        private static JSONNode LoadJson(string fileName)
        {
            Debug.Log($"使用Json 加载 {fileName}");
            
            var assetURL = $"Assets/Bundles/Configs/Jsons/{fileName}.json";
            var assetMgr = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
            var servant = assetMgr.GetServant();

            var text = servant.GetRawAssetText(assetURL);
            servant.DisposeAll();
            return JSON.Parse(text);
        }
        
        private static ByteBuf LoadByteBuf(string fileName)
        {
            Debug.Log($"使用Bytes 加载 {fileName}");
            
            var assetURL = $"Assets/Bundles/Configs/Bytes/{fileName}.bytes";
            var assetMgr = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
            var servant = assetMgr.GetServant();
            var bytes = servant.GetRawAssetBytes(assetURL);
            servant.DisposeAll();
            return new ByteBuf(bytes);
        }

    
    }
}