using UnityEngine;
using ZFramework.Core;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace GamePlay
{
	public class GuideView:UIView,IBeginDragHandler, IEndDragHandler
	{
		public GuideView_Bind Bind;

        private float fingerStartPos;
        private float fingerEndPos;

        private bool isFirstBattle = true;

        public override void OnLoadFinish()
        {
            Bind.Get_tip_text.text = "首次战斗提示";
            RectTransform rectFingerArea = Bind.Get_fingerArea_img.rectTransform;
            fingerStartPos = rectFingerArea.localPosition.x + rectFingerArea.rect.width / 2;
            fingerEndPos = rectFingerArea.localPosition.x - rectFingerArea.rect.width / 2;
        }

        public override void OnOpen()
        {
            FingerMove(fingerStartPos, fingerEndPos);
        }
        public override void OnClose()
        {
            base.OnClose();
        }
        private void FingerMove(float from, float to)
        {
            Bind.Get_finger_img.transform.DOLocalMoveX(to, 1.5f).OnComplete(() => FingerMove(to, from));
        }
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log($"[X] 鼠标按下");
            }
        }
        private float startDragX;
        public void OnBeginDrag(PointerEventData eventData)
        {
            startDragX = Input.mousePosition.x;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            float changePos = Input.mousePosition.x - startDragX;
            if (changePos >= fingerStartPos || changePos <= fingerEndPos)
            {
                OnClose();
            }
        }
    }
}
