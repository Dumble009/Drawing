using System.Collections;
using UnityEngine;

public class DrawingTextureGenerator : MonoBehaviour
{
	const int TEXTURE_NUM = 10;
	Texture2D[] m_textures;

	[SerializeField] int m_scale;
	[SerializeField] float m_power;
	[SerializeField] float m_cor = 1;
	[SerializeField] Material m_postEffectMat;
	[SerializeField] float m_scaleX = 1;
	[SerializeField] float m_scaleY = 1;
	[SerializeField] float m_updateInterval = 0.1f;
	[SerializeField] float m_xScaleFactorStability = 0.9f;
	[SerializeField] float m_yScaleFactorStability = 0.9f;

	private void Awake()
	{
		int w = Screen.width;
		int h = Screen.height;
		m_textures = new Texture2D[TEXTURE_NUM];
		for(int i = 0; i < TEXTURE_NUM; i++)
		{
			m_textures[i] = new Texture2D(w, h, TextureFormat.ARGB32, false);
			CreateTexture(m_textures[i]);
		}

		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	private void Start()
	{
		StartCoroutine(UpdateImage());
	}

	IEnumerator UpdateImage()
	{
		int i = 0;
		while (true)
		{
			m_postEffectMat.SetTexture("_DrawingTex", m_textures[i]);
			i = (i + 1) % TEXTURE_NUM;
			yield return new WaitForSeconds(m_updateInterval);
		}
	}


	void CreateTexture(Texture2D tex)
	{
		int w = Screen.width;
		int h = Screen.height;
		float xScaleFactor = Random.value * (1.0f - m_xScaleFactorStability) + m_xScaleFactorStability;
		float yScaleFactor = Random.value * (1.0f - m_yScaleFactorStability) + m_yScaleFactorStability;
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				float sample = Mathf.PerlinNoise((float)x / w * m_scale * m_scaleX * xScaleFactor + Random.value, (float)y / h * m_scale * m_scaleY * yScaleFactor + Random.value);
				sample = Mathf.Pow(sample, m_power) * m_cor;
				tex.SetPixel(x, y, new Color(sample, sample, sample));
			}
		}

		tex.Apply();
	}
}
