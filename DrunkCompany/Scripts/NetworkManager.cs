using GameNetcodeStuff;
using StaticNetcodeLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using System;
using DrunkCompany;
using BepInEx;



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
					Material material = StartOfRound.Instance.unlockablesList.unlockables[30].suitMaterial;
					player.thisPlayerModel.material = material;
					player.thisPlayerModelLOD1.material = material;
					player.thisPlayerModelLOD2.material = material;
					player.thisPlayerModelArms.material = material;
					player.currentSuitID = 30;
					currentPlayers.Add(player);
                }
            }

			
			currentPlayers = Shuffle(currentPlayers);
			// Case 1: When player count is 4 or fewer
			if (currentPlayers.Count <= 4)
			{
				PlayerControllerB captain = currentPlayers[0];  // First player becomes captain
				List<PlayerControllerB> group = currentPlayers.Skip(1).ToList();  // Remaining players in one group

				Console.WriteLine($"Captain is: {captain}");

				if (group.Count > 0)
				{
					// Assign one color to the remaining group
					int color = colors[0];  // Assign first color
					Console.WriteLine($"Group {color}: {string.Join(", ", group)}");
				}
				else
				{
					Console.WriteLine("No players to group.");
				}

			}
			else
			{
				// Case 2: More than 4 players, regular group splitting logic

				// Split players into groups of 2
				List<List<PlayerControllerB>> groups = GetGroupList(currentPlayers, 2);

				// Check for single-player group and promote them to captain
				PlayerControllerB captain = null;
				foreach (var group in groups)
				{
					if (group.Count == 1)
					{
						captain = group[0];
						groups.Remove(group);
						break;
					}
				}

				// Assign colors to each group 
				Console.WriteLine("Groups with assigned colors:");
				for (int i = 0; i < groups.Count; i++)
				{
					int color = colors[i % colors.Count];
					Console.WriteLine($"Group {color}: {string.Join(", ", groups[i])}");
				}

				// Print the captain, if there is one
				if (captain == null)
				{
					Console.WriteLine($"Captain is: {captain}");
				}
				else
				{
					Console.WriteLine("All players are evenly grouped.");
				}
			}

			//List<PlayerControllerB> players = GetTeamList();
			//PlayerControllerB Captain = currentPlayers[UnityEngine.Random.Range(0, currentPlayers.Count())];


			ShowHudMessageClientRpc(Captain.playerUsername);


        }
		// Shuffle the list of players using LINQ and a random number generator
		private static List<PlayerControllerB> Shuffle(List<PlayerControllerB> list)
		{
			System.Random rng = new System.Random();
			return list.OrderBy(x => rng.Next()).ToList();
		}

		//// Returns a list of players with random generation (2 to 10 players)
		//private static List<PlayerControllerB> GetTeamList()
		//{
		//	System.Random rng = new System.Random();
		//	int numPlayers = rng.Next(2, 11);  // Randomly choose between 2 to 10 players
		//	List<PlayerControllerB> playerList = new List<PlayerControllerB>();

		//	for (int i = 1; i <= numPlayers; i++)
		//	{
		//		playerList.Add("Player" + i);
		//	}

		//	// Print the players in the lobby
		//	Console.WriteLine("Players in Lobby: " + string.Join(", ", playerList));
		//	return playerList;
		//}
		// Splits the list into groups of a specified size using LINQ
		private static List<List<PlayerControllerB>> GetGroupList(List<PlayerControllerB> list, int numInGroup)
		{
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
			Material material = StartOfRound.Instance.unlockablesList.unlockables[suitID].suitMaterial;
			player.thisPlayerModel.material = material;
			player.thisPlayerModelLOD1.material = material;
			player.thisPlayerModelLOD2.material = material;
			player.thisPlayerModelArms.material = material;
			player.currentSuitID = suitID;
		}
	}
}
