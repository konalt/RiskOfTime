using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject instance;
    public float blockHeight = 1f;
    public Vector2 blockYRange = new Vector2(1f, 3f);
    public Vector2 blockWidthRange = new Vector2(5f, 10f);
    public Vector2 gapRange = new Vector2(2f, 7f);

    private float RangedRandom(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    private void CreateSingleBlock(Vector3 info)
    {
        GameObject block = Instantiate(instance);
        block.transform.position = new Vector3(info.x, info.y, transform.position.z);
        block.transform.localScale = new Vector3(info.z, blockHeight, 1f);
        block.transform.parent = transform;
    }

    private void GenerateMultipleBlocks(int count)
    {
        float x = 0;
        float y = 0;
        int n = 3;
        for (int i = 0; i < count; i++)
        {
            float width = RangedRandom(blockWidthRange);
            CreateSingleBlock(new Vector3(transform.position.x + x, transform.position.y + RangedRandom(blockYRange), width));
            x += width;
            x += RangedRandom(blockWidthRange);
            if (i % n == 0)
            {
                x = 0;
                y += RangedRandom(blockYRange);
            }
        }
    }

    private void Start()
    {
        GenerateMultipleBlocks(24);
    }
}
