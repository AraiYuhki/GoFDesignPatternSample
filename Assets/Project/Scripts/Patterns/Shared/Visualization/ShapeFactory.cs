using System.Collections.Generic;
using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// 2Dビジュアライゼーション用のスプライトをプロシージャルに生成するファクトリ
    /// 円・矩形・三角形の基本図形をTexture2Dから作成し、キャッシュする
    /// </summary>
    public static class ShapeFactory {
        /// <summary>生成済みスプライトのキャッシュ</summary>
        private static readonly Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        /// <summary>テクスチャ解像度（ピクセル）</summary>
        private const int Resolution = 128;
        /// <summary>SpriteのPixels Per Unit</summary>
        private const float PixelsPerUnit = 128f;

        /// <summary>
        /// 円形スプライトを取得する
        /// </summary>
        /// <returns>円形のSprite</returns>
        public static Sprite GetCircle() {
            const string key = "circle";
            if (spriteCache.TryGetValue(key, out var cached)) {
                return cached;
            }

            var texture = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            float center = Resolution * 0.5f;
            float radiusSq = center * center;

            for (int y = 0; y < Resolution; y++) {
                for (int x = 0; x < Resolution; x++) {
                    float dx = x - center + 0.5f;
                    float dy = y - center + 0.5f;
                    float distSq = dx * dx + dy * dy;
                    float alpha = Mathf.Clamp01((radiusSq - distSq) / (center * 2f));
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }
            texture.Apply();

            var sprite = Sprite.Create(texture, new Rect(0, 0, Resolution, Resolution), new Vector2(0.5f, 0.5f), PixelsPerUnit);
            spriteCache[key] = sprite;
            return sprite;
        }

        /// <summary>
        /// 矩形スプライトを取得する
        /// </summary>
        /// <returns>矩形のSprite</returns>
        public static Sprite GetRect() {
            const string key = "rect";
            if (spriteCache.TryGetValue(key, out var cached)) {
                return cached;
            }

            const int size = 4;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    texture.SetPixel(x, y, Color.white);
                }
            }
            texture.Apply();

            var sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            spriteCache[key] = sprite;
            return sprite;
        }

        /// <summary>
        /// 三角形（矢印頭）スプライトを取得する
        /// </summary>
        /// <returns>三角形のSprite</returns>
        public static Sprite GetTriangle() {
            const string key = "triangle";
            if (spriteCache.TryGetValue(key, out var cached)) {
                return cached;
            }

            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;

            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
            for (int y = 0; y < size; y++) {
                float progress = (float)y / size;
                int halfWidth = Mathf.RoundToInt(progress * size * 0.5f);
                int centerX = size / 2;
                for (int x = centerX - halfWidth; x <= centerX + halfWidth; x++) {
                    if (x >= 0 && x < size) {
                        texture.SetPixel(x, y, Color.white);
                    }
                }
            }
            texture.Apply();

            var sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            spriteCache[key] = sprite;
            return sprite;
        }
    }
}
