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
using System.Runtime.InteropServices;

namespace NekoDrive.Controls
{
   internal class NativeMethods
   {
      private NativeMethods() {}

      [DllImport( "user32.dll" )]
      public static extern IntPtr GetWindowDC( IntPtr hWnd );

      [DllImport( "user32.dll" )]
      public static extern int ReleaseDC( IntPtr hWnd, IntPtr hDC );

      [DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool GetTextMetrics( IntPtr hdc, out TEXTMETRIC lptm );

      [DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
      public static extern IntPtr SelectObject( IntPtr hdc, IntPtr hgdiobj );

      [DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
      [return : MarshalAs( UnmanagedType.Bool)]
      public static extern bool DeleteObject( IntPtr hdc );

      [Serializable, StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
      public struct TEXTMETRIC
      {
         public int tmHeight;
         public int tmAscent;
         public int tmDescent;
         public int tmInternalLeading;
         public int tmExternalLeading;
         public int tmAveCharWidth;
         public int tmMaxCharWidth;
         public int tmWeight;
         public int tmOverhang;
         public int tmDigitizedAspectX;
         public int tmDigitizedAspectY;
         public char tmFirstChar;
         public char tmLastChar;
         public char tmDefaultChar;
         public char tmBreakChar;
         public byte tmItalic;
         public byte tmUnderlined;
         public byte tmStruckOut;
         public byte tmPitchAndFamily;
         public byte tmCharSet;
      }

   }
}
