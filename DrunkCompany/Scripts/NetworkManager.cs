using GameNetcodeStuff;
using StaticNetcodeLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using DrunkCompany;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using BepInEx;
using System.IO;
using System.Threading;

namespace DrunkCompany.Scripts
{
    [StaticNetcode]
    class NetworkManager : MonoBehaviour
    {
		//Used to create a list of players in the lobby
        internal static List<PlayerControllerB> currentPlayers = new List<PlayerControllerB>();

		//Used to keep track of the first death player
        internal static List<string> deadPlayers = new List<string>();

		public static List<string> PosterFolders = new List<string>();


		//Method to send a message to all players in their HUD
		[ClientRpc]
        public static void ShowHudMessageClientRpc(string name)
        {
			DrunkCompany.Logger.LogMessage($"this function has ran {name}");
			DrunkCompany.Logger.LogMessage($"This the current list {currentPlayers}");
            HUDManager.Instance.DisplayTip("The captain", $"{name} has be designated.");

        }

		//Method to randomly select a captain from currentPlayers List
        public static void DetermineCaptain()
        {
			
			DrunkCompany.Logger.LogMessage("Determine Captain function was ran");
			//CreateBitmapImage();
			deadPlayers.Clear();
            currentPlayers.Clear();

			//Loops through players in the lobby and adds them to currentPlayers List
			PlayerControllerB[] allPlayerScripts = StartOfRound.Instance.allPlayerScripts;
			foreach (PlayerControllerB player in allPlayerScripts)
            {
				if (!player.isPlayerControlled || player.isPlayerDead) { continue; }
                {
					DrunkCompany.Logger.LogMessage($"Adding Player {player} to currentPlayers");
					DrunkCompany.Logger.LogMessage($"Adding Player with client ID {player.actualClientId} to currentPlayers");
					DrunkCompany.Logger.LogMessage($"Adding Player with username {player.playerUsername} to currentPlayers");
					Material material = StartOfRound.Instance.unlockablesList.unlockables[30].suitMaterial;
					player.thisPlayerModel.material = material;
					player.thisPlayerModelLOD1.material = material;
					player.thisPlayerModelLOD2.material = material;
					player.thisPlayerModelArms.material = material;
					player.currentSuitID = 30;
					currentPlayers.Add(player);
                }
            }
			
			
            PlayerControllerB Captain = currentPlayers[UnityEngine.Random.Range(0, currentPlayers.Count())];

			
			ShowHudMessageClientRpc(Captain.playerUsername);


        }

		//Method to send first death warning HUD message to all players
        [ClientRpc]
        public static void DeathWarningClientRpc(int playerId)
        {
            PlayerControllerB firstDeath = currentPlayers[playerId];
			DrunkCompany.Logger.LogMessage($"The amount of dead players currently {deadPlayers.Count()}");
			DrunkCompany.Logger.LogMessage($"This is the playerId {firstDeath.playerUsername}");
			DrunkCompany.Logger.LogMessage($"This is the playerId {currentPlayers[playerId]}");
			if (deadPlayers.Count() == 0)
            {
                HUDManager.Instance.DisplayTip("Someone has died", "This is their name " + firstDeath.playerUsername, isWarning: true);
                deadPlayers.Add(firstDeath.playerUsername);
            }
        }

		[ClientRpc]
		public static void SetPlayerSuit(PlayerControllerB player)
		{
			Material material = StartOfRound.Instance.unlockablesList.unlockables[30].suitMaterial;
			player.thisPlayerModel.material = material;
			player.thisPlayerModelLOD1.material = material;
			player.thisPlayerModelLOD2.material = material;
			player.thisPlayerModelArms.material = material;
			player.currentSuitID = 30;
		}



		//public static Bitmap CreateBitmapImage()
		//{
		//	DrunkCompany.Logger.LogMessage("Bitmap function was ran");
		//	string sImageText = "test";
		//	Bitmap objBmpImage = new Bitmap(2, 2);

		//	int intWidth = 256;
		//	int intHeight = 256;

		//	// Create the Font object for the image text drawing.
		//	System.Drawing.Font objFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

		//	// Create a graphics object to measure the text's width and height.
		//	System.Drawing.Graphics objGraphics = System.Drawing.Graphics.FromImage(objBmpImage);

		//	// This is where the bitmap size is determined.
		//	intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
		//	intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

		//	// Create the bmpImage again with the correct size for the text and font.
		//	objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));


		//	// Add the colors to the new bitmap.
		//	objGraphics = System.Drawing.Graphics.FromImage(objBmpImage);

		//	// Set Background color

		//	objGraphics.Clear(System.Drawing.Color.White);
		//	objGraphics.SmoothingMode = SmoothingMode.HighQuality;



		//	objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
		//	objGraphics.DrawString(sImageText, objFont, new SolidBrush(System.Drawing.Color.Black), 0, 0, StringFormat.GenericDefault);

		//	objGraphics.Flush();

		//	objBmpImage.Save($"{Paths.PluginPath}test.png");

		//	DrunkCompany.Logger.LogMessage($"Bitmap function was saved to this path: {Paths.PluginPath}");
		//	return (objBmpImage);

		//}


	}
}
