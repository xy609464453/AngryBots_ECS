using System;

using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct BulletPrefab : IComponentData
{
	public Entity Value;
}
