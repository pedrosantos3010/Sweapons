﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject _go = new GameObject("InputManager");
                _instance = _go.AddComponent<PlayerManager>() as PlayerManager;
            }

            return _instance;
        }
    }

    [SerializeField] int playerCount = 2;
    [SerializeField] Color[] playerColors;

    [SerializeField] Weapon[] weapons;
    [SerializeField] Player playerPrefab;

    Dictionary<int, int> playersIDtoIndex = new Dictionary<int, int>();

    int[] weaponPlayer;
    Player[] players;

    GameObject[] spawnPoints;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        SpawnAllPlayers();
    }

    public void SetSpawnPosition(Transform _transform)
    {
        _transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    void SpawnAllPlayers()
    {
        weaponPlayer = new int[playerCount];
        players = new Player[playerCount];
        List<int> allPositions = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            allPositions.Add(i);
        }

        for (int i = 0; i < playerCount; i++)
        {
            int instanciationPosition = allPositions[Random.Range(0, allPositions.Count)];
            allPositions.Remove(instanciationPosition);

            players[i] = Instantiate(playerPrefab, spawnPoints[instanciationPosition].transform.position, Quaternion.identity) as Player;
            players[i].SetPlayerNumber(i, playerColors[i]);

            int _weaponPlayerNumber = i == 0 ? 1 : 0;
            Weapon _weapon = Instantiate(weapons[0]) as Weapon;
            _weapon.SetPlayerNumber(_weaponPlayerNumber, playerColors[_weaponPlayerNumber]);
            players[i].SetWeapon(_weapon.gameObject);
        }
    }

    public Color GetPlayerColorByIndex (int playerIndex)
    {
        return playerColors[playerIndex];
    }

    public Color GetPlayerColorByID (int playerID)
    {
        return GetPlayerColorByIndex(playersIDtoIndex[playerID]);
    }

    public void ResetPlayersID ()
    {
        playersIDtoIndex.Clear();
    }

    public void RemovePlayerID (int actorNumber)
    {
        playersIDtoIndex.Remove(actorNumber);
    }

    public void AddPlayerId (int actorNumber)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!playersIDtoIndex.ContainsValue(i))
            {
                playersIDtoIndex.Add(actorNumber, i);
                return;
            }
        }
    }

}
