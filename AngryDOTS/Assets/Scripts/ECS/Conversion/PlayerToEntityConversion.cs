using Unity.Entities;

using UnityEngine;

public class PlayerToEntityConversion : MonoBehaviour, IConvertGameObjectToEntity
{
    public float healthValue = 1f;
    public Entity entity;
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
        manager.AddComponent(entity, typeof(PlayerTag));

        Health health = new Health { Value = healthValue };
        manager.AddComponentData(entity, health);
        this.entity = entity;
    }
}