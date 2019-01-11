/*******************************************************************************************************************************/
/*                                                                                                                              */
/*  Program         : CustomDateTimePicker                                                                                      */
/*  Desc            : This is a user control created as per specs to show a textbox which accepts datetimeonly and has a dropdown 
 *                    which contains a calendar to pick date and a textbox which only accepts time input. These items in popup 
 *                    are linked to the main textbox.                                                                           */
/*  Author          : Darshan Honnali PattanaShetty                                                                             */
/*  Date Created    : 1/11/2019                                                                                                */
/*  Last Mod On     :                                                                                                           */
/********************************************************************************************************************************/


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace Example
{
    /// <summary>
    /// Interaction logic for CustomDateTimePicker.xaml
    /// </summary>
    public partial class CustomDateTimePicker : UserControl
    {
        public bool valid = false, valueChanged = true, mainDateTimeValueChanged = true, calValueChangedByMaskedInput = false;
        public DateTime correctDateTime;
        DateTime? startDate = null, endDate = null;
        public RadCalendar PopUpCalendar = null;
        public RadMaskedDateTimeInput PopUpMaskedTimeInput = null;
        public RadMaskedDateTimeInput MainMaskedDateTimeInput = null;
        public CustomDateTimePicker()
        {
            InitializeComponent();
            MainMaskedDateTimeInput = radMaskedDateTimeInput;
        }
        public void SetStartDate(DateTime startdate)
        {
            startDate = startdate;
            //MaskedInputExtensions.SetMinimum(MainMaskedDateTimeInput, startdate);
        }
        public void SetEndDate(DateTime enddate)
        {
            endDate = enddate;
            //MaskedInputExtensions.SetMaximum(MainMaskedDateTimeInput, enddate);
        }
        void CustomDateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var template = radMaskedDateTimeInput.Template;
            PopUpCalendar = (RadCalendar)template.FindName("CalDisplay", radMaskedDateTimeInput);
            PopUpMaskedTimeInput = (RadMaskedDateTimeInput)template.FindName("radMaskedTimeInput", radMaskedDateTimeInput);
            if (PopUpCalendar != null && startDate != null)
            {
                PopUpCalendar.SelectableDateStart = startDate;
            }
            if (PopUpCalendar != null && endDate != null)
            {
                PopUpCalendar.SelectableDateEnd = endDate;
            }
        }

        private void CustomPopup_Opened(object sender, EventArgs e)
        {
            calValueChangedByMaskedInput = true;
            if (radMaskedDateTimeInput.Value != null)
            {
                PopUpCalendar.DisplayDate = radMaskedDateTimeInput.Value.Value.Date;
                PopUpCalendar.SelectedDate = radMaskedDateTimeInput.Value.Value.Date;
                correctDateTime = radMaskedDateTimeInput.Value.Value;
            }
            calValueChangedByMaskedInput = false;
        }


        // to retain focus on time part and stop from lossing focus to main datetime part when its clicked
        private void radMaskedTimeInput_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void radMaskedTimeInput_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (PopUpMaskedTimeInput.Text != null && !valid)
            {
                valid = true;
                PopUpMaskedTimeInput.RaiseEvent(new RoutedEventArgs(LostFocusEvent, PopUpMaskedTimeInput));
                valid = false;
            }
            mainDateTimeValueChanged = true;
            valueChanged = true;
        }

        private void PreviewLostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
        private void radMaskedDateTimeInput_KeyUp(object sender, RoutedEventArgs e)
        {
            var value = radMaskedDateTimeInput.Value;
            if (value != null && value.Value < startDate)
            {
                radMaskedDateTimeInput.Value = null;
            }
            else if (value != null && value.Value > endDate)
            {
                radMaskedDateTimeInput.Value = null;
            }
            e.Handled = true;
        }
        private void radMaskedDateTimeInput_LostFocus(object sender, RoutedEventArgs e)
        {
            var value = radMaskedDateTimeInput.Value;
            if (value != null && value.Value < startDate)
            {
                radMaskedDateTimeInput.Value = null;
            }
            else if (value != null && value.Value > endDate)
            {
                radMaskedDateTimeInput.Value = null;
            }
            else if (radMaskedDateTimeInput.Text != null)
            {
                try
                {
                    Convert.ToDateTime(radMaskedDateTimeInput.Text);
                }
                catch(Exception ex)
                {
                    radMaskedDateTimeInput.Value = new DateTime(1,1,1);
                    radMaskedDateTimeInput.Value = null;
                    radMaskedDateTimeInput.Text = "";
                }
            }
            e.Handled = true;
        }
        private void radMaskedDateTimeInput_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (!mainDateTimeValueChanged)
            {
                if (!valueChanged)
                {
                    correctDateTime = radMaskedDateTimeInput.Value.Value;
                    valueChanged = true;
                }
                radMaskedDateTimeInput.Value = correctDateTime;
            }
            calValueChangedByMaskedInput = true;
            if (radMaskedDateTimeInput.Value != null)
            {
                PopUpCalendar.DisplayDate = radMaskedDateTimeInput.Value.Value.Date;
                PopUpCalendar.SelectedDate = radMaskedDateTimeInput.Value.Value.Date;
            }
            calValueChangedByMaskedInput = false;
        }




        // for future use if any//
        private void radMaskedTimeInput_ValueChanged(object sender, RoutedEventArgs e)
        {
            mainDateTimeValueChanged = false;
            valueChanged = false;
            var value = PopUpMaskedTimeInput.Value;
            if (value.Value.Year == 0001)
            {
                DateTime date = correctDateTime.Date;
                TimeSpan time = radMaskedDateTimeInput.Value.Value.TimeOfDay;
                PopUpMaskedTimeInput.Value = date + time;
            }
            
        }
        private void CalDisplay_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!calValueChangedByMaskedInput)
            {
                radMaskedDateTimeInput.Value = PopUpCalendar.SelectedDate;
            }
        }

        public void SetBackgroundToYellow()
        {
            this.Resources["DynamicBackgroundBrush"] = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00));
        }

        public void SetBackgroundToWhite()
        {
            this.Resources["DynamicBackgroundBrush"] = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
        }
    }
}
