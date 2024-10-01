using DrunkCompany.Scripts;
using DrunkCompany;
using HarmonyLib;
using System.Threading;


namespace DrunkCompany.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class StartMatchLeverPatch
	{
		//Waits till the ship lands to determine who captain is
		[HarmonyPatch("ResetStats")]
		[HarmonyPrefix]
		public static void SendingMessagePatch()
		{

			DrunkCompany.Logger.LogMessage("This is a test");
			DrunkCompany.Logger.LogMessage("sending message function ran");
			NetworkManager.DetermineCaptain();
		}
	}
}
