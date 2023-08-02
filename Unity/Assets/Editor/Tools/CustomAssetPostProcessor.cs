using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Assets.Editor.Tools
{
    /// <summary>
    /// https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html
    /// </summary>

    public class CustomAssetPostProcessor : AssetPostprocessor
    {
        /// <summary>
        /// 模型导入之前调用
        /// </summary>
        private void OnPreprocessModel()
        {
            ModelImporter _importer = (ModelImporter)assetImporter;
            _importer.materialImportMode = ModelImporterMaterialImportMode.None;
            
        }

        /// <summary>
        /// 导入贴图前处理
        /// </summary>
        void OnPreprocessTexture() {
            Debug.Log($"导入贴图 {assetPath}");

            TextureImporter importer = assetImporter as TextureImporter;
            importer.mipmapEnabled = false;
            importer.isReadable = false;

            if (assetPath.EndsWith("png")) {
                importer.textureType = TextureImporterType.Sprite;
            }
            else {
                //importer.textureType = TextureImporterType.Default;
            }
            //设置不同平台的属性
            //var setting = importer.GetPlatformTextureSettings("Android");
            //if (setting.format == TextureImporterFormat.ARGB32 || setting.format == TextureImporterFormat.RGB24)
            //{
            //    Debug.Log("Android平台贴图没压缩:" + importer.assetPath);
            //}
            //setting = importer.GetPlatformTextureSettings("iPhone");
            //if (setting.format == TextureImporterFormat.ARGB32 || setting.format == TextureImporterFormat.RGB24)
            //{
            //    Debug.Log("iOS平台贴图没压缩:" + importer.assetPath);
            //}

        }

        /// <summary>
        /// 导入纹理后的处理
        /// </summary>
        /// <param name="texture"></param>

        //private void OnPostprocessTexture(Texture2D texture)
        //{
        //    Debug.Log("导入贴图：" + texture.name);
        //    TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        //    if (textureImporter != null)
        //    {
        //        string AtlasName = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(assetPath)).Name;
        //        textureImporter.textureType = TextureImporterType.Sprite;
        //        textureImporter.spriteImportMode = SpriteImportMode.Single;
        //        textureImporter.spritePackingTag = AtlasName;
        //        textureImporter.mipmapEnabled = false;
        //    }
        //}

        /// <summary>
        /// 所有的资源的导入完成后都会调用
        /// </summary>
        /// <param name="importedAssets">导入资源路径</param>
        /// <param name="deletedAssets">删除资源路径</param>
        /// <param name="movedAssets">移动资源目标路径</param>
        /// <param name="movedFromAssetPaths">移动资源源路径</param>
        //private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        //{
        //    foreach (string str in importedAssets)
        //    {
        //        Debug.Log("导入资源: " + str);
        //    }
        //    foreach (string str in deletedAssets)
        //    {
        //        Debug.Log("删除资源: " + str);
        //    }
        //    for (int i = 0; i < movedAssets.Length; i++)
        //    {
        //        Debug.Log("从:" + movedFromAssetPaths[i] + "，移动资源到:" + movedAssets[i]);
        //    }
        //}
    }
}