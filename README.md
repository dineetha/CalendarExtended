# CalendarExtended
Custom User Control Calendar with extra customizing and localizing facilities written in C# for Windows Forms.
Features include bigger customizable calendar control, highlighted holidays and special dates.
This source code is distributed as free and open source for educational purposes.

To highlight dates you have to add files to assembly directory with following format.
```
File Name and Extension - Year.highlight
File Content - year,month,date,holiday_name|year,month,date,holiday_name|year,month,date,holiday_name
```

You can add custom dates and events. Of course you can change file extension name and format by editing source code if you like.

```
For example if you want to highlight 2017-07-01 and 2017-09-15
File Name - 2017.highlight
File Content - 2017,7,1,holiday|2020,09,15,event
```
