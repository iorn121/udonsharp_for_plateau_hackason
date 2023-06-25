using UdonSharp;
using UnityEngine;
using System.Collections.Generic;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;


public class ViewUsers : UdonSharpBehaviour
{
    [Header("表示するテキスト")]
    public Text m_text;

    [Header("スコアテキスト")]
    public Text s_text;

    VRCPlayerApi[] players = new VRCPlayerApi[20];
    // [UdonSynced] Dictionary<string, int> players_score = new Dictionary<string, int>();
    [UdonSynced] private int score = 0;
    int _count = 0;

    [Header("[Option] Join音")]
    public AudioSource m_audioSource;


    void Start()
    {
        _count = VRCPlayerApi.GetPlayerCount();
        VRCPlayerApi.GetPlayers(players);
        foreach(VRCPlayerApi player in players)
        {
            m_text.text = "";
            if (player == null) continue;
            m_text.text += player.displayName + " 0p";
            m_text.text += "\n";
            s_text.text = score.ToString() + " p";
            // if (!players_score.ContainsKey(player.displayName))
            // {
            //     players_score.Add(player.displayName, 0);
            // }
        }
    }

    void Update()
    {
        s_text.text = score.ToString() + " p";
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        _count = VRCPlayerApi.GetPlayerCount();
        VRCPlayerApi.GetPlayers(players);
        foreach (VRCPlayerApi player_joined in players)
        {
            if (player_joined == null) continue;
            m_text.text = "";
            m_text.text += player_joined.displayName;

            m_text.text += "\n";
        }
        if (m_audioSource != null && m_audioSource.isPlaying)
        {
            m_audioSource.Play();
        }
        s_text.text = score.ToString() + " p";
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        _count = VRCPlayerApi.GetPlayerCount();
        VRCPlayerApi.GetPlayers(players);
        foreach (VRCPlayerApi player_left in players)
        {
            if (player_left == null) continue;
            m_text.text = "";
            m_text.text += player_left.displayName;
            m_text.text += "\n";
        }
        s_text.text = score.ToString() + " p";
    }

}
