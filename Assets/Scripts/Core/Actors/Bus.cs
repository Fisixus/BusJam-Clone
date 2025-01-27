using System.Collections.Generic;
using System.Linq;
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
        
        public Tween SetPosition(float newLocalX, float animTime = 0.7f)
        {
            transform.DOKill();
            var tween = transform.DOLocalMoveX(newLocalX, animTime).SetEase(Ease.InQuad);
            return tween;
        }
        
        public void SetPosition(BusDataSO busData, int order, bool isAnimOn=false, float animTime = 0.7f)
        {
            transform.DOKill();
            Order = order;
            if (isAnimOn)
            {
                transform.DOLocalMoveX(busData.OrderXPositions[order], animTime).SetEase(Ease.InQuad);
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
            name = ToString();
        }
        
        /// <summary>
        /// Checks if the given bus is full.
        /// </summary>
        public bool IsBusFull()
        {
            return BusChairs.All(chair => !chair.IsAvailable);
        }

        /// <summary>
        /// Finds the next available chair in the active bus.
        /// </summary>
        public BusChair GetNextAvailableChair()
        {
            return BusChairs.FirstOrDefault(chair => chair.IsAvailable);
        }
        
        public void SitNextChair(ColorDataSO colorData)
        {
            BusChair nextChair = GetNextAvailableChair();
            if (nextChair != null)
            {
                nextChair.IsAvailable = false;
                nextChair.SetChairOwner(ColorType,colorData);
            }
        }
        
        public override string ToString()
        {
            return $"Bus:{ColorType}, Order:{Order}";
        }
    }
}
