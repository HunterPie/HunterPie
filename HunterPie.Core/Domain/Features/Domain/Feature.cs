using HunterPie.Core.Architecture;
using Newtonsoft.Json;
using System.ComponentModel;

namespace HunterPie.Core.Domain.Features.Domain
{
    public class Feature : IFeature
    {
        private readonly Observable<bool> _isEnabled = false;
        public Observable<bool> IsEnabled => _isEnabled;

        [JsonConstructor]
        public Feature()
        {
            _isEnabled.PropertyChanged += OnPropertyChange;
        }

        public Feature(bool defaultInitializer = false)
        {
            _isEnabled = defaultInitializer;
            _isEnabled.PropertyChanged += OnPropertyChange;
        }

        ~Feature()
        {
            _isEnabled.PropertyChanged -= OnPropertyChange;
        }

        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (IsEnabled)
                OnEnable();
            else
                OnDisable();
        }

        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}
    }
}
