using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyBulletPattern")]
public class EnemyBulletPattern : ScriptableObject
{
    [SerializeField] private float direction;
    public float Direction { get => direction; set => direction = value; }

    [SerializeField] private float speed;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private int count;
    public int Count { get => count; set => count = value; }

    [SerializeField] private float opneAngle;
    public float OpenAngle { get => opneAngle; set => opneAngle = value; }

    [SerializeField] private bool isAimPlayer;
    public bool IsAimPlayer { get => isAimPlayer; set => isAimPlayer = value; }

    [SerializeField] private int power;
    public int Power { get => power; set => power = value; }
}
