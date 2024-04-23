namespace Popeye.Modules.AudioSystem
{
    public class GlobalParametersController
    {
        private readonly GlobalParametersConfig _config;

        public GlobalParametersController(GlobalParametersConfig config)
        {
            _config = config;
        }

        public void StartListeningToParameters()
        {
            foreach (SoundParameter parameter in _config.Parameters)
            {
                parameter.OnValueChanged += UpdateParameter;
            }
        }
        
        public void StopListeningToParameters()
        {
            foreach (SoundParameter parameter in _config.Parameters)
            {
                parameter.OnValueChanged -= UpdateParameter;
            }
        }
        
        
        private void UpdateParameter(SoundParameter parameter)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameter.Name, parameter.Value);
        }
        
    }
}