using System.Collections;
using UnityEngine;

public class DrawingTextureGenerator : MonoBehaviour
{
	Texture2D m_texture;

	[SerializeField] int m_scale;
	[SerializeField] float m_power;
	[SerializeField] float m_cor = 1;
	[SerializeField] Material m_postEffectMat;
	[SerializeField] float m_scaleX = 1;
	[SerializeField] float m_scaleY = 1;

	private void Awake()
	{
		int w = Screen.width;
		int h = Screen.height;
		m_texture = new Texture2D(w, h, TextureFormat.ARGB32, false);
	}

	private void Start()
	{
		StartCoroutine(UpdateImage());
		m_postEffectMat.SetTexture("_DrawingTex", m_texture);
	}

	IEnumerator UpdateImage()
	{
		// 1. パーリンノイズテクスチャの生成
		while (true)
		{
			int w = Screen.width;
			int h = Screen.height;
			float t = Time.realtimeSinceStartup;
			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					float sample = Mathf.PerlinNoise((float)x / w * m_scale * m_scaleX + Random.value, (float)y / h * m_scale * m_scaleY);
					sample = Mathf.Pow(sample, m_power) * m_cor;
					m_texture.SetPixel(x, y, new Color(sample, sample, sample));

					if(Time.realtimeSinceStartup - t >= 1.0f / 60.0f)
					{
						yield return null;
						t = Time.realtimeSinceStartup;
					}
				}
			}

			m_texture.Apply();
		}
	}
}
