using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CurrencyConverterApplication.ViewModel
{
    public class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();
        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName is not null && _errorsByPropertyName.ContainsKey(propertyName)
            ? _errorsByPropertyName[propertyName]
                : Enumerable.Empty<string>();
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        protected void AddError(string error,
            [CallerMemberName] string? propertyname = null)
        {
            if (propertyname is null) return;
            if (!_errorsByPropertyName.ContainsKey(propertyname))
            {
                _errorsByPropertyName[propertyname] = new List<string>();
            }
            if (!_errorsByPropertyName[propertyname].Contains(error))
            {
                _errorsByPropertyName[propertyname].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyname));
                RaisePropertychanged(nameof(HasErrors));
            }
        }
        protected void ClearErrors([CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertychanged(nameof(HasErrors));
            }
        }
    }
}