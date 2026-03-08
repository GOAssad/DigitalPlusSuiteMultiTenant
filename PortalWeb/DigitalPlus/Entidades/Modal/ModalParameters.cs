namespace DigitalPlus.Entidades.Modal
{
    public class ModalParameters
    {
        private Dictionary<string, object> _parameters;

        public ModalParameters()
        {
            _parameters = new Dictionary<string, object>();
        }

        public void Add(string parameterName, object value)
        {
            _parameters[parameterName] = value;
        }

        public T Get<T>(string parameterName)
        {
            if(!_parameters.ContainsKey(parameterName))
            {
                throw new KeyNotFoundException("No Existe.");
            }

            return (T)_parameters[parameterName];
        }

        public T TryGet<T>(string parameterName)
        {
            if(_parameters.ContainsKey(parameterName)) 
            {
                return (T)_parameters[parameterName];
            }

            return default;
        }
    }
}
