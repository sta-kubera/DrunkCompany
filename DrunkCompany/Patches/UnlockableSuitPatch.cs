using GameNetcodeStuff;
using HarmonyLib;
using System.Xml.Linq;


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
			DrunkCompany.Logger.LogDebug($"this function has ran {suitID}");
			return false;
		}

	}
}
