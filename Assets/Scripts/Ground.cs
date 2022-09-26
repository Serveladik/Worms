using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    
    public Texture2D baseTexture;

    Texture2D cloneTexture;
    SpriteRenderer spriteRenderer;
    float _worldWidth, _worldHeight;
    float _pixelWidth, _pixelHeight;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        cloneTexture = Instantiate(baseTexture);
        cloneTexture.alphaIsTransparency = true;

        UpdateTexture();
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public float WorldWidth
    {
        get
        {
            if(_worldWidth == 0)
            {
                _worldWidth = spriteRenderer.bounds.size.x;
            }
            return _worldWidth;
        }
    }

    public float WorldHeight
    {
        get
        {
            if(_worldHeight == 0)
            {
                _worldHeight = spriteRenderer.bounds.size.y;
            }
            return _worldHeight;
        }
    }

    public float PixelWidth
    {
        get
        {
            if(_pixelWidth == 0)
            {
                _pixelWidth = spriteRenderer.sprite.texture.width;
            }
            return _pixelWidth;
        }
    }

    public float PixelHeight
    {
        get
        {
            if(_pixelHeight == 0)
            {
                _pixelHeight = spriteRenderer.sprite.texture.height;
            }
            return _pixelHeight;
        }
    }

    void UpdateTexture()
    {
        spriteRenderer.sprite = Sprite.Create(cloneTexture, new Rect(0, 0, cloneTexture.width, cloneTexture.height), new Vector2(0.5f, 0.5f), 50f);
    }

    Vector2Int WorldToPixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;
        float dx = (pos.x - transform.position.x);
        float dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * PixelWidth + dx * (PixelWidth / WorldWidth));
        v.y = Mathf.RoundToInt(0.5f * PixelHeight + dy * (PixelHeight / WorldHeight));


        return v;
    }

    void SubstractHole(CircleCollider2D objectCollider)
    {
        Vector2Int v = WorldToPixel(objectCollider.bounds.center);

        int radius = Mathf.RoundToInt(objectCollider.bounds.size.x * PixelWidth / WorldWidth);
        int px, nx, py, ny, d;

        for(int i = 0; i<= radius; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(radius * radius - i * radius));
            for(int j = 0; j<= d; j++)
            {
                px = v.x + i;
                nx = v.x - i;
                py = v.y + j;
                ny = v.y - j;

                cloneTexture.SetPixel(px, py, Color.clear);
                cloneTexture.SetPixel(nx, py, Color.clear);
                cloneTexture.SetPixel(px, ny, Color.clear);
                cloneTexture.SetPixel(nx, ny, Color.clear);
            }
        }
        cloneTexture.Apply();
        UpdateTexture();

        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D objectCollider)
    {
        if(objectCollider.transform.tag != "Bullet")
        {
            return;
        }
        if(!objectCollider.GetComponent<CircleCollider2D>())
        {
            return;
        }

        SubstractHole(objectCollider.GetComponent<CircleCollider2D>());
        Destroy(objectCollider.gameObject, 0.1f);
    }
}
