using GamePlay;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using ZFramework.Core;
public enum MapBlockType
{
    startMap,
    endMap,
    LoopMap,
}
public class MapBlockManager : MonoBehaviour
{
    
    private MapBlockController mapBlock;//地图快资源
    private List<MapBlockController> mapBlocks;//地图块集合

    private int allowBlockCount;//允许存在的地图块数量

    private int mapIndex;//最新地图块编号

    private float nextZPos;//最新地图块生成z轴坐标
    private int startMapID;//起点地图id
    private int endMapID;//终点地图id
    private List<int> loopMapID= new List<int>();//循环地图id列表
    private int loopNum;//循环地图数量
    private void Awake()
    {
        GamePlayData.Instance.MapBlockMgrEx = this;
        mapBlocks = new List<MapBlockController>();
        allowBlockCount = 5;
    }
    private void OnDestroy()
    {
        GamePlayData.Instance.MapBlockMgrEx = null;
    }
    private void Start()
    {
        InitMapBlock();
    }
    private void InitData()
    {
        var map = TestData.GetMapData(GamePlayData.Instance.CurMapID);
        startMapID = map.StartMap;
        endMapID = map.EndMap;
        foreach (var id in map.LoopMap)
        {
            loopMapID.Add(id);
        }
        loopNum = map.LoopNum;
    }
    /// <summary>
    /// 初始化生成地图块，并将出生点通知给角色管理类
    /// </summary>
    private void InitMapBlock()
    {
        mapIndex = 0;
        nextZPos = 0f;
        InitData();

        for (int i=0;i<allowBlockCount;i++)
        {
            CreateMapBlock();
        }
        GamePlayData.Instance.CharacterMgrEx.InitCharacter(mapBlocks[0].spwanPoint.position);
    }
    /// <summary>
    /// 加载地图块资源
    /// </summary>
    private void LoadAssets(string path)
    {
        var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
        var servant = assetComponent.GetServant();
        mapBlock = servant.GetAsset<MapBlockController>(path);
        //servant.DisposeAll();
    }
    private MapBlockType mapType;
    /// <summary>
    /// 生成地图块
    /// </summary>
    private void CreateMapBlock()
    {
        if (GamePlayData.Instance.EventTriggerMgrEx.CheckEventOver())
            mapIndex = loopNum + 1;
        if (mapIndex > loopNum + 1) return;
        cfg.demo.MapBlock _mapBlock;
        if (mapIndex == 0)
        {
            _mapBlock = TestData.GetMapBlockData(startMapID);
            mapType = MapBlockType.startMap;
        }
        else if (mapIndex == loopNum + 1)
        {
            _mapBlock = TestData.GetMapBlockData(endMapID);
            mapType = MapBlockType.endMap;
        }
        else
        {
            int index = mapIndex % loopMapID.Count;
            if (index == 0)
                index = loopMapID.Count;
            _mapBlock = TestData.GetMapBlockData(loopMapID[index - 1]);
            mapType = MapBlockType.LoopMap;
        }
        LoadAssets(_mapBlock.Path);
        MapBlockController map = Instantiate(mapBlock, transform);
        if (map != null)
        {
            if (mapBlocks.Count != 0)
            {
                float preDistance = mapBlocks[mapBlocks.Count - 1].GetZDistance;
                nextZPos += map.GetZDistance + preDistance;
            }
            else
            {
                nextZPos += map.GetZDistance;
            }
            map.transform.position = new Vector3(0, 0, nextZPos);
            map.name = $"mapBlock {mapIndex}";
            map.MapType = mapType;
            mapIndex++;
            mapBlocks.Add(map);

            GamePlayData.Instance.EventTriggerMgrEx.CreateEvents(nextZPos);
        }
    }
    /// <summary>
    /// 删除已通过的地图块
    /// </summary>
    private void ReturnMapBlock()
    {
        if (mapBlocks.Count <= 0) return;
        MapBlockController controller = mapBlocks[0];
        mapBlocks.RemoveAt(0);
        Destroy(controller.gameObject,5);//TODO 改变销毁为返回到对象池
    }
    /// <summary>
    /// 角色抵达一个新地图块，删除旧的，创建新的
    /// </summary>
    /// <param name="mapctrl">地图块</param>
    public void CharacterArriveNewMapBlock(MapBlockController mapctrl)
    {
        ReturnMapBlock();
        CreateMapBlock();
    }

    public void ReChallenge()
    {
        for (int i = 0; i < mapBlocks.Count; i++)
        {
            Destroy(mapBlocks[i].gameObject);
        }
        mapBlocks.Clear();
        InitMapBlock();
    }
}
