using System.Diagnostics.CodeAnalysis;

namespace SaberCombatMeta.Simulation
{
    public class PlayerManager: EntityManager<PlayerEntity>
    {
        [MaybeNull]
        public PlayerEntity Player => Entities.Count > 0 ? Entities[0] : null;
    }
}