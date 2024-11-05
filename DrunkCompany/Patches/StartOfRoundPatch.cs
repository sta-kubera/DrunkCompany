using DrunkCompany.Scripts;
using DrunkCompany;
using HarmonyLib;
using GameNetcodeStuff;
using System.Threading;
using Unity.Netcode;


namespace DrunkCompany.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class StartMatchLeverPatch
	{
		//Waits till the stats reset to DetermineTeams
		[HarmonyPatch("ResetStats")]
		[HarmonyPostfix]
		public static void SendingMessagePatch()
		{
			if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
				NetworkM.DetermineTeamsServerRpc();
		}
	}
}
