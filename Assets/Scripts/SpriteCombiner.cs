using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MysticalForestAdventure
{
	public static class SpriteCombiner
	{
		public static Sprite CombineSpritesVertically(List<Sprite> spritesToCombine, int spacing)
		{
			if (spritesToCombine == null || spritesToCombine.Count == 0)
			{
				Debug.LogError("No sprites provided to combine.");
				return null;
			}

			int maxWidth = 0;
			int combinedHeight = spacing * (spritesToCombine.Count - 1);

			foreach (var sprite in spritesToCombine)
			{
				maxWidth = Mathf.Max(maxWidth, (int)sprite.rect.width);
				combinedHeight += (int)sprite.rect.height;
			}

			Texture2D combinedTexture = new Texture2D(maxWidth, combinedHeight, TextureFormat.ARGB32, false);

			Color transparent = new Color(0, 0, 0, 0);
			for (int x = 0; x < maxWidth; x++)
			{
				for (int y = 0; y < combinedHeight; y++)
				{
					combinedTexture.SetPixel(x, y, transparent);
				}
			}

			combinedTexture.Apply();

			int offsetY = 0;

			foreach (var sprite in spritesToCombine)
			{
				Texture2D spriteTexture = SpriteToTexture(sprite);
				if (spriteTexture == null)
				{
					Debug.LogError("Failed to convert sprite to texture.");
					continue;
				}

				int offsetX = (maxWidth - spriteTexture.width) / 2;
				for (int x = 0; x < spriteTexture.width; x++)
				{
					for (int y = 0; y < spriteTexture.height; y++)
					{
						Color pixelColor = spriteTexture.GetPixel(x, y);
						combinedTexture.SetPixel(x + offsetX, y + offsetY, pixelColor);
					}
				}

				offsetY += spriteTexture.height + spacing;
			}

			combinedTexture.Apply();

			Sprite combinedSprite = Sprite.Create(combinedTexture, new Rect(0, 0, maxWidth, combinedHeight), new Vector2(0.5f, 0.5f));

			return combinedSprite;
		}

		public static Texture2D CombineSpritesVerticallyForTexture(List<Sprite> spritesToCombine, int spacing)
		{
			return SpriteToTexture(CombineSpritesVertically(spritesToCombine, spacing));
		}

		private static Texture2D SpriteToTexture(Sprite sprite)
		{
			if (sprite == null)
				return null;

			Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);
			Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
			texture.SetPixels(pixels);
			texture.Apply();
			return texture;
		}
	}
}
