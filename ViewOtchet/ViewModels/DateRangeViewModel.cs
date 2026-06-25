using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OtchetClient.ViewModels
{
        public class DateRangeViewModel : INotifyPropertyChanged
        {
            private DateTime? _startDate;
            private DateTime? _endDate;

            public DateTime? StartDate
            {
                get => _startDate;
                set
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));

                    // Корректируем EndDate если нужно
                    if (_endDate < _startDate)
                    {
                        EndDate = _startDate;
                    }
                }
            }

            public DateTime? EndDate
            {
                get => _endDate;
                set
                {
                    if (_startDate != null && value < _startDate)
                    {
                        _endDate = _startDate;
                    }
                    else
                    {
                        _endDate = value;
                    }

                    OnPropertyChanged(nameof(EndDate));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
