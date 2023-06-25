using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using System;


public class AirPlane : UdonSharpBehaviour
{

    [Header("ジェット噴射するオブジェクト")]
    public Transform m_body;
    [Header("ジェットのパワー")]
    public float m_power = 1;
    [Header("ジェットの最大速度")]
    public float m_max_speed = 1;

    [Header("(Option) ジェットのエフェクト")]
    public ParticleSystem m_particle;
    [Header("(Option) ジェットの噴射音(ループ音を推奨)")]
    public AudioSource m_sound;
    [Header("(Option) リセット位置と時間")]
    public Transform m_reset_position;
    public VRC_Pickup m_reset_pickup;
    public int m_reset_time = 30;

    bool IsUse = false;
    bool isPicked = false;
    DateTime _reset_time;


    public override void OnPickupUseDown()
    {
        IsUse = true;
        if (m_particle != null)
            m_particle.Play();
        if (m_sound != null)
            m_sound.Play();
    }
    public override void OnPickupUseUp()
    {
        IsUse = false;
        if (m_particle != null)
            m_particle.Stop();
        if (m_sound != null)
            m_sound.Stop();
    }

    void Update()
    {
        if (IsUse)
        {
            Vector3 vector = Networking.LocalPlayer.GetVelocity();
            vector += (m_body.forward * m_power);
            if (vector.magnitude > m_max_speed)
            {
                vector = vector.normalized * m_max_speed;
            }
            Networking.LocalPlayer.SetVelocity(vector);
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

