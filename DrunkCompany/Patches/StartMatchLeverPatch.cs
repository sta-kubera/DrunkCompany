using BepInEx.Logging;
using DrunkCompany.Scripts;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Xml.Linq;


namespace DrunkCompany.Patches
{

    internal class StartMatchLeverPatch
	{
		
		[HarmonyPatch(typeof(StartOfRound))]
		

		[HarmonyPatch("OnShipLandedMiscEvents")]
		[HarmonyPostfix]
		public static void SendingMessagePatch()
		{
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