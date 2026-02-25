using System;

namespace Mechanics.Product
{
    public interface IIncomeObject
    {
        event Action OnIncomeChanged;
    }
}
