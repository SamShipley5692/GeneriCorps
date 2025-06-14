using UnityEngine;
using System.Collections.Generic;

public class PoliteAIQueue : MonoBehaviour
{
    public static Queue<enemyAI> AttackQueue = new Queue<enemyAI>();
    public static int maxActiveAttackers = 1;

    public static void TryJoinQueue(enemyAI enemy)
    {
        if (!AttackQueue.Contains(enemy))
            AttackQueue.Enqueue(enemy);
    }

    public static void UpdateQueue()
    {
        int active = 0;
        foreach (enemyAI enemy in AttackQueue)
        {
            if (active < maxActiveAttackers)
            {
                enemy.EnableAttack();
                active++;
            }
            else
            {
                enemy.WaitInQueue();
            }
        }
    }

    public static void RemoveFromQueue(enemyAI enemy)
    {
        Queue<enemyAI> newQueue = new Queue<enemyAI>();
        foreach (enemyAI e in AttackQueue)
        {
            if (e != enemy)
                newQueue.Enqueue(e);
        }
        AttackQueue = newQueue;
    }
}
