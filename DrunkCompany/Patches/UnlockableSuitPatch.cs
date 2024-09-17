using GameNetcodeStuff;
using HarmonyLib;


namespace DrunkCompany.Patches
{
	[HarmonyPatch(typeof(UnlockableSuit))]
	internal class UnlockableSuitPatch
	{
		//Suppresses method to allow players to switch suits
		[HarmonyPrefix]
		[HarmonyPatch(typeof(UnlockableSuit), nameof(UnlockableSuit.SwitchSuitForPlayer))]
		private static bool SuppressCallIfNeeded(PlayerControllerB player, int suitID)
		{
			DrunkCompany.Logger.LogMessage($"The player {player} tried to switch suits to {suitID}");
			return false;
		}

	}
}
