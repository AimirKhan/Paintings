using UnityEngine;
using Zenject;


public class AmplitudeInstaller : MonoInstaller
{
    [SerializeField] private AnalyticsHelper m_AnaliticsHelper;
    public override void InstallBindings()
    {
        Container.Bind<AnalyticsHelper>().FromInstance(m_AnaliticsHelper);
        Container.QueueForInject(m_AnaliticsHelper);
    }
}