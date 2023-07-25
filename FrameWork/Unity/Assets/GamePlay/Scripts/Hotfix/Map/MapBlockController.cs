using GamePlay;
using UnityEngine;
using ZFramework.Core;

public class MapBlockController : MonoBehaviour
{
    public Transform spwanPoint;//出生点，后续做一个单独的地图块

    private float zDistance;//本地图块中心点到z轴边界距离
    public float GetZDistance => zDistance;

    private bool isArrive;//角色是否抵达本地图快

    private MapBlockManager mapMgr;//地图块管理类


    private MapBlockType mapType;
    public MapBlockType MapType { set { mapType = value; } }

    void Awake()
    {
        isArrive = false;
        mapMgr = transform.parent.GetComponent<MapBlockManager>();
        zDistance = transform.Find("Ground").GetComponent<BoxCollider>().bounds.size.z / 2;
    }
    
    /// <summary>
    /// 角色抵达该地图块，通知管理类
    /// </summary>
    /// <param name="other">角色</param>
    private void OnTriggerEnter(Collider other)
    {
        if (MapBlockType.startMap == mapType)
            return;
        if (!isArrive)
        {
            CharacterController character = other.gameObject.GetComponent<CharacterController>();
            if (character != null)
            {
                mapMgr.CharacterArriveNewMapBlock(this);
                isArrive = true;
                if(MapBlockType.endMap==mapType)
                {
                    //TODO 播放结算界面
                    GamePlayData.Instance.CharacterMgrEx.GameWinHandle();
                }
            }
        }
    }
}
