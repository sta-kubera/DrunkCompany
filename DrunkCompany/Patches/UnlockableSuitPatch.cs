using GameNetcodeStuff;
using HarmonyLib;


namespace DrunkCompany.Patches
{
	[HarmonyPatch(typeof(UnlockableSuit))]
	internal class UnlockableSuitPatch
	{


		//[HarmonyPatch("SwitchSuitServerRpc")]
		//[HarmonyPrefix]
		//public static void StoppingSuitChange(ref int playerID)
		//{
		//	DrunkCompany.Logger.LogMessage("This function ran for the suit change");
		//	DrunkCompany.Logger.LogMessage($"{playerID} tried to change a suit to using serverRPC");
		//}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(UnlockableSuit), nameof(UnlockableSuit.SwitchSuitForPlayer))]
		private static bool SuppressCallIfNeeded(PlayerControllerB player, int suitID)
		{
			DrunkCompany.Logger.LogMessage($"The player {player} tried to switch suits to {suitID}");
			return false;
		}

	}
}
