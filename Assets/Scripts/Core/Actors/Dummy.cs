using System;
using System.Collections.Generic;
using Core.Actors.Ability;
using Core.Actors.Data;
using DG.Tweening;
using UnityEngine;

namespace Core.Actors
{
    public class Dummy : MonoBehaviour
    {
        [field: SerializeField] public DummyNavigator Navigator { get; private set; }
        [field: SerializeField] public Renderer JointRenderer { get; private set; }
        [field: SerializeField] public Renderer SurfaceRenderer { get; private set; }
        [field: SerializeField] public Outline SurfaceOutline { get; private set; }
        [field: SerializeField] public SpriteRenderer Emoji { get; private set; }
        [field: SerializeField] public Vector2Int Coordinate { get; set; }
        [field: SerializeField] public ColorType ColorType { get; set; }

        private Sequence _emojiSeq;
        
        public void SetStartPosition(float startX, Vector2 spacing)
        {
            transform.localPosition = new Vector3(startX + Coordinate.x * spacing.x, transform.localPosition.y,
                Coordinate.y * -spacing.y);
        }
        
        public void SetAttributes(Vector2Int newCoord, ColorType colorType)
        {
            Coordinate = newCoord;
            ColorType = colorType;
            SetOutline(false);
            if(ColorType == ColorType.Empty) this.gameObject.SetActive(false);
            name = ToString();
        }
        
        public void ResetAttributes()
        {
            Coordinate = -Vector2Int.one;
            ColorType = ColorType.Empty;
        }

        public void SetColor(ColorDataSO colorData)
        {
            JointRenderer.material.color = colorData.Colors[ColorType];
            SurfaceRenderer.material.color = colorData.Colors[ColorType];
        }
        
        public void SetOutline(bool isOn)
        {
            SurfaceOutline.OutlineWidth = isOn ? 4 : 0;
        }

        public void PlayEmojiAnimation()
        {
            _emojiSeq?.Kill();
            _emojiSeq = DOTween.Sequence()
                .Append(Emoji.transform.DOLocalMoveY(2, 0.25f))
                .Join(Emoji.DOFade(1, 0.25f))
                .AppendInterval(0.25f)
                .Append(Emoji.transform.DOLocalMoveY(1, 0.25f))
                .Join(Emoji.DOFade(0, 0.15f));
        }

        
        public override string ToString()
        {
            return $"Dummy:{ColorType}, Column{Coordinate.x},Row{Coordinate.y}";
        }


    }
}
