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
    class NetworkM : MonoBehaviour
	{
		//Used to create a list of players in the lobby
        internal static List<PlayerControllerB> currentPlayers = new List<PlayerControllerB>();

		//Suit ID:
		//Red = 33
		//purple = 32
		//Pink = 31
		//Neon Green = 30
		//Dandelion = 29
		//Cyan = 28
		//Orange = 0

		//private static List<int> colors = new List<int> { 28, 29, 32, 30, 31, 0 };
		private static List<int> colors = new List<int> { 30, 29, 32, 28, 31, 0 };

		//Used to keep track of the first death player
		internal static List<string> deadPlayers = new List<string>();

		public static List<string> PosterFolders = new List<string>();
		
		//PlayerControllerB captain

		//Method to randomly select a captain from currentPlayers List
		[ServerRpc]
		public static void DetermineTeamsServerRpc()
        {
			int randomSeed = UnityEngine.Random.Range(1, 10000);
			deadPlayers.Clear();
            currentPlayers.Clear();

			//Loops through players in the lobby and adds them to currentPlayers List
			PlayerControllerB[] allPlayerScripts = StartOfRound.Instance.allPlayerScripts;
			foreach (PlayerControllerB player in allPlayerScripts)
            {
				if (!player.isPlayerControlled || player.isPlayerDead) { continue; }
                {
					currentPlayers.Add(player);
				}
            }

			
			currentPlayers = Shuffle(currentPlayers, randomSeed);  // Shuffles players
			NetworkBehaviourReference captain = currentPlayers[0];  // First player becomes captain
			ShowHudMessageClientRpc(captain);
			SetPlayerSuitClientRpc(captain, 33);
			List<PlayerControllerB> remainingPlayers = currentPlayers.Skip(1).ToList();  // Remaining players in one group


			// Split players into groups of 2
			List<List<PlayerControllerB>> groups = GetGroupList(remainingPlayers, 2);

			if (groups[0].Count > 1)
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
			for (int i = 0; i < groups.Count; i++)
			{
				int color = colors[i % colors.Count];
				foreach (NetworkBehaviourReference player in groups[i]) {
					SetPlayerSuitClientRpc(player, color);
				}	
			}

        }
		// Shuffle the list of players using LINQ and a random number generator
		private static List<PlayerControllerB> Shuffle(List<PlayerControllerB> list, int randomSeed)
		{
			System.Random rng = new System.Random(randomSeed);
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
			PlayerControllerB firstDeath = StartOfRound.Instance.allPlayerScripts[playerId];
			DrunkCompany.Logger.LogDebug($"This is the playerID: {playerId} and this is the player from current players: {firstDeath.playerUsername}");
			if (deadPlayers.Count() == 0)
            {
                HUDManager.Instance.DisplayTip(firstDeath.playerUsername, "The first person to die!", isWarning: true);
                deadPlayers.Add(firstDeath.playerUsername);
            }
        }
		//Method to send a message to all players in their HUD
		[ClientRpc]
		public static void ShowHudMessageClientRpc(NetworkBehaviourReference captain)
		{
			PlayerControllerB playerB = (PlayerControllerB)captain;
			HUDManager.Instance.DisplayTip("The captain is", $"{playerB.playerUsername}");
		}

		[ClientRpc]
		public static void SetPlayerSuitClientRpc(NetworkBehaviourReference player, int suitID)
		{
			
			PlayerControllerB playerB = (PlayerControllerB)player;
			Material material = StartOfRound.Instance.unlockablesList.unlockables[suitID].suitMaterial;
			playerB.thisPlayerModel.material = material;
			playerB.thisPlayerModelLOD1.material = material;
			playerB.thisPlayerModelLOD2.material = material;
			playerB.thisPlayerModelArms.material = material;
			playerB.currentSuitID = suitID;
			DrunkCompany.Logger.LogDebug($"This is the suitID: {suitID} and this is the player: {playerB}");
			deadPlayers.Clear();

		}
	}

}
