using System.Collections;
using System.Collections.Generic;

namespace Training.DomainClasses;

public class ReadOnlyOf<TItem> : IEnumerable<TItem>
{
    private IEnumerable<TItem> _pets;

    public ReadOnlyOf(IEnumerable<TItem> pets)
    {
        _pets = pets;
    }

    public IEnumerator<TItem> GetEnumerator()
    {
        return _pets.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
