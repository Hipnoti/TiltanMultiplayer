using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Managers
{
    public enum GameSyncedState {OnTheWay, Arrived, Decision, ReadyToOrder, Order, Party}
    public class GameSyncManager : MonoBehaviour
    {
        private const string GAME_STATE_SYNC_KEY = nameof(GAME_STATE_SYNC_KEY);


        [SerializeField] private GameSyncedState debugGameSyncedState;
        [SerializeField] private GameSyncedState targetPlayersGameSyncedState;
        public void SetLocalPlayerState(GameSyncedState gameSyncedState)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add(GAME_STATE_SYNC_KEY, gameSyncedState);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }

        public bool AreAllPlayersInCertainPhase(GameSyncedState gameSyncedState)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.ContainsKey(GAME_STATE_SYNC_KEY))
                {
                    GameSyncedState currentPlayerGameSyncState = 
                        (GameSyncedState)player.CustomProperties[GAME_STATE_SYNC_KEY];

                    if (currentPlayerGameSyncState != gameSyncedState)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void Update()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(GAME_STATE_SYNC_KEY))
            {
                GameSyncedState currentPlayerGameSyncState = 
                    (GameSyncedState)PhotonNetwork.LocalPlayer.CustomProperties[GAME_STATE_SYNC_KEY];
                Debug.Log("Current game state for local player is " + currentPlayerGameSyncState);
            }
            
            Debug.Log(AreAllPlayersInCertainPhase(targetPlayersGameSyncedState));
        }

        [ContextMenu("Set Local Player In State Debug")]
        void SetLocalPlayerDebugState()
        {
            SetLocalPlayerState(debugGameSyncedState);
        }


        [ContextMenu("Get out!")]
        void GetOutOfHome()
        {
            SetLocalPlayerState(GameSyncedState.OnTheWay);
            Debug.Log("on the way");
            //Animation and other stuff
            StartCoroutine(ArrivedCoRoutine());
        }

        IEnumerator ArrivedCoRoutine()
        {
            Debug.Log("checking everyone is on the way");
            yield return new WaitUntil(() => AreAllPlayersInCertainPhase(GameSyncedState.OnTheWay));
            Debug.Log("want to be there already, checking with others");
        //    SetLocalPlayerState(GameSyncedState.Arrived);
            yield return new WaitUntil(() => AreAllPlayersInCertainPhase(GameSyncedState.Arrived));
            Debug.Log("I'm there!!!");
        }
        
    }
    
}