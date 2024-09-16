using GameNetcodeStuff;
using StaticNetcodeLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace DrunkCompany.Scripts
{
    [StaticNetcode]
    class NetworkManager : MonoBehaviour
    {
        internal static List<PlayerControllerB> currentPlayers = new List<PlayerControllerB>();

        internal static List<PlayerControllerB> deadPlayers = new List<PlayerControllerB>();


        [ClientRpc]
        public static void PingClientRpc(string name)
        {
			//StartOfRound instance = StartOfRound.Instance;
			DrunkCompany.Logger.LogMessage($"this function has ran {name}");
			DrunkCompany.Logger.LogMessage($"This the current list {currentPlayers}");
            HUDManager.Instance.DisplayTip("The captain", $"{name} has be designated.");



        }

        public static void DetermineCaptain()
        {
			DrunkCompany.Logger.LogMessage("Determine Captain function was ran");

            currentPlayers.Clear();

            foreach (PlayerControllerB player in StartOfRound.Instance.allPlayerScripts)
            {
                if (!player.isPlayerControlled || player.isPlayerDead) { continue; }
                {
					DrunkCompany.Logger.LogMessage($"Adding Player {player.playerUsername} to currentPlayers");
                    currentPlayers.Add(player);
                }
            }

            var Captain = currentPlayers[Random.Range(0, currentPlayers.Count())];

            PingClientRpc(Captain.playerUsername);


        }

        [ClientRpc]
        public static void DeathWarningClientRpc(int playerId)
        {
            PlayerControllerB firstDeath = currentPlayers[playerId];
            if (deadPlayers.Count() > 0)
            {
                HUDManager.Instance.DisplayTip("Someone has died", "This is their name " + firstDeath.playerUsername, isWarning: true);
                deadPlayers.Add(firstDeath);
            }
            //StartOfRound instance = StartOfRound.Instance;


        }

        //[ClientRpc]
        //public static void randomSuit(int suitID)
        //{
        //	//StartOfRound instance = StartOfRound.Instance;
        //	TestAnnouncer.Logger.LogMessage($"this function has ran {suitID}");
        //	UnlockableSuit suit;

        //	foreach (PlayerControllerB player in currentPlayers)
        //	{
        //		if (!player.isPlayerControlled || player.isPlayerDead) { continue; }
        //		{
        //			SwitchSuitForPlayer(player, suitID);
        //			suit.Sw

        //		}
        //	}

        //}

    }
}
