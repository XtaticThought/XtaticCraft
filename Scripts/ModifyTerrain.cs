
using UnityEngine;

public class ModifyTerrain : Singleton<ModifyTerrain>
{
    private World world;
    private GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        world = gameObject.GetComponent("World") as World;
        character = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        LoadChunks(GameObject.FindGameObjectWithTag("Player").transform.position, 50, 60);
    }

    public void DestroyBlock(float range, byte block)
    {
        Ray ray = new Ray(character.transform.position, character.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                DestroyBlockAt(hit, block);
            }
        }

    }

    public void AddBlock(float range, byte block)
    {
        Ray ray = new Ray(character.transform.position, character.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.distance < range)
            {
                AddBlockAt(hit, block);
            }
        }
    }

    public void DestroyBlockAt(RaycastHit hit, byte block)
    {
        Vector3 position = hit.point;
        position += (hit.normal * -0.5f);
        SetBlockAt(position, block);
    }

    public void AddBlockAt(RaycastHit hit, byte block)
    {
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);
        SetBlockAt(position, block);
    }

    public void SetBlockAt(Vector3 position, byte block)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        world.WorldData[x, y, z] = block;
        UpdateChunkAt(x, y, z);
    }

    public void UpdateChunkAt(int x, int y, int z)
    {
        int updateX = Mathf.FloorToInt(x / world.ChunkSize);
        int updateY = Mathf.FloorToInt(y / world.ChunkSize);
        int updateZ = Mathf.FloorToInt(z / world.ChunkSize);

        world.Chunks[updateX, updateY, updateZ].IsUpdate = true;


        if (x - (world.ChunkSize * updateX) == 0 && updateX != 0)
        {
            world.Chunks[updateX - 1, updateY, updateZ].IsUpdate = true;
        }


        if (x - (world.ChunkSize * updateX) == 15 && updateX != world.Chunks.GetLength(0) - 1)
        {
            world.Chunks[updateX + 1, updateY, updateZ].IsUpdate = true;
        }

        if (y - (world.ChunkSize * updateY) == 0 && updateY != 0)
        {
            world.Chunks[updateX , updateY - 1, updateZ].IsUpdate = true;
        }


        if (y - (world.ChunkSize * updateY) == 15 && updateY != world.Chunks.GetLength(1) - 1)
        {
            world.Chunks[updateX, updateY + 1, updateZ].IsUpdate = true;
        }

        if (z - (world.ChunkSize * updateZ) == 0 && updateZ != 0)
        {
            world.Chunks[updateX, updateY, updateZ - 1].IsUpdate = true;
        }


        if (z - (world.ChunkSize * updateZ) == 15 && updateZ != world.Chunks.GetLength(2) - 1)
        {
            world.Chunks[updateX, updateY, updateZ + 1].IsUpdate = true;
        }

    }

    public void LoadChunks(Vector3 playerPos, float distToLoad, float distToDestroy)
    {
        for (int x = 0; x < world.Chunks.GetLength(0); x++)
        {
            for (int z = 0; z < world.Chunks.GetLength(2); z++)
            {
                float dist = Vector2.Distance(new Vector2(x * world.ChunkSize, z * world.ChunkSize), new Vector2(playerPos.x, playerPos.z));

                if (dist < distToLoad)
                {
                    if (world.Chunks[x, 0, z] == null)
                    {
                        world.GenerateChunk(x, z);
                    }
                }
                else if (dist > distToDestroy)
                {
                    if(world.Chunks[x, 0, z] != null)
                    {
                        world.DestroyChunk(x, z);
                    }
                }
            }
        }
            
    }
}
