using UnityEngine.UI;
using UnityEngine;

// by @Bullrich

namespace Blue.Console
{
	public class ToggleIcon : MonoBehaviour {
        public Sprite toggleSprite;
        Sprite originalSprite;

        public void ChangeSprite() {
            Image imageContainer = GetComponent<Image>();
            if (originalSprite == null) {
                originalSprite=imageContainer.sprite;
                imageContainer.sprite = toggleSprite;
            } else {
                if (imageContainer.sprite == originalSprite)
                    imageContainer.sprite = toggleSprite;
                else
                    imageContainer.sprite = originalSprite;
            }
        }
	}
}
