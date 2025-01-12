using System;

namespace Mechanics.Companies
{
    public interface ICharacterCompanyListener
    {
        event Action<ICompanyData> OnMyCharacterCompanyDataChanged;
    }
}