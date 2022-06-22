using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventSystem;
using EventSystem.Data;

public class PlayerController : MonoBehaviour
{
    public USAMapController mapController;

    [SerializeField, Tooltip("seconds from one node to another")]
    private float m_moveSpeed = 2.0f;

    private List<string> m_curMovementRoute = new List<string>();
    private List<Vector3> m_curMovementRoutePos = new List<Vector3>();

    private bool m_bInMove = false;
    private float m_moveTime = 0.0f;

    private string m_curState;
    private string m_targetState;

    // Start is called before the first frame update
    void Start()
    {
        m_curState = PlayerPrefs.GetString("InitialState", "Utah");
        m_targetState = "";
        mapController.OnStateSelectedOrUnselected(m_curState, true);

        transform.position = mapController.GetStateNodePos(m_curState);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bInMove && Input.GetKeyDown("space"))
        {
            if(mapController.FindShortestPath(m_curState, m_targetState, out m_curMovementRoute))
            {
                m_bInMove = true;
                m_moveTime = 0.0f;

                m_curMovementRoutePos = mapController.GetCurRouteNodesPos(m_curMovementRoute);

                StartCoroutine(MoveToTargetState());
            }
        }

        if (!m_bInMove && Input.GetMouseButtonDown(1)) // use right mouse button to select the target state
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                string state = hit.transform.name.Trim('1').Trim('_');
                if (mapController.CheckStateExist(state))
                {
                    EventManager.TriggerEvent(CustomEventName.EVENT_GAME_STATE_SELECTED, new StateSelectedData { bStartPoint = false, bEndPoint = true, stateName = state });

                    if (state != m_targetState && state != m_curState)
                    {
                        mapController.OnStateSelectedOrUnselected(m_targetState, false); // select a new target
                        m_targetState = state;
                        mapController.OnStateSelectedOrUnselected(m_targetState, true);

                        if (mapController.FindShortestPath(m_curState, m_targetState, out m_curMovementRoute))
                        {
                            mapController.RenderThePath(m_curMovementRoute);
                        }
                    }
                }
            }
        }
    }

    IEnumerator MoveToTargetState()
    {
        while (m_bInMove)
        {
            m_moveTime += Time.deltaTime;

            int maxNode = m_curMovementRoute.Count - 1;
            int prevNode = Mathf.Min((int)(m_moveTime / m_moveSpeed), maxNode);
            int targetNode = Mathf.Min(prevNode + 1, maxNode);

            float t = m_moveTime - prevNode * m_moveSpeed;

            if (m_moveTime >= maxNode * m_moveSpeed)// we arrived the destination
            {
                EventManager.TriggerEvent(CustomEventName.EVENT_UI_SHOW_POPUP,
                    new PopUpMessageData
                    {
                        closeAction = null,
                        confirmAction = null,
                        title = "Congratulations",
                        message = string.Format("We arrived new state: {0}.", m_targetState),
                        layer = 0
                    });

                EventManager.TriggerEvent(CustomEventName.EVENT_GAME_PLAYER_MOVEMENT_COMPLETED, new PlayerLocationData { stateName = m_targetState });
                mapController.OnStateSelectedOrUnselected(m_curState, false);
                mapController.ClearThePath();

                m_curState = m_targetState;
                m_targetState = "";
                PlayerPrefs.SetString("InitialState", m_curState);
                m_bInMove = false;
                break;
            }
            else
            {
                transform.position = Vector3.Lerp(m_curMovementRoutePos[prevNode], m_curMovementRoutePos[targetNode], t / m_moveSpeed);
                yield return null;
            }
        }
    }
}
