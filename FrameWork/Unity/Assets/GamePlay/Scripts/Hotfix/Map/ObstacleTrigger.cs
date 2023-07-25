using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    private ObstacleEvent obstacleEvent;
    public ObstacleEvent ObstacleEventMethod { get { return obstacleEvent; } set { obstacleEvent = value; } }
    /// <summary>
    /// 进入触发器
    /// </summary>
    /// <param name="other">触发物体</param>
    private void OnTriggerEnter(Collider other)
    {
        CharacterController character=other.GetComponent<CharacterController>();
        if(character != null )
        {
            obstacleEvent.StartTrigger(character);
        }
    }
}
