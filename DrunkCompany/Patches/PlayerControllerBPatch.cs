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
		[HarmonyPrefix]
		public static void SendingDeathWarning(ref int playerId)
		{

			DrunkCompany.Logger.LogMessage("sending death Warning Ran");
			NetworkManager.DeathWarningClientRpc(playerId);

		}
	}
}
