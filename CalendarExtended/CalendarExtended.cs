#region CalendarExtended®
/*
 CalendarExtended® 
 Custom User Control Calendar with extra customizing and localizing facilities.
 Version 1.0.0.0 
 Target Framework .NET Framework 4.0
 Features - bigger calendar control, customizable, highlight holidays, special dates and any given date
 This source code is distributed as free and open source for educational purposes
 © Sudam Dineetha 2017
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Xml;

namespace CalendarExtended
{
    /// <summary>
    /// Extended Month Calendar Control for Windows Forms
    /// </summary>
    public partial class CalendarExtended : UserControl
    {
        #region Variables
        private DateTime nowDateTime = DateTime.Now;
        private int selectedMonth = DateTime.Now.Month;
        private string nl = Environment.NewLine;
        /// <summary>
        /// Symbol Character for filling empty cells
        /// </summary>
        public string emptyCellSymbol = "*";
        #endregion

    /// <summary>
    /// Default Constructor for Extended Month Calendar Control
    /// </summary>
        public CalendarExtended()
        {
            InitializeComponent();
            CreateCalendar(nowDateTime);
            button2.Text = "Today: " + DateTime.Now.ToString("yyyy-MM-dd");
            SelectToday();
        }
   
    /// <summary>
    /// Access Calendar DataGridView
    /// </summary>
        public DataGridView DataGrid
        {
            get { return this.Calendar; }
            set { this.Calendar = value; }
        }
    /// <summary>
    /// Access Year Month Label
    /// </summary>
        public Label TitleLabel
        {
            get { return this.label1; }
            set { this.label1 = value; }
        }

        /// <summary>
        /// Access Day Detail Label
        /// </summary>
        public Label DetailLabel 
        {
            get { return this.label2; }
            set { this.label2 = value; }
        }

        /// <summary>
        /// Access Previous Month Button
        /// </summary>
        public Button BackwordButton
        {
            get { return this.button1; }
            set { this.button1 = value; }
        }

    /// <summary>
    /// Access Goto Today Button
    /// </summary>
        public Button ResetButton
        {
            get { return this.button2; }
            set { this.button2 = value; }
        }
    
    /// <summary>
    /// Access Next Month Button
    /// </summary>
        public Button ForwordButton
        {
            get { return this.button3; }
            set { this.button3 = value; }
        } 


        // Fill Calendar DataGridView Rows with Calendar Dates
        // Call Highlight Days Methods and Add Week Numbers
        private void CreateCalendar(DateTime datetime)
        {
            Calendar.Rows.Clear();
            var startDate = datetime;  //DateTime.Now;
            label1.Text = datetime.Year + " " + datetime.ToString("MMMM");
            var year = startDate.Year;
            var month = startDate.Month;
            DayOfWeek firstDate = new DateTime(year, month, 1).DayOfWeek;
            var firstDateOffset = Convert.ToInt32(firstDate);
            firstDateOffset = (firstDateOffset == 0) ? 7 : firstDateOffset;
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var lastDateOffset = 35 - (firstDateOffset - 1) - daysInMonth;
            string[] dates = new string[42];

            int j = 0;
            for (int i = 0; i < 42; i++)
            {
                if (i >= firstDateOffset - 1 && i < (daysInMonth + firstDateOffset - 1))
                {
                    j++;
                    dates[i] = j.ToString();
                }
                else
                {
                    dates[i] = emptyCellSymbol;
                }
            }
            string[] dateRows = ArraySplit.SplitArray(dates, 6);
            foreach (var item in dateRows)
            {
                Calendar.Rows.Add(item.Split(','));
            }
            // AddWeekNumbers(datetime);
            if (dateRows[5].Split(',')[0] == emptyCellSymbol)
            {
                Calendar.Rows.RemoveAt(5);
            }

            HighlightDays();

            } // End CreateCalendar method

        // Button Events
        private void button1_Click(object sender, EventArgs e)
        {
            selectedMonth = selectedMonth - 1;
            if (selectedMonth < 1)
            {
                CreateCalendar(new DateTime(nowDateTime.Year - 1, 12, 1));
                nowDateTime = new DateTime(nowDateTime.Year - 1, 12, 1);
                selectedMonth = 12;
            }
            else if (selectedMonth > 12)
            {
                CreateCalendar(new DateTime(nowDateTime.Year + 1, 1, 1));
                selectedMonth = 1;
                nowDateTime = new DateTime(nowDateTime.Year + 1, 1, 1);
            }
            else
            {
                CreateCalendar(new DateTime(nowDateTime.Year, selectedMonth, 1));
            }
            HighlightDays();
        }

        private void button2_Click(object sender, EventArgs e)
        {        
            nowDateTime = DateTime.Now;
            selectedMonth = DateTime.Now.Month;
            HighlightDays();
            CreateCalendar(DateTime.Now);
            SelectToday();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            selectedMonth = selectedMonth + 1;
            if (selectedMonth < 1)
            {
                CreateCalendar(new DateTime(nowDateTime.Year - 1, 12, 1));
                nowDateTime = new DateTime(nowDateTime.Year - 1, 12, 1);
                selectedMonth = 12;
            }
            else if (selectedMonth > 12)
            {
                CreateCalendar(new DateTime(nowDateTime.Year + 1, 1, 1));
                selectedMonth = 1;
                nowDateTime = new DateTime(nowDateTime.Year + 1, 1, 1);
            }
            else
            {
                CreateCalendar(new DateTime(nowDateTime.Year, selectedMonth, 1));
            }
            HighlightDays();
        }

        // Highlight Days Methods
        private void DayHighlighter(int year, int month, Dictionary<int,string> days) {
            var cellStyle = new DataGridViewCellStyle();
            cellStyle.ForeColor = Color.Red;
            Calendar.Columns[5].DefaultCellStyle = cellStyle;
            Calendar.Columns[6].DefaultCellStyle = cellStyle;
            if (label1.Text == DateTime.Now.Year + " " + DateTime.Now.ToString("MMMM"))
            {
                int x1 = 5;
                int v1 = -1;
                if (Calendar.Rows.Count == 6)
                {
                    x1 = 6;
                }
                for (int i = 0; i < x1; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (Calendar.Rows[i].Cells[j].Value.ToString() != emptyCellSymbol)
                        {
                            v1 = Convert.ToInt32(Calendar.Rows[i].Cells[j].Value);
                        }
                        if (v1 == DateTime.Now.Day)
                        {
                            Calendar.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                            Calendar.Rows[i].Cells[j].ToolTipText = "Today";
                        }
                    }
                }  
            }         
            
            if (year == this.nowDateTime.Year && month == this.selectedMonth)
            {
                int x = 5;
                int v = -1;
                if (Calendar.Rows.Count == 6)
                {
                    x = 6;
                }
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (Calendar.Rows[i].Cells[j].Value.ToString() != emptyCellSymbol)
                        {
                            v = Convert.ToInt32(Calendar.Rows[i].Cells[j].Value);
                        }
                        if (days.ContainsKey(v))
                        {

                            foreach (var item in days)
                            {
                                if (item.Value.ToString().Contains("Poyaday"))
                                {
                                    Calendar.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                                    Calendar.Rows[i].Cells[j].Style.ForeColor = Color.Red;
                                }
                                else
                                {
                                    Calendar.Rows[i].Cells[j].Style.BackColor = Color.LightYellow;
                                    Calendar.Rows[i].Cells[j].Style.ForeColor = Color.Red;
                                }
                                Calendar.Rows[i].Cells[j].ToolTipText = item.Value;
                                Calendar.Rows[i].Cells[j].Tag = item.Value;
                            }
                        }
                    }
                }
            }
        }

        private void HighlightDays() {
           var files = Directory.GetFiles(Application.StartupPath, "*.highlight", SearchOption.TopDirectoryOnly);
           int year = 0;
           int month = 0;
            foreach (var item in files)
           {
               string yearStr = File.ReadAllText(item, Encoding.ASCII);
               string[] monthArray = yearStr.Split('|');
               foreach (var item1 in monthArray)
               {
                   string[] daysArray = item1.Split(',');
                   year = Convert.ToInt32(daysArray[0]);
                   month = Convert.ToInt32(daysArray[1]);

                   for (int i = 2; i < daysArray.Length - 1; i += 2)
                   {
                       var dict = new Dictionary<int, string>();
                       dict.Add(Convert.ToInt32(daysArray[i]), daysArray[i + 1].ToString());
                       DayHighlighter(year, month, dict);
                   }
               }
           }        
        }

        // Selecting Today
        private void SelectToday() {
            Calendar.Rows[0].Cells[0].Selected = false;
            int x = 5;
            if (Calendar.Rows.Count == 6)
            {
                x = 6;
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (Calendar.Rows[i].Cells[j].Value.ToString() != emptyCellSymbol)
                    {
                        if (Convert.ToInt32(Calendar.Rows[i].Cells[j].Value) == DateTime.Now.Day)
                        {
                            Calendar.Rows[i].Cells[j].Selected = true;
                        }
                    }
                }
            }
        }

        private void ShowDateDetails(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                label2.Text = Convert.ToString(Calendar.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
            }
        }
        /// <summary>
        /// RefreshCalendar method refills cells with CreateCalendar
        /// </summary>
        public void RefreshCalendar() {
            CreateCalendar(DateTime.Now);
        }

        private void Calendar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDateDetails(e);
        }

        private void Calendar_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ShowDateDetails(e);
        }

        private void AddWeekNumbers(DateTime dt) {
            var weekdatetime = dt;
            for (int i = 0; i < 6; i++)
            {
                Calendar.Rows[i].HeaderCell.Value = Convert.ToString(WeekNumber.GetISO8601WeekNumber(weekdatetime));
                weekdatetime = weekdatetime.AddDays(7);
            }
        }
}

    //  Static class for splitting an array into n parts
    static class ArraySplit
    {
        /// <summary>
        /// Split arrey an array into n parts
        /// </summary>
        /// <param name="ArrInput">Array to be split</param>
        /// <param name="n_column">Number of parts</param>
        /// <returns>An array of string</returns>
        public static string[] SplitArray(string[] ArrInput, int n_column)
        {
            string[] OutPut = new string[n_column];
            int NItem = ArrInput.Length;
            int ItemsForColum = NItem / n_column;
            int _total = ItemsForColum * n_column;
            int MissElement = NItem - _total;

            int[] _Arr = new int[n_column];
            for (int i = 0; i < n_column; i++)
            {
                int AddOne = (i < MissElement) ? 1 : 0;
                _Arr[i] = ItemsForColum + AddOne;
            }

            int offset = 0;
            for (int Row = 0; Row < n_column; Row++)
            {
                for (int i = 0; i < _Arr[Row]; i++)
                {
                    OutPut[Row] += ArrInput[i + offset] + ",";
                }
                offset += _Arr[Row];
            }
            return OutPut;
        }
    }

    // Static class for getting week numbers
    static class WeekNumber {
        public static int GetISO8601WeekNumber(DateTime dt) {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
            if (day>=DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                dt = dt.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
