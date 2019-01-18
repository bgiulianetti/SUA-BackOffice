using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Utilities
{
    using System;
    using System.IO;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class PDFCalendar
    {
        const int UPPER_RIGHT_X = 792;
        const int UPPER_RIGHT_Y = 612;
        const int DAY_HIGHLIGHT_WIDTH = 15;
        const int DAY_HIGHLIGHT_CORNER = 4;
        private DateTime _dt;
        private int _rows, _first_offset_of_month, _days_in_month;
        private static float _cell_leading;
        private PdfPTable _ppt;
        private static Font _font_day, _font_event_even, _font_event_odd;
        // ----------------------------------------  
        public PDFCalendar(int year, int month)
        {
            _init_fonts();
            _cell_leading = _font_day.Size - 1;
            _init_calendar_dates(year, month);
            _init_table();
        }
        // ----------------------------------------  
        private void _init_fonts()
        {
            _font_day = new Font(Font.FontFamily.HELVETICA, 8);
            _font_event_even = new Font(_font_day);
            _font_event_odd = new Font(_font_day);
            _font_day.SetStyle(Font.BOLD);
            _font_event_odd.SetColor(0, 0, 255);
        }
        // ----------------------------------------  
        // calculate where all the days go
        private void _init_calendar_dates(int year, int month)
        {
            _dt = new DateTime(year, month, 1);
            _first_offset_of_month = (int)_dt.DayOfWeek;
            _days_in_month = DateTime.DaysInMonth(_dt.Year, _dt.Month);
            int row_days = _first_offset_of_month + _days_in_month;
            _rows = row_days > 28 && row_days <= 35
              ? 5 : row_days > 35
                ? 6 : 4;
        }
        // ----------------------------------------  
        // initialize calendar headings
        private void _init_table()
        {
            _ppt = new PdfPTable(7);
            PdfPCell table_header = new PdfPCell();
            table_header.GrayFill = 0.8F;
            table_header.HorizontalAlignment = Element.ALIGN_CENTER;
            // row1 => month, year
            table_header.Phrase = new Phrase(_dt.ToString("y"), _font_day);
            table_header.Colspan = 7;
            _ppt.AddCell(table_header);
            // row2 => days of week
            string[] days = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            table_header.Colspan = 1;
            foreach (string day in days)
            {
                table_header.Phrase = new Phrase(day, _font_day);
                _ppt.AddCell(table_header);
            }
            _ppt.WidthPercentage = 100;
        }
        // ----------------------------------------  
        // write the table
        public void create(string filePath)
        {
            var document = new Document();
            document.SetPageSize(new Rectangle(UPPER_RIGHT_X, UPPER_RIGHT_Y));
            try
            {
                FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                PdfWriter writer = PdfWriter.GetInstance(document, file);
                document.Open();
                Document.Compress = false; // debugging
                                           // calculate fixed, equal height for all cells (rows)
                float height = (UPPER_RIGHT_Y - document.TopMargin - document.BottomMargin - 25) / _rows;
                PdfPCell day = new PdfPCell();
                day.PaddingTop = 0;
                _add_event ae = new _add_event();
                int count = 0;
                int day_counter = 0;
                int last_offset_of_month = _first_offset_of_month + _days_in_month;

                for (int i = 0; i < _rows; ++i)
                {
                    // set fixed row height
                    day.FixedHeight = height;
                    for (int j = 0; j < 7; ++j)
                    {
                        string daynum = count >= _first_offset_of_month && count < last_offset_of_month ? (++day_counter).ToString() : "";
                        // we're re-using the CellEvent object, so reset it when not needed!
                        day.CellEvent = daynum != "" && day_counter % 5 == 0 ? ae : null;
                        day.Phrase = new Phrase(daynum, _font_day);
                        _ppt.AddCell(day);
                        ++count;
                    }
                }
                document.Add(_ppt);
            }
            catch
            { }
            finally
            {
                if (document != null && document.IsOpen()) document.Close();
            }
        }
        // ----------------------------------------
        // custom functionality when writing each day's event to each cell
        private class _add_event : IPdfPCellEvent
        {
            public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
            {
                // rounded rectangle, highlighted days with event(s)
                PdfContentByte cbb = canvases[PdfPTable.BACKGROUNDCANVAS];
                cbb.SetColorStroke(new GrayColor(0.4f));
                cbb.SetColorFill(BaseColor.YELLOW);
                cbb.RoundRectangle(
                  position.Left,        // lower-left x-coordinate
                  position.Top -        // lower-left y-coordinate
                      _cell_leading - 3,
                  DAY_HIGHLIGHT_WIDTH,  // highlight rectangle width
                  _cell_leading + 3,    // highlight rectangle height
                  DAY_HIGHLIGHT_CORNER  // corner "roundness"
                );
                cbb.FillStroke();
                PdfContentByte cb = canvases[PdfPTable.TEXTCANVAS];
                ColumnText ct = new ColumnText(cb);
                // set exact coordinates for ColumnText
                ct.SetSimpleColumn(
                  position.Left + 2,  // lower-left x; add some padding
                  position.Bottom,    // lower-left y
                  position.Right,     // upper-right x
                  position.Top        // upper-right x; adjust for existing content
                      - _cell_leading - 3
                );
                string[] lines = {
                    "event one",
                    "event two",
                    "event three continuing over more than one line.",
                    "event four"
      };
                // visually separate events by font color
                for (int i = 0; i < lines.Length; ++i)
                {
                    ct.AddElement(new Phrase(
                      _cell_leading,
                      lines[i],
                      i % 2 == 0 ? _font_event_even : _font_event_odd
                    ));
                }
                ct.Go();
            }
        }
    }
}