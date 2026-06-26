using System.Collections.Generic;

public class CompositeSkinManager : ISkinComponent
{
    private readonly List<ISkinComponent> _components;

    public CompositeSkinManager(List<ISkinComponent> components)
    {
        _components = components;
    }

    public void Apply(ManData data)
    {
        foreach (var component in _components)
        {
            component.Apply(data);
        }
    }
}