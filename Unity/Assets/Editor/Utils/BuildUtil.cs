using System;
using UnityEditor;

namespace BuildTool
{
    public static class BuildUtil
    {
        /// <summary>
        /// 切换打包环境
        /// </summary>
        /// <param name="buildTarget"></param>
        public static void SwitchBuildPlatform(BuildTarget buildTarget)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(buildTarget) ,buildTarget);
        }

        public static BuildTarget GetActiveBuildTarget()
        {
            return EditorUserBuildSettings.activeBuildTarget;
        }
        public static BuildTargetGroup GetActiveBuildTargetGroup()
        {
            return BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
        }
    }
}