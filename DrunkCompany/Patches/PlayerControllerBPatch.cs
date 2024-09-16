using DrunkCompany.Scripts;
using GameNetcodeStuff;
using HarmonyLib;
using JetBrains.Annotations;
using Unity.Services.Authentication.Internal;


namespace DrunkCompany.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
	internal class PlayerControllerBPatch
	{
		
		
		[HarmonyPatch("KillPlayerServerRpc")]
		[HarmonyPrefix]
		public static void SendingDeathWarning(ref int playerId)
		{

			DrunkCompany.Logger.LogMessage("sending death Warning Ran");
			NetworkManager.DeathWarningClientRpc(playerId);

		}
	}
}
