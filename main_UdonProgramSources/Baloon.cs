using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using System;

public class Baloon : UdonSharpBehaviour
{

    [Header("浮上するオブジェクト")]
    public Transform m_body;

    [Header("VRCstation")]
    public VRCStation station;
    [Header("浮上する速度")]
    public float m_max_speed = 1;

    [Header("(Option) 浮上する音(ループ音を推奨)")]
    public AudioSource m_sound;
    [Header("(Option) リセット位置と時間")]
    public Transform m_reset_position;
    public VRC_Pickup m_reset_pickup;
    public int m_reset_time = 30;

    bool IsUse = false;
    bool isPicked = false;
    DateTime _reset_time;

    public override void Interact()
    {
        IsUse = true;
        station.UseStation(Networking.LocalPlayer);
        if (m_sound != null)
            m_sound.Play();
    }

    public void Pressed()
    {
        IsUse = false;
        Transform bal_tra = this.transform;
        Vector3 pos = bal_tra.position;
        pos.z += 2.0f;
        bal_tra.position = pos;
        station.stationEnterPlayerLocation = bal_tra;
        station.ExitStation(Networking.LocalPlayer);
        if (m_sound != null)
            m_sound.Stop();
        isPicked = false;
        m_body.position = m_reset_position.position;
        m_body.rotation = m_reset_position.rotation;
    }
    void Start()
    {
        m_reset_position=this.transform;
    }

    void Update()
    {
        if (IsUse)
        {
            
            Vector3 vector = Vector3.up * m_max_speed;

           this.transform.position+=vector;
        }

        if (m_reset_position != null && m_reset_pickup != null)
        {
            if (m_reset_pickup.IsHeld)
            {
                isPicked = true;
                _reset_time = DateTime.UtcNow.AddSeconds(m_reset_time);
            }
            else if (DateTime.UtcNow > _reset_time && isPicked)
            {
                isPicked = false;
                m_body.position = m_reset_position.position;
                m_body.rotation = m_reset_position.rotation;
            }
        }
    }
}
