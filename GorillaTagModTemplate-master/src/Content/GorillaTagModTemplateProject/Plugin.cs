using UnityEngine;
using Photon.Pun;  // Photon Unity Networking (PUN)
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Collections;

public class GorillaTagTrackerMod : MonoBehaviourPunCallbacks
{
    private string discordWebhookUrl = "https://discord.com/api/webhooks/1292659066002477086/_CbWZZ_aOqhh7z8kKYsxIQJz5ywRjpfzZb-LeAJgiKFIyGOhNcEjPugu-TmHR4-FuYHG"; // Replace with your actual webhook URL
    private string playerName;
    private string roomCode;
    private Color playerColor;

    void Start()
    {
        // Start tracking the player info when the mod initializes
        StartCoroutine(TrackPlayerInfo());
    }

    // Coroutine to track player data every few seconds
    IEnumerator TrackPlayerInfo()
    {
        while (true)
        {
            // Fetch player name and room code using Photon
            playerName = PhotonNetwork.NickName;  // Get the current player name
            roomCode = PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.Name : "No Room";  // Get the current room code if available

            // Get player's color (replace this with actual color retrieval logic)
            playerColor = GetPlayerColor();  // Fetch player's color (custom method for Gorilla Tag)

            // Send the data to Discord webhook
            SendDataToDiscord();

            // Wait for a few seconds before sending the data again
            yield return new WaitForSeconds(10f);  // Adjust the interval as needed
        }
    }

    // Method to get the player's color (replace with actual color handling in Gorilla Tag)
    Color GetPlayerColor()
    {
        // Example: Replace this logic with the actual Gorilla Tag logic for player color
        return GorillaTagPlayerSettings.GetPlayerColor();
    }

    // Method to send the data to Discord using the webhook
    async void SendDataToDiscord()
    {
        var client = new HttpClient();

        // Create the payload to send to Discord
        var data = new
        {
            username = "CREATOR TRACKER",
            embeds = new[]
            {
                new
                {
                    title = "Player Information",
                    fields = new[]
                    {
                        new { name = "Player Name", value = playerName },
                        new { name = "Room Code", value = roomCode },
                        new { name = "Player Color", value = ColorUtility.ToHtmlStringRGB(playerColor) }
                    }
                }
            }
        };

        var jsonData = JsonConvert.SerializeObject(data);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        // Send POST request to the Discord webhook
        await client.PostAsync(discordWebhookUrl, content);
    }
}
