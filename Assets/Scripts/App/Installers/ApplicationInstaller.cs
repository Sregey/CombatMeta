using SaberCombatMeta.App.Contracts;
using SaberCombatMeta.Meta;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.App
{
    public class ApplicationInstaller: MonoInstaller
    {
        [SerializeField]
        private Application _application;
        
        public override void InstallBindings()
        {
            Container.Bind(typeof(IApplication), typeof(Application)).FromInstance(_application).AsSingle().NonLazy();
            Container.Bind<MetaController>().AsSingle().NonLazy();
            Container.Bind<ISaveController>().To<LocalSaveController>().AsSingle().NonLazy();
        }
    }
}