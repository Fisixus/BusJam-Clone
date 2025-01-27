using System.Collections.Generic;
using Core.Actors.Data;
using DG.Tweening;
using UnityEngine;

namespace Core.Actors
{
    public class Bus : MonoBehaviour
    {
        [field: SerializeField] public int Order { get; private set; }
        [field: SerializeField] public List<Renderer> BusRenderers { get; private set; }
        [field: SerializeField] public ColorType ColorType { get; set; }
        [field: SerializeField] public List<BusChair> BusChairs { get; set; }
        [field: SerializeField] public Transform DoorTr { get; set; }
        
        public Tween SetPosition(float newLocalX)
        {
            transform.DOKill();
            float animTime = 0.5f;
            var tween = transform.DOLocalMoveX(newLocalX, animTime).SetEase(Ease.InQuad);
            return tween;
        }
        
        public void SetPosition(BusDataSO busData, int order, bool isAnimOn=false)
        {
            transform.DOKill();
            Order = order;
            if (isAnimOn)
            {
                transform.DOLocalMoveX(busData.OrderXPositions[order], 0.5f).SetEase(Ease.InQuad);
            }
            else
            {
                transform.localPosition = new Vector3(busData.OrderXPositions[order],transform.localPosition.y, transform.localPosition.z);
            }
        }
        public void SetColor(ColorDataSO colorData)
        {
            foreach (var bRenderer in BusRenderers)
            {
                bRenderer.materials[1].color = colorData.Colors[ColorType];
            }
        }
        public void SetAttributes(int order, ColorType colorType)
        {
            Order = order;
            ColorType = colorType;
            if(ColorType == ColorType.Empty) this.gameObject.SetActive(false);
            name = ToString();
        }
        
        public override string ToString()
        {
            return $"Bus:{ColorType}, Order:{Order}";
        }
    }
}
