using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OWOrganizerApp.Helpers.Converters
{
    public class EditAccountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Tuple<bool, Models.AccountModel> acc = new Tuple<bool, Models.AccountModel>((bool)values[0], (Models.AccountModel)values[1]);
            return acc;
        }
        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
