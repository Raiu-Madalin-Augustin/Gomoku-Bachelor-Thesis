using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gomoku.GUI.ViewModels
{
    public abstract class ViewModelBase :
      INotifyPropertyChanging,
      INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event PropertyChangingEventHandler? PropertyChanging;

        public void NotifyPropertyChanged(
          [CallerMemberName] string? propertyName = null)
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(
                  this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected void SetProperty<T>(
          ref T field,
          T newValue,
          [CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null) return;
            if (EqualityComparer<T>.Default.Equals(field, newValue)) return;

            PropertyChanging?.Invoke(
                this, new PropertyChangingEventArgs(propertyName));
            field = newValue;
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }
    }
}