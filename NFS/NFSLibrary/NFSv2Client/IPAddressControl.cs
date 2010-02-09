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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms; 
using System.Windows.Forms.VisualStyles;


namespace NFSv2Client
{
   [DesignerAttribute( typeof( IPAddressControlDesigner ) )]
   public partial class
   IPAddressControl : UserControl
   {
      #region Public Constants

      public const int NumberOfFields = 4;

      #endregion // Public Constants

      #region Public Events

      public event EventHandler<FieldChangedEventArgs> FieldChangedEvent;

      #endregion //Public Events

      #region Public Properties

      [DefaultValue(true)]
      public override bool AutoSize
      {
         get
         {
            return base.AutoSize;
         }
         set
         {
            base.AutoSize = value;

            if ( AutoSize )
            {
               AdjustSize();
            }
         }
      }

      public int Baseline
      {
         get 
         {
            NativeMethods.TEXTMETRIC textMetric = GetTextMetrics( Handle, Font );

            int offset = textMetric.tmAscent + 1;

            switch ( BorderStyle )
            {
               case BorderStyle.Fixed3D:
                  offset += Fixed3DOffset.Height;
                  break;
               case BorderStyle.FixedSingle:
                  offset += FixedSingleOffset.Height;
                  break;
            }

            return offset;
         }
      }

      public bool Blank
      {
         get
         {
            foreach ( FieldControl fc in _fieldControls )
            {
               if ( !fc.Blank )
               {
                  return false;
               }
            }

            return true;
         }
      }

      [DefaultValue(BorderStyle.Fixed3D)]
      public new BorderStyle BorderStyle
      {
         get
         {
            return _borderStyle;
         }
         set
         {
            _borderStyle = value;
            AdjustSize();
            Invalidate();
         }
      }

      public override bool Focused
      {
         get
         {
            foreach ( FieldControl fc in _fieldControls )
            {
               if ( fc.Focused )
               {
                  return true;
               }
            }

            return false;
         }
      }

      public override Size MinimumSize
      {
         get
         {
            return CalculateMinimumSize();
         }
      }

      public bool ReadOnly
      {
         get
         {
            return _readOnly;
         }
         set
         {
            _readOnly = value;

            foreach ( FieldControl fc in _fieldControls )
            {
               fc.ReadOnly = _readOnly;
            }

            foreach ( DotControl dc in _dotControls )
            {
               dc.ReadOnly = _readOnly;
            }

            Invalidate();
         }
      }

      [Browsable(true)]
      public override string Text
      {
         get
         {
            StringBuilder sb = new StringBuilder(); ;

            for ( int index = 0; index < _fieldControls.Length; ++index )
            {
               sb.Append( _fieldControls[index].Text );

               if ( index < _dotControls.Length )
               {
                  sb.Append( _dotControls[index].Text );
               }
            }

            return sb.ToString();
         }
         set
         {
            Parse( value );
         }
      }

      #endregion // Public Properties

      #region Public Methods

      public void Clear()
      {
         foreach ( FieldControl fc in _fieldControls )
         {
            fc.Clear();
         }
      }

      public byte[] GetAddressBytes()
      {
         byte[] bytes = new byte[NumberOfFields];

         for ( int index = 0; index < NumberOfFields; ++index )
         {
            bytes[index] = _fieldControls[index].Value;
         }

         return bytes;
      }

      public void SetAddressBytes( byte[] bytes )
      {
         Clear();

         int length = Math.Min( NumberOfFields, bytes.Length );

         for ( int i = 0; i < length; ++i )
         {
            _fieldControls[i].Text = bytes[i].ToString( CultureInfo.InvariantCulture );
         }
      }

      public void SetFieldFocus( int fieldId )
      {
         if ( ( fieldId >= 0 ) && ( fieldId < NumberOfFields ) )
         {
            _fieldControls[fieldId].TakeFocus( Direction.Forward, Selection.All );
         }
      }

      public void SetFieldRange( int fieldId, byte rangeLower, byte rangeUpper )
      {
         if ( ( fieldId >= 0 ) && ( fieldId < NumberOfFields ) )
         {
            _fieldControls[fieldId].RangeLower = rangeLower;
            _fieldControls[fieldId].RangeUpper = rangeUpper;
         }
      }

      public override string ToString()
      {
         StringBuilder sb = new StringBuilder();

         for ( int index = 0; index < NumberOfFields; ++index )
         {
            sb.Append( _fieldControls[index].ToString() );

            if ( index < _dotControls.Length )
            {
               sb.Append( _dotControls[index].ToString() );
            }
         }

         return sb.ToString();
      }

      #endregion Public Methods

      #region Constructors

      public IPAddressControl()
      {
         BackColor = SystemColors.Window;

         ResetBackColorChanged();

         for ( int index = 0; index < _fieldControls.Length; ++index )
         {
            _fieldControls[index] = new FieldControl();

            _fieldControls[index].CedeFocusEvent += new EventHandler<CedeFocusEventArgs>( this.OnCedeFocus );
            _fieldControls[index].FieldId = index;
            _fieldControls[index].Name = "FieldControl" + index.ToString( CultureInfo.InvariantCulture );
            _fieldControls[index].Parent = this;
            _fieldControls[index].SpecialKeyEvent += new EventHandler<SpecialKeyEventArgs>( this.OnSpecialKey );
            _fieldControls[index].TextChangedEvent += new EventHandler<TextChangedEventArgs>( this.OnFieldTextChanged );

            Controls.Add( _fieldControls[index] );

            if ( index < ( NumberOfFields - 1 ) )
            {
               _dotControls[index] = new DotControl();

               _dotControls[index].Name = "DotControl" + index.ToString( CultureInfo.InvariantCulture );
               _dotControls[index].Parent = this;

               Controls.Add( _dotControls[index] );
            }
         }

         InitializeComponent();

         SetStyle( ControlStyles.AllPaintingInWmPaint, true );
         SetStyle( ControlStyles.ContainerControl, true );
         SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
         SetStyle( ControlStyles.ResizeRedraw, true );
         SetStyle( ControlStyles.Selectable, true );
         SetStyle( ControlStyles.UserPaint, true );

         _referenceTextBox.AutoSize = true;

         Size = MinimumSize;
         AutoSize = true;

         DragEnter += new DragEventHandler( IPAddressControl_DragEnter );
         DragDrop += new DragEventHandler( IPAddressControl_DragDrop );
      }

      #endregion // Constructors

      #region Protected Methods

      protected override void OnBackColorChanged( EventArgs e )
      {
         base.OnBackColorChanged( e );
         _backColorChanged = true;
      }

      protected override void OnFontChanged( EventArgs e )
      {
         base.OnFontChanged( e );
         AdjustSize();
      }

      protected override void OnGotFocus( EventArgs e )
      {
         base.OnGotFocus( e );
         _fieldControls[0].TakeFocus( Direction.Forward, Selection.All );
      }

      protected override void OnMouseEnter( EventArgs e )
      {
         base.OnMouseEnter( e );
         Cursor = Cursors.IBeam;
      }

      protected override void OnPaint( PaintEventArgs e )
      {
         base.OnPaint( e );

         Color backColor = BackColor;

         if ( !_backColorChanged )
         {
            if ( !Enabled || ReadOnly )
            {
               backColor = SystemColors.Control;
            }
         }

         e.Graphics.FillRectangle( new SolidBrush( backColor ), ClientRectangle );

         Rectangle rectBorder = new Rectangle( ClientRectangle.Left, ClientRectangle.Top,
            ClientRectangle.Width - 1, ClientRectangle.Height - 1 );

         switch ( BorderStyle )
         {
            case BorderStyle.Fixed3D:

               if ( Application.RenderWithVisualStyles )
               {
                  ControlPaint.DrawVisualStyleBorder( e.Graphics, rectBorder );
               }
               else
               {
                  ControlPaint.DrawBorder3D( e.Graphics, ClientRectangle, Border3DStyle.Sunken );
               }
               break;

            case BorderStyle.FixedSingle:

               ControlPaint.DrawBorder( e.Graphics, ClientRectangle,
                  SystemColors.WindowFrame, ButtonBorderStyle.Solid );
               break;
         }
      }

      protected override void OnSizeChanged( EventArgs e )
      {
         base.OnSizeChanged( e );
         AdjustSize();
      }

      #endregion // Protected Methods

      #region Private Methods

      private void AdjustSize()
      {
         Size newSize = MinimumSize;

         if ( Size.Width > newSize.Width )
         {
            newSize.Width = Size.Width;
         }

         if ( Size.Height > newSize.Height )
         {
            newSize.Height = Size.Height;
         }

         if ( AutoSize )
         {
            Size = new Size( MinimumSize.Width, MinimumSize.Height );
         }
         else
         {
            Size = newSize;
         }

         LayoutControls();
      }

      private Size CalculateMinimumSize()
      {
         Size minimumSize = new Size( 0, 0 );

         foreach ( FieldControl fc in _fieldControls )
         {
            minimumSize.Width += fc.Size.Width;
            minimumSize.Height = Math.Max( minimumSize.Height, fc.Size.Height );
         }

         foreach ( DotControl dc in _dotControls )
         {
            minimumSize.Width += dc.Size.Width;
            minimumSize.Height = Math.Max( minimumSize.Height, dc.Size.Height );
         }

         switch ( BorderStyle )
         {
            case BorderStyle.Fixed3D:
               minimumSize.Width += 6;
               minimumSize.Height += ( GetSuggestedHeight() - minimumSize.Height );
               break;
            case BorderStyle.FixedSingle:
               minimumSize.Width += 4;
               minimumSize.Height += ( GetSuggestedHeight() - minimumSize.Height );
               break;
         }

         return minimumSize;
      }

      private int GetSuggestedHeight()
      {
         _referenceTextBox.BorderStyle = BorderStyle;
         _referenceTextBox.Font = Font;
         return _referenceTextBox.Height;
      }

      private static NativeMethods.TEXTMETRIC GetTextMetrics( IntPtr hwnd, Font font )
      {
         IntPtr hdc = NativeMethods.GetWindowDC( hwnd );

         NativeMethods.TEXTMETRIC textMetric;
         IntPtr hFont = font.ToHfont();

         try
         {
            IntPtr hFontPreviouse = NativeMethods.SelectObject( hdc, hFont );
            NativeMethods.GetTextMetrics( hdc, out textMetric );
            NativeMethods.SelectObject( hdc, hFontPreviouse );
         }
         finally
         {
            NativeMethods.ReleaseDC( hwnd, hdc );
            NativeMethods.DeleteObject( hFont );
         }

         return textMetric;
      }

      private void IPAddressControl_DragDrop( object sender, System.Windows.Forms.DragEventArgs e )
      {
         Text = e.Data.GetData( DataFormats.Text ).ToString();
      }

      private void IPAddressControl_DragEnter( object sender, System.Windows.Forms.DragEventArgs e )
      {
         if ( e.Data.GetDataPresent( DataFormats.Text ) )
         {
            e.Effect = DragDropEffects.Copy;
         }
         else
         {
            e.Effect = DragDropEffects.None;
         }
      }

      private void LayoutControls()
      {
         SuspendLayout();

         int difference = this.Size.Width - MinimumSize.Width;

         Debug.Assert( difference >= 0 );

         int numOffsets = _fieldControls.Length + _dotControls.Length + 1;

         int div = difference / ( numOffsets );
         int mod = difference % ( numOffsets );

         int[] offsets = new int[numOffsets];

         for ( int index = 0; index < numOffsets; ++index )
         {
            offsets[index] = div;

            if ( index < mod )
            {
               ++offsets[index];
            }
         }

         int x = 0;
         int y = 0;

         switch ( this.BorderStyle )
         {
            case BorderStyle.Fixed3D:
               x = Fixed3DOffset.Width;
               y = Fixed3DOffset.Height;
               break;
            case BorderStyle.FixedSingle:
               x = FixedSingleOffset.Width;
               y = FixedSingleOffset.Height;
               break;
         }

         int offsetIndex = 0;

         x += offsets[offsetIndex++];

         for ( int i = 0; i < _fieldControls.Length; ++i )
         {
            _fieldControls[i].Location = new Point( x, y );

            x += _fieldControls[i].Size.Width;

            if ( i < _dotControls.Length )
            {
               x += offsets[offsetIndex++];
               _dotControls[i].Location = new Point( x, y );
               x += _dotControls[i].Size.Width;
               x += offsets[offsetIndex++];
            }
         }

         ResumeLayout( false );
      }

      private void OnCedeFocus( Object sender, CedeFocusEventArgs e )
      {
         if ( ( e.Direction == Direction.Reverse && e.FieldId == 0 ) ||
              ( e.Direction == Direction.Forward && e.FieldId == ( NumberOfFields - 1 ) ) )
         {
            return;
         }

         int fieldId = e.FieldId;

         if ( e.Direction == Direction.Forward )
         {
            ++fieldId;
         }
         else
         {
            --fieldId;
         }

         _fieldControls[fieldId].TakeFocus( e.Direction, e.Selection );
      }

      private void OnFieldTextChanged( Object sender, TextChangedEventArgs e )
      {
         if ( null != FieldChangedEvent )
         {
            FieldChangedEventArgs args = new FieldChangedEventArgs();
            args.FieldId = e.FieldId;
            args.Text = e.Text;
            FieldChangedEvent( this, args );
         }

         OnTextChanged( EventArgs.Empty );
      }

      private void OnSpecialKey( Object sender, SpecialKeyEventArgs e )
      {
         switch ( e.KeyCode )
         {
            case Keys.Back:

               if ( e.FieldId > 0 )
               {
                  _fieldControls[e.FieldId-1].HandleSpecialKey( Keys.Back );
               }
               break;

            case Keys.Home:

               _fieldControls[0].TakeFocus( Direction.Forward, Selection.None );
               break;

            case Keys.End:

               _fieldControls[NumberOfFields - 1].TakeFocus( Direction.Reverse, Selection.None );
               break;
         }
      }

      private void Parse( String text )
      {
         Clear();

         if ( null == text )
         {
            return;
         }

         int textIndex = 0;

         int index = 0;

         for ( index = 0; index < _dotControls.Length; ++index )
         {
            int findIndex = text.IndexOf( _dotControls[index].Text, textIndex );

            if ( findIndex >= 0 )
            {
               _fieldControls[index].Text = text.Substring( textIndex, findIndex - textIndex );
               textIndex = findIndex + _dotControls[index].Text.Length;
            }
            else
            {
               break;
            }
         }

         _fieldControls[index].Text = text.Substring( textIndex );
      }

      // a hack to remove an FxCop warning
      private void ResetBackColorChanged()
      {
         _backColorChanged = false;
      }

      #endregion Private Methods

      #region Private Data

      private bool _readOnly;
      private BorderStyle _borderStyle = BorderStyle.Fixed3D;
      private FieldControl[] _fieldControls = new FieldControl[NumberOfFields];
      private DotControl[] _dotControls = new DotControl[NumberOfFields - 1];
      private bool _backColorChanged;

      private Size Fixed3DOffset = new Size( 3, 3 );
      private Size FixedSingleOffset = new Size( 2, 2 );

      private TextBox _referenceTextBox = new TextBox();

      #endregion  // Private Data
   }

   public class FieldChangedEventArgs : EventArgs
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
}