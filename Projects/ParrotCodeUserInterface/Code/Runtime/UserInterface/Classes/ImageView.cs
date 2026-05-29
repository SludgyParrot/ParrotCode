using UnityEngine;
using ParrotCode.Native.Common;
using UnityEngine.UI;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public sealed class ImageView : BaseMonoBehaviour, IImageView
    {
        private Image imageRenderer;

        public Image ImageRenderer
        {
            get
            {
                if (imageRenderer == null) 
                    imageRenderer = GetComponent<Image>();
                return imageRenderer;
            }
        }

        public void SetColor(Color color)
            => ImageRenderer.color = color;

        public void SetImage(Sprite image)
            => imageRenderer.sprite = image;
    }
}
