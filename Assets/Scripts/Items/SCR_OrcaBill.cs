using UnityEngine;
using UnityEngine.Networking;

public class SCR_OrcaBill : NetworkBehaviour {

    public GameObject instancer;
    public float whaleDuration = 5;
    SCR_PlayerTempStats next_dest;
    private float timer = 0;
    private float distanceToTarget = 5;
    public void Start2()
    {
        next_dest = instancer.GetComponent<SCR_PlayerTempStats>();

        ChangePingoStatus(false);
    }

    void ChangePingoStatus(bool _status)
    {
        instancer.GetComponent<SCR_CharacterMotor_Net>().usingItem = !_status;
        instancer.GetComponent<CapsuleCollider>().enabled = _status;
        instancer.GetComponent<SCR_CharacterMotor>().activeModel.gameObject.SetActive(_status);
        instancer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        instancer.GetComponent<Rigidbody>().useGravity = _status;
        instancer.GetComponent<SCR_CharacterMotor>().currentSpeed = 0;
    }

    void Update()
    {
        if (!instancer)
            return;

        timer += Time.deltaTime;
        if (timer < whaleDuration)
            distanceToTarget = 0;
        else
            distanceToTarget = 5.0f;

        if (Vector3.Distance(transform.position, next_dest.nextTarget.transform.position) > distanceToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, next_dest.nextTarget.transform.position, Time.deltaTime * 20);
            transform.rotation = Quaternion.LookRotation(next_dest.nextTarget.transform.position - transform.position);
            instancer.GetComponent<Rigidbody>().position = transform.position;
            instancer.transform.position = transform.position;
        }
        else
        {
            instancer.GetComponent<Rigidbody>().position = transform.position;
            instancer.transform.rotation = Quaternion.LookRotation(next_dest.nextTarget.transform.position - instancer.transform.position);
            ChangePingoStatus(true);
            Invoke("DestroyMe", 0.2f);
        }
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetInstancer(GameObject _netId)
    {
        Rpc_SetMyObject(_netId.GetComponent<NetworkIdentity>().netId);
    }
    [Command]
    void Cmd_SetInstancer(NetworkInstanceId _netId)
    {
        instancer = NetworkServer.FindLocalObject(_netId).gameObject;
        Start2();

    }
    [ClientRpc]
    public void Rpc_SetMyObject(NetworkInstanceId _netId)
    {
        instancer = ClientScene.FindLocalObject(_netId);
        Start2();

    }
}