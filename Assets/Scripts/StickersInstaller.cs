using UnityEngine;
using Zenject;

public class StickersInstaller : MonoInstaller
{
    [SerializeField] private SaveLevelsComplete m_LevelsComplete;
    public override void InstallBindings()
    {
        //Container.Bind<SaveLevelsComplete>().FromInstance(m_LevelsComplete).AsSingle().NonLazy();

        Container.Bind<SaveLevelsComplete>().FromInstance(m_LevelsComplete);
        Container.QueueForInject(m_LevelsComplete);
    }
    
}