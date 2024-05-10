using System.Net.Mail;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards.Telegraph
{
    public interface IHazardDispenserTelegraph
    {
        UniTaskVoid TelegraphHazard(float duration);
    }
}