using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Extension.Zenject
{
    public class MonoAdapter: MonoInstaller
    {
        [SerializeField] private List<MonoInstaller> _installers;
        public override void InstallBindings()
        {
            foreach (var installer in _installers)
            {
                Container.Inject(installer);
                installer.InstallBindings();
            }
        }
    }
}