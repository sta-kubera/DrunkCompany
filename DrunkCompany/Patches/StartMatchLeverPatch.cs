using DrunkCompany.Scripts;
using DrunkCompany;
using HarmonyLib;


namespace DrunkCompany.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class StartMatchLeverPatch
	{
		[HarmonyPatch("OnShipLandedMiscEvents")]
		[HarmonyPrefix]
		public static void SendingMessagePatch()
		{
			DrunkCompany.Logger.LogMessage("This is a test");
			DrunkCompany.Logger.LogMessage("sending message function ran");
			NetworkManager.DetermineCaptain();
		}
	}
}

//{
//	[HarmonyPatch(typeof(StartMatchLever))]
//	internal class StartMatchLeverPatch
//{
//	[HarmonyPatch(nameof(StartMatchLever.PullLever))]
//	[HarmonyPostfix]
//	public static void SendingMessagePatch()
//	{
//		TestAnnouncer.Logger.LogMessage("sending message function ran");
//		NetworkManager.PingClientRpc("Tis me");
//	}
//}
//}