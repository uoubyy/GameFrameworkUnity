using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventSystem;
using EventSystem.Data;
using Utility;

using GameData;

using Newtonsoft.Json;

public class USAMapController : MonoBehaviour
{
    // TODO:
    // I want to separate state into a single class
    // decouple the state with the map controller

    [Serializable]
    public struct StateInfo
    {
        // has too much info encapsulate in this struct
        // maybe split route nodes into a separate layer
        public string stateName;
        public GameObject stateLand;
        public GameObject stateBoard;
        public GameObject routeNode;

        public StateInfo(String _name, GameObject _land, GameObject _board, GameObject _node)
        {
            this.stateName = _name;
            this.stateBoard = _board;
            this.stateLand = _land;
            this.routeNode = _node;
        }
    }

    [SerializeField]
    private Transform m_boards;

    [SerializeField]
    private Transform m_states;

    [SerializeField]
    private Transform m_routesPanel;

    [SerializeField]
    private LineRenderer m_lineRender;

#if DEBUG
    public List<StateInfo> m_usaMapList = new List<StateInfo>();
#endif

    private Dictionary<string, StateInfo> m_usaMap = new Dictionary<string, StateInfo>();

    private Dictionary<string, string[]> m_allStaesAdjacents = new Dictionary<string, string[]>();

    void Awake()
    {
        foreach (Transform child in m_states.transform)
        {
            string stateName = child.name.Replace("_layer", "");
            GameObject stateBoard = m_boards.Find(stateName)?.gameObject;
            GameObject routeNode = m_routesPanel.Find(stateName.Trim('_'))?.gameObject;

#if DEBUG
            m_usaMapList.Add(new StateInfo(stateName.Trim('_'), child.gameObject, stateBoard, routeNode));
#endif

            m_usaMap[stateName.Trim('_')] = new StateInfo(stateName.Trim('_'), child.gameObject, stateBoard, routeNode);
        }

        TextAsset jsonTextFile = Resources.Load<TextAsset>("Text/AdjacentStates");
        m_allStaesAdjacents = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonTextFile.text);
    }

    void OnEnable()
    {
        //EventManager.StartListening(CustomEventName.EVENT_GAME_PLAYER_MOVEMENT_COMPLETED, OnPlayerMoved);
    }

    void OnDisable()
    {
        //EventManager.StopListening(CustomEventName.EVENT_GAME_PLAYER_MOVEMENT_COMPLETED, OnPlayerMoved);
    }


    public void OnStateSelectedOrUnselected(string state, bool value)
    {
        if (m_usaMap.ContainsKey(state))
            DisplayStateIcon(state, value);
    }

    void DisplayStateIcon(string state, bool value)
    {
        m_usaMap[state].routeNode.SetActive(value);
    }

    // Find the shortest path from one node to another
    // BFS
    public bool FindShortestPath(string origin,string destination, out List<string> routes)
    {
        routes = new List<string>();
        var frontier = new Queue<string>();
        frontier.Enqueue(origin);
        Dictionary<string, string> prevNodes = new Dictionary<string, string>();

        while(frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == destination)
                break;

            if (!m_allStaesAdjacents.ContainsKey(current))
                continue;

            foreach(var next in m_allStaesAdjacents[current])
            {
                if(!prevNodes.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    prevNodes[next] = current;
                }
            }
        }

        routes.Clear();
        string node = destination;
        while(node != origin)
        {
            // no father node find
            // cannot reach
            if(!prevNodes.ContainsKey(node))
            {
                routes.Clear();
                Debug.Log(string.Format("Can not find any path from {0} to {1}, check the map info json!", origin, destination));
                return false;
            }

            routes.Add(node);
            node = prevNodes[node];
        }

        routes.Add(origin);

        routes.Reverse();

        return true;
    }

    // render the route path
    public void RenderThePath(in List<string> routeNodes)
    {
        m_lineRender.positionCount = routeNodes.Count;

        for (int i = 0; i < routeNodes.Count; ++i)
        {
            m_lineRender.SetPosition(i, m_usaMap[routeNodes[i]].routeNode.transform.position);
        }
    }

    //
    public void ClearThePath()
    {
        m_lineRender.positionCount = 0;
    }

    // find all nodes can arrive from on states within limited steps
    public Dictionary<int, List<string>> FindAllNodesCanArrive(string origin, int steps)
    {
        Dictionary<int, List<string>> availableNodes = new Dictionary<int, List<string>>();

        var curFrontier = new Queue<string>();
        var nextFrontier = new Queue<string>();
        curFrontier.Enqueue(origin);

        int level = 1;
        while(curFrontier.Count > 0 && level <= steps)
        {
            var current = curFrontier.Dequeue();

            // make sure this node has adjacent states info
            if (m_allStaesAdjacents.ContainsKey(current))
            {
                foreach (var next in m_allStaesAdjacents[current])
                {
                    if (!availableNodes.ContainsKey(level))
                        availableNodes.Add(level, new List<string>());

                    if (!availableNodes[level].Contains(next))
                    {
                        availableNodes[level].Add(next);
                        nextFrontier.Enqueue(next);
                    }
                }
            }

            if (curFrontier.Count == 0)
            {
                curFrontier = nextFrontier;
                nextFrontier.Clear();
            }

        }

        return availableNodes;
    }

    public bool CheckStateExist(string state)
    {
        return m_usaMap.ContainsKey(state);
    }

    public Vector3 GetStateNodePos(string state)
    {
        return m_usaMap[state].routeNode.transform.position;
    }

    public List<Vector3> GetCurRouteNodesPos(List<string> m_routeNodes)
    {
        List<Vector3> pos = new List<Vector3>();

        if(m_routeNodes.Count > 0)
        {
            foreach(string node in m_routeNodes)
            {
                pos.Add(m_usaMap[node].routeNode.transform.position);
            }
        }

        return pos;
    }
}
