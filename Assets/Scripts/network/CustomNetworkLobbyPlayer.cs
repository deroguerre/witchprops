using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkLobbyPlayer : NetworkLobbyPlayer
{
    private NetworkLobbyManager GetLobbyManager()
    {
        return NetworkManager.singleton as NetworkLobbyManager;
    }
    
    private void OnGUI()
    {
        if (!ShowLobbyGUI)
            return;
        NetworkLobbyManager lobbyManager = this.GetLobbyManager();

        if ((bool) (UnityEngine.Object) lobbyManager && (!lobbyManager.showLobbyGUI || SceneManager.GetSceneAt(0).name != lobbyManager.lobbyScene))
            return;
        Rect position = new Rect((float) (100 + (int) slot * 100), 200f, 100f, 20f);
        if (this.isLocalPlayer)
        {
            string text = !readyToBegin ? "(Pas Prêt)" : "(Prêt)";
            GUI.Label(position, text);
            if (readyToBegin)
            {
                position.y += 25f;
                if (!GUI.Button(position, "STOP"))
                    return;
                this.SendNotReadyToBeginMessage();
            }
            else
            {
                position.y += 25f;
                if (GUI.Button(position, "COMMENCER"))
                    this.SendReadyToBeginMessage();
                position.y += 25f;
                // if (GUI.Button(position, "Remove"))
                //     ClientScene.RemovePlayer(this.GetComponent<NetworkIdentity>().playerControllerId);
            }
        }
        else
        {
            GUI.Label(position, "Joueur [" + (object) this.netId + "]");
            position.y += 25f;
            GUI.Label(position, "Prêt [" + (object) readyToBegin + "]");
        }
    }
}
