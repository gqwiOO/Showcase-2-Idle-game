using System;

namespace Mechanics.Income.Service
{
    public interface ILaunderService
    {
        float GetMaxLaunderAmount();
        bool CanLaunder(float amount);
        void Launder(float amount);
        event Action OnMaxAmountChanged;
    }
}
