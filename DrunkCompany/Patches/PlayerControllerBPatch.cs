using DrunkCompany.Scripts;
using GameNetcodeStuff;
using DrunkCompany;
using HarmonyLib;



namespace DrunkCompany.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
	internal class PlayerControllerBPatch
	{
		//Listens for the first player to die each round
		
		[HarmonyPatch("KillPlayerServerRpc")]
		[HarmonyPostfix]
		public static void SendingDeathWarning(ref int playerId)
		{
			
			DrunkCompany.Logger.LogDebug($"This is the playerId from serverDeathWarning: {playerId}");
			NetworkM.DeathWarningClientRpc(playerId);

		}
	}
}
