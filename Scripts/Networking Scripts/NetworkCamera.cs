using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimonHa.Core.Singletons;
public class NetworkCamera : Singleton<NetworkCamera>
{
    public void FollowPlayer(Transform player_transform)
    {
        transform.position = new Vector3(player_transform.position.x, player_transform.position.y, -10);
    }
}
