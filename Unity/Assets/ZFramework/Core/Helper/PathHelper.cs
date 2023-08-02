using UnityEngine;
using System.Collections;
using System.IO;
using Sirenix.Utilities;

namespace ZFramework {
    public static class PathHelper
    {

        public static void CreateDir(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        public static string NormalizePath(this string path) {
            var normalizePath = path.Replace('\\','/');
            return normalizePath;
        }

        public static bool EndsWith(this string path,params string[] extenstion) {

            foreach (var item in extenstion)
            {
                if (path.FastEndsWith(item)) {
                    return true;
                }
            }
            return false;
        }
        public static bool Contains(this string path,string[] targets) {
            foreach (var item in targets){
                if (path.Contains(item)) {
                    return true;
                }
            }

            return false;
        }

        #region Editor

#if UNITY_EDITOR
        public static string AssetPath2AbsoluteDataPath(string assetPath)
        {
            
            return $"{Application.dataPath.RemoveEnd("Assets")}{assetPath}".NormalizePath();
        }

        public static string AssetPath2AbsoluteStreammingAssetPath(string assetPath)
        {
            return $"{Application.streamingAssetsPath.RemoveEnd("Assets")}{assetPath}".NormalizePath();
        }
        public static string AssetPath2AbsolutePersistenPath(string assetPath)
        {
            return $"{Application.persistentDataPath.RemoveEnd("Assets")}{assetPath}".NormalizePath();
        }

        public static string AbsoluteDataPath2AssetPath(string assetFullPath)
        {
            return assetFullPath.Substring(Application.dataPath.Length - "Assets".Length);            
        }
#endif

        static string RemoveEnd(this string path,string target) {
            return path.Remove(path.IndexOf(target));
        }


        #endregion
    }

}
