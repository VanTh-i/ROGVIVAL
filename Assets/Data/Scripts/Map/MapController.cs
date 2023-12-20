using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : ThaiBehaviour
{
    [SerializeField] protected List<GameObject> terrainChunks;
    [SerializeField] protected float checkerRadius;
    [SerializeField] protected LayerMask terrainMask;
    public GameObject currentChunk;
    protected Transform poolHolder;
    protected Vector3 playerLastPosition;

    protected const string Up = "Up", Down = "Down", Left = "Left", Right = "Right", RightUp = "Right Up", RightDown = "Right Down", LeftUp = "Left Up", LeftDown = "Left Down";

    [Header("---Optimization---")]
    [SerializeField] protected List<GameObject> spawnerChunk;
    protected Transform player;
    [SerializeField] protected int maxOpDist;
    private int opDist;
    private float optCooldown;
    [SerializeField] protected float optCooldownDur;

    protected override void LoadComponents()
    {
        if (poolHolder != null)
        {
            return;
        }
        poolHolder = transform.Find("Pool");

        player = FindPlayer.GetPlayer();
        if (player == null)
        {
            Debug.LogError("Can not find player");
        }
    }

    private void Start()
    {
        playerLastPosition = player.transform.position;
    }

    private void Update()
    {
        ChunkCheker();
        DeSpawnChunk();
    }

    private void ChunkCheker()
    {
        if (!currentChunk)
        {
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);
        CheckAndSpawnChunk(directionName);
        if (directionName.Contains(Up))
        {
            CheckAndSpawnChunk(Up);
        }
        if (directionName.Contains(Down))
        {
            CheckAndSpawnChunk(Down);
        }
        if (directionName.Contains(Left))
        {
            CheckAndSpawnChunk(Left);
        }
        if (directionName.Contains(Right))
        {
            CheckAndSpawnChunk(Right);
        }
    }

    private void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }
    private string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;
        if (MathF.Abs(direction.x) > MathF.Abs(direction.y))
        {
            if (direction.y > 0.5f)
            {
                return direction.x > 0 ? RightUp : LeftUp;
            }
            else if (direction.y < -0.5f)
            {
                return direction.x > 0 ? RightDown : LeftDown;
            }
            else
            {
                return direction.x > 0 ? Right : Left;
            }
        }
        else
        {
            if (direction.x > 0.5f)
            {
                return direction.y > 0 ? RightUp : RightDown;
            }
            else if (direction.x < -0.5f)
            {
                return direction.y > 0 ? LeftUp : LeftDown;
            }
            else
            {
                return direction.y > 0 ? Up : Down;
            }
        }
    }

    private void SpawnChunk(Vector3 spawnPos)
    {
        int random = Random.Range(0, terrainChunks.Count);
        GameObject chunk = Instantiate(terrainChunks[random], spawnPos, Quaternion.identity);
        chunk.transform.parent = poolHolder;
        spawnerChunk.Add(chunk);
    }

    private void DeSpawnChunk()
    {
        optCooldown -= Time.deltaTime;
        if (optCooldown <= 0f)
        {
            optCooldown = optCooldownDur;
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnerChunk)
        {
            opDist = (int)Vector3.Distance(player.position, chunk.transform.position);

            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }

}
