// Copyright (c) 2007 Michael Chapman

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace NFSv2Client
{
   internal enum Direction
   {
      Forward,
      Reverse
   }

   internal enum Selection
   {
      None,
      All
   }

   internal class CedeFocusEventArgs : EventArgs
   {
      private int _fieldId;
      private Direction _direction;
      private Selection _selection;

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public Direction Direction
      {
         get
         {
            return _direction;
         }
         set
         {
            _direction = value;
         }
      }

      public Selection Selection
      {
         get
         {
            return _selection;
         }
         set
         {
            _selection = value;
         }
      }
   }

   internal class SpecialKeyEventArgs : EventArgs
   {
      private int _fieldId;
      private Keys _keyCode;

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public Keys KeyCode
      {
         get
         {
            return _keyCode;
         }
         set
         {
            _keyCode = value;
         }
      }
   }

   internal class TextChangedEventArgs : EventArgs
   {
      private int _fieldId;
      private String _text;

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public String Text
      {
         get
         {
            return _text;
         }
         set
         {
            _text = value;
         }
      }
   }

   internal class FieldControl : TextBox
   {
      #region Public Constants

      public const byte MinimumValue = 0;
      public const byte MaximumValue = 255;

      #endregion // Public Constants

      #region Public Events

      public event EventHandler<CedeFocusEventArgs> CedeFocusEvent;
      public event EventHandler<SpecialKeyEventArgs> SpecialKeyEvent;
      public event EventHandler<TextChangedEventArgs> TextChangedEvent;

      #endregion // Public Events

      #region Public Properties

      public bool Blank
      {
         get
         {
            return ( Text.Length == 0 );
         }
      }

      public int FieldId
      {
         get
         {
            return _fieldId;
         }
         set
         {
            _fieldId = value;
         }
      }

      public override Size MinimumSize
      {
         get
         {
            return TextRenderer.MeasureText( "255", Font );
         }
      }

      public byte RangeLower
      {
         get
         {
            return _rangeLower;
         }
         set
         {
            if ( value < MinimumValue )
            {
               _rangeLower = MinimumValue;
            }
            else if ( value > _rangeUpper )
            {
               _rangeLower = _rangeUpper;
            }
            else
            {
               _rangeLower = value;
            }

            if ( Value < _rangeLower )
            {
               Text = _rangeLower.ToString( CultureInfo.InvariantCulture );
            }
         }
      }

      public byte RangeUpper
      {
         get
         {
            return _rangeUpper;
         }
         set
         {
            if ( value < _rangeLower )
            {
               _rangeUpper = _rangeLower;
            }
            else if ( value > MaximumValue )
            {
               _rangeUpper = MaximumValue;
            }
            else
            {
               _rangeUpper = value;
            }

            if ( Value > _rangeUpper )
            {
               Text = _rangeUpper.ToString( CultureInfo.InvariantCulture );
            }
         }
      }

      public byte Value
      {
         get
         {
            byte result;

            if ( !Byte.TryParse( Text, out result ) )
            {
               result = RangeLower;
            }

            return result;
         }
      }

      #endregion // Public Properties

      #region Public Methods

      public void HandleSpecialKey( Keys keyCode )
      {
         switch ( keyCode )
         {
            case Keys.Back:
               Focus();
               if ( TextLength > 0  )
               {
                  int newLength = TextLength - 1;
                  Text = Text.Substring( 0, newLength );
               }
               SelectionStart = TextLength;
               break;
         }
      }

      public void TakeFocus( Direction direction, Selection selection )
      {
         Focus();

         if ( selection == Selection.All )
         {
            SelectionStart = 0;
            SelectionLength = TextLength;
         }
         else
            if ( direction == Direction.Forward )
            {
               SelectionStart = 0;
            }
            else
            {
               SelectionStart = TextLength;
            }
      }

      public override string ToString()
      {
         return Value.ToString( CultureInfo.InvariantCulture );
      }

      #endregion // Public Methods

      #region Constructors

      public FieldControl()
      {
         BorderStyle = BorderStyle.None;
         MaxLength = 3;
         Size = MinimumSize;
         TabStop = false;
         TextAlign = HorizontalAlignment.Center;
      }

      #endregion //Constructors

      #region Protected Methods

      protected override void OnKeyDown( KeyEventArgs e )
      {
         base.OnKeyDown( e );

         if ( e.KeyCode == Keys.Home || 
              e.KeyCode == Keys.End )
         {
            SendSpecialKeyEvent( e.KeyCode );
            return;
         }

         _invalidKeyDown = false;

         if ( !NumericKeyDown( e ) )
         {
            if ( !ValidKeyDown( e ) )
            {
               _invalidKeyDown = true;
            }
         }

         if ( IsCedeFocusKey( e.KeyCode ) )
         {
            SendCedeFocusEvent( Direction.Forward, Selection.All );
         }

         if ( e.KeyCode == Keys.Left || e.KeyCode == Keys.Up )
         {
            if ( e.Modifiers == Keys.Control )
            {
               SendCedeFocusEvent( Direction.Reverse, Selection.All );
            }
            else
               if ( SelectionLength == 0 && SelectionStart == 0 )
               {
                  SendCedeFocusEvent( Direction.Reverse, Selection.None );
               }
         }

         if ( e.KeyCode == Keys.Back )
         {
            HandleBackKey( e );
         }

         if ( e.KeyCode == Keys.Delete )
         {
            if ( SelectionStart < TextLength && TextLength > 0 )
            {
               int index = SelectionStart;
               Text = Text.Remove( SelectionStart, ( SelectionLength > 0 ) ? SelectionLength : 1 );
               SelectionStart = index;
               e.SuppressKeyPress = true;
            }
         }

         if ( e.KeyCode == Keys.Right || e.KeyCode == Keys.Down )
         {
            if ( e.Modifiers == Keys.Control )
            {
               SendCedeFocusEvent( Direction.Forward, Selection.All );
            }
            else if ( SelectionLength == 0 && SelectionStart == Text.Length )
            {
               SendCedeFocusEvent( Direction.Forward, Selection.None );
            }
         }
      }

      protected override void OnKeyPress( KeyPressEventArgs e )
      {
         if ( _invalidKeyDown )
         {
            e.Handled = true;
         }

         base.OnKeyPress( e );
      }

      protected override void OnParentBackColorChanged( EventArgs e )
      {
         base.OnParentBackColorChanged( e );
         BackColor = Parent.BackColor;
      }

      protected override void OnParentForeColorChanged( EventArgs e )
      {
         base.OnParentForeColorChanged( e );
         ForeColor = Parent.ForeColor;
      }

      protected override void OnSizeChanged( EventArgs e )
      {
         base.OnSizeChanged( e );
         Size = MinimumSize;
      }

      protected override void OnTextChanged( EventArgs e )
      {
         base.OnTextChanged( e );

         if ( !Blank )
         {
            int val;
            if ( !Int32.TryParse( Text, out val ) )
            {
               Text = String.Empty;
            }
            else
            {
               if ( val > RangeUpper )
               {
                  Text = RangeUpper.ToString( CultureInfo.InvariantCulture );
               }
               else
               {
                  Text = val.ToString( CultureInfo.InvariantCulture );
               }
            }
         }

         if ( null != TextChangedEvent )
         {
            TextChangedEventArgs args = new TextChangedEventArgs();
            args.FieldId = FieldId;
            args.Text = Text;
            TextChangedEvent( this, args );
         }

         if ( Text.Length == MaxLength && Focused )
         {
            SendCedeFocusEvent( Direction.Forward, Selection.All );
         }
      }

      protected override void OnValidating( System.ComponentModel.CancelEventArgs e )
      {
         base.OnValidating( e );

         if ( !Blank )
         {
            if ( Value < RangeLower )
            {
               Text = RangeLower.ToString( CultureInfo.InvariantCulture );
            }
         }
      }

      #endregion // Protected Methods

      #region Private Methods

      private bool IsCedeFocusKey( Keys keyCode )
      {
         if ( keyCode == Keys.OemPeriod ||
              keyCode == Keys.Decimal ||
              keyCode == Keys.Space )

         {
            if ( TextLength != 0 && SelectionLength == 0 && SelectionStart != 0 )
            {
               return true;
            }
         }

         return false;
      }

      private void HandleBackKey( KeyEventArgs e )
      {
         if ( TextLength == 0 || ( SelectionStart == 0 && SelectionLength == 0 ) )
         {
            SendSpecialKeyEvent( Keys.Back );
            e.SuppressKeyPress = true;
         }
         else if ( SelectionLength > 0 )
         {
            int index = SelectionStart;
            Text = Text.Remove( SelectionStart, SelectionLength );
            SelectionStart = index;
            e.SuppressKeyPress = true;
         }
         else if ( SelectionStart > 0 )
         {
            int index = --SelectionStart;
            Text = Text.Remove( SelectionStart, 1 );
            SelectionStart = index;
            e.SuppressKeyPress = true;
         }
      }

      private static bool NumericKeyDown( KeyEventArgs e )
      {
         if ( e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9 )
         {
            if ( e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9 )
            {
               return false;
            }
         }

         return true;
      }

      private void SendCedeFocusEvent( Direction direction, Selection selection )
      {
         if ( null != CedeFocusEvent )
         {
            CedeFocusEventArgs args = new CedeFocusEventArgs();
            args.FieldId = FieldId;
            args.Direction = direction;
            args.Selection = selection;
            CedeFocusEvent( this, args );
         }
      }

      private void SendSpecialKeyEvent( Keys keyCode )
      {
         if ( null != SpecialKeyEvent )
         {
            SpecialKeyEventArgs args = new SpecialKeyEventArgs();

            args.FieldId = FieldId;
            args.KeyCode = keyCode;
            SpecialKeyEvent( this, args );
         }
      }

      private static bool ValidKeyDown( KeyEventArgs e )
      {
         if ( e.KeyCode == Keys.Back ||
              e.KeyCode == Keys.Delete )
         {
            return true;
         }
         else if ( e.Modifiers == Keys.Control &&
                   ( e.KeyCode == Keys.C ||
                     e.KeyCode == Keys.V ||
                     e.KeyCode == Keys.X ) )
         {
            return true;
         }

         return false;
      }

      #endregion // Private Methods

      #region Private Data

      private int _fieldId = -1;
      private bool _invalidKeyDown;
      private byte _rangeLower; // = MinimumValue;  // this is removed for FxCop approval
      private byte _rangeUpper = MaximumValue;

      #endregion // Private Data
   }
}
