using GameNetcodeStuff;
using StaticNetcodeLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using DrunkCompany;

namespace DrunkCompany.Scripts
{
    [StaticNetcode]
    class NetworkManager : MonoBehaviour
    {
		//Used to create a list of players in the lobby
        internal static List<PlayerControllerB> currentPlayers = new List<PlayerControllerB>();

		//Used to keep track of the first death player
        internal static List<string> deadPlayers = new List<string>();


		//Method to send a message to all players in their HUD
        [ClientRpc]
        public static void ShowHudMessageClientRpc(string name)
        {
			DrunkCompany.Logger.LogMessage($"this function has ran {name}");
			DrunkCompany.Logger.LogMessage($"This the current list {currentPlayers}");
            HUDManager.Instance.DisplayTip("The captain", $"{name} has be designated.");

        }

		//Method to randomly select a captain from currentPlayers List
        public static void DetermineCaptain()
        {
			DrunkCompany.Logger.LogMessage("Determine Captain function was ran");
            deadPlayers.Clear();
            currentPlayers.Clear();

			//Loops through players in the lobby and adds them to currentPlayers List
			PlayerControllerB[] allPlayerScripts = StartOfRound.Instance.allPlayerScripts;
			foreach (PlayerControllerB player in allPlayerScripts)
            {
				if (!player.isPlayerControlled || player.isPlayerDead) { continue; }
                {
					DrunkCompany.Logger.LogMessage($"Adding Player {player} to currentPlayers");
					DrunkCompany.Logger.LogMessage($"Adding Player with client ID {player.actualClientId} to currentPlayers");
					DrunkCompany.Logger.LogMessage($"Adding Player with username {player.playerUsername} to currentPlayers");
					//TeamSuitOrganizerClientRpc(player, 1);
					//TeamSuitOrganizerAllClientRpc(1);
					currentPlayers.Add(player);
                }
            }
			
			
            PlayerControllerB Captain = currentPlayers[Random.Range(0, currentPlayers.Count())];

			ShowHudMessageClientRpc(Captain.playerUsername);


        }

		//Method to send first death warning HUD message to all players
        [ClientRpc]
        public static void DeathWarningClientRpc(int playerId)
        {
            PlayerControllerB firstDeath = currentPlayers[playerId];
			DrunkCompany.Logger.LogMessage($"The amount of dead players currently {deadPlayers.Count()}");
			DrunkCompany.Logger.LogMessage($"This is the playerId {firstDeath.playerUsername}");
			DrunkCompany.Logger.LogMessage($"This is the playerId {currentPlayers[playerId]}");
			if (deadPlayers.Count() == 0)
            {
                HUDManager.Instance.DisplayTip("Someone has died", "This is their name " + firstDeath.playerUsername, isWarning: true);
                deadPlayers.Add(firstDeath.playerUsername);
            }
        }

		//Test method to switch suit for specific player
		//Currently breaks game
		[ClientRpc]
		public static void TeamSuitOrganizerClientRpc(PlayerControllerB player, int suitID)
		{
			DrunkCompany.Logger.LogMessage("The team organizer function got ran");
			UnlockableSuit.SwitchSuitForPlayer(player, suitID);

			

		}

		//Test method to switch suit for all players
		//Currently works, but need to fix it to change for individual players
		[ClientRpc]
		public static void TeamSuitOrganizerAllClientRpc(int suitID)
		{
			DrunkCompany.Logger.LogMessage("The team organizer function got ran for all players");
			UnlockableSuit.SwitchSuitForAllPlayers(suitID);



		}


	}
}
