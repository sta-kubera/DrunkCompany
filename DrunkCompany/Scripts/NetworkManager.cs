using GameNetcodeStuff;
using StaticNetcodeLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using System;
using DrunkCompany;
using BepInEx;
using Unity.Services.Authentication.Internal;



namespace DrunkCompany.Scripts
{
    [StaticNetcode]
    class NetworkManager : MonoBehaviour
    {
		//Used to create a list of players in the lobby
        internal static List<PlayerControllerB> currentPlayers = new List<PlayerControllerB>();
		
		//Suit ID:
		//Red = 32
		//Cyan = 28
		//Dandelion = 29
		//Orange = 0
		//Neon Green = 30
		//Pink = 31
		//white = 33
		private static List<int> colors = new List<int> { 28, 29, 0, 30, 31, 33 };

		//Used to keep track of the first death player
		internal static List<string> deadPlayers = new List<string>();

		public static List<string> PosterFolders = new List<string>();


		//Method to send a message to all players in their HUD
		[ClientRpc]
        public static void ShowHudMessageClientRpc(string name)
        {
			DrunkCompany.Logger.LogMessage($"this function has ran {name}");
			DrunkCompany.Logger.LogMessage($"This the current list {currentPlayers}");
			HUDManager.Instance.DisplayTip("The captain", $"{name} has be designated.");
		}

		//Method to randomly select a captain from currentPlayers List
        public static void DetermineTeams()
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

					currentPlayers.Add(player);
                }
            }

			
			currentPlayers = Shuffle(currentPlayers);
			// Case 1: When player count is 4 or fewer
			PlayerControllerB captain = currentPlayers[0];  // First player becomes captain
			ShowHudMessageClientRpc(captain.playerUsername);
			SetPlayerSuit(captain, 32);
			List<PlayerControllerB> remainingPlayers = currentPlayers.Skip(1).ToList();  // Remaining players in one group



			// Case 2: More than 4 players, regular group splitting logic

			// Split players into groups of 2
			DrunkCompany.Logger.LogMessage($"This is the remaining players before groups  {remainingPlayers.Count}");
			List<List<PlayerControllerB>> groups = GetGroupList(remainingPlayers, 2);

			if (groups[0].Count == 1)
			{
				//do nothing
			}
			else
			{
				for (int i = 0; i < groups.Count; i++)
				{

					if (groups[i].Count == 1 && i > 0)
					{
						PlayerControllerB leftoverPlayer = groups[i][0];
						groups.Remove(groups[i]);
						if (i - 1 >= 0)
						{
							groups[i - 1].Add(leftoverPlayer);
						}
						break;
					}
				}
			}



			// Assign colors to each group 
			DrunkCompany.Logger.LogMessage($"This is the player before color function:  {groups.Count}");
			for (int i = 0; i < groups.Count; i++)
			{
				
				int color = colors[i % colors.Count];
				foreach (PlayerControllerB player in groups[i]) {
					DrunkCompany.Logger.LogMessage($"Group {color}: {string.Join(", ", groups[i])}");
					DrunkCompany.Logger.LogMessage($"This is the player:  {player}");
					DrunkCompany.Logger.LogMessage($"This is the color for suit:  {color}");
					SetPlayerSuit(player, color);
				}	
			}

				// Print the captain, if there is one
			if (captain == null)
			{
				DrunkCompany.Logger.LogMessage($"Captain is: {captain}");
			}
			else
			{
				DrunkCompany.Logger.LogMessage("All players are evenly grouped.");
			}

        }
		// Shuffle the list of players using LINQ and a random number generator
		private static List<PlayerControllerB> Shuffle(List<PlayerControllerB> list)
		{
			System.Random rng = new System.Random();
			return list.OrderBy(x => rng.Next()).ToList();
		}

		// Splits the list into groups of a specified size using LINQ
		private static List<List<PlayerControllerB>> GetGroupList(List<PlayerControllerB> list, int numInGroup)
		{
			if (list.Count <= numInGroup)
			{
				return new List<List<PlayerControllerB>> { list };
			}
				return list.Select((player, index) => new { player, index })
						   .GroupBy(x => x.index / numInGroup)
						   .Select(group => group.Select(x => x.player).ToList())
						   .ToList();
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

		public static void SetPlayerSuit(PlayerControllerB player, int suitID)
		{

			DrunkCompany.Logger.LogMessage($"This is the playerId {player}");
			DrunkCompany.Logger.LogMessage($"This is the suitID {suitID}");
			Material material = StartOfRound.Instance.unlockablesList.unlockables[suitID].suitMaterial;
			player.thisPlayerModel.material = material;
			player.thisPlayerModelLOD1.material = material;
			player.thisPlayerModelLOD2.material = material;
			player.thisPlayerModelArms.material = material;
			player.currentSuitID = suitID;
		}
	}
}
