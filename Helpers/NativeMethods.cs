using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static class NativeMethods
{
    internal static IntPtr Zero = IntPtr.Zero;

    internal const int GWL_STYLE = -16;


    internal const int SB_HORZ = 0;
    internal const int SB_VERT = 1;
    internal const int SB_BOTH = 3;
    internal const int SB_PAGERIGHT = 3;
    internal const int SB_THUMBPOSITION = 4;
    internal const int SB_THUMBTRACK = 5;

    internal const int WM_HSCROLL = 0x114;
    internal const int WM_VSCROLL = 0x115; //****
    internal const int WM_SCROLL = 276; // Horizontal scroll
    //internal const int WM_VSCROLL = 277; // Vertical scroll

    internal const int SB_LINEUP = 0; // Scrolls one line up
    internal const int SB_LINELEFT = 0;// Scrolls one cell left
    internal const int SB_LINEDOWN = 1; // Scrolls one line down
    internal const int SB_LINERIGHT = 1;// Scrolls one cell right
    internal const int SB_PAGEUP = 2; // Scrolls one page up
    internal const int SB_PAGELEFT = 2;// Scrolls one page left
    internal const int SB_PAGEDOWN = 3; // Scrolls one page down
    internal const int SB_PAGERIGTH = 3; // Scrolls one page right
    internal const int SB_PAGETOP = 6; // Scrolls to the upper left
    internal const int SB_LEFT = 6; // Scrolls to the left
    internal const int SB_PAGEBOTTOM = 7; // Scrolls to the upper right
    internal const int SB_RIGHT = 7; // Scrolls to the right
    internal const int SB_ENDSCROLL = 8; // Ends scroll

    internal const int WS_BORDER = 0x00800000;
    
    internal const int WS_EX_CLIENTEDGE = 0x00000200;
    internal const int WS_HSCROLL = 0x00100000;
    internal const int WS_VSCROLL = 0x00200000;

    internal const int EM_LINESCROLL = 0x00B6;

    internal static int EM_SETCUEBANNER = 0x1501;
    internal static int CB_SETCUEBANNER = 0x1703;

    internal const int LB_ADDSTRING = 0x180;
    internal const int LB_INSERTSTRING = 0x181;
    internal const int LB_DELETESTRING = 0x182;
    internal const int LB_RESETCONTENT = 0x184;

    internal const int WM_NCPAINT = 0x0085;

    internal const int SIF_TRACKPOS = 0x10;
    internal const int SIF_RANGE = 0x1;
    internal const int SIF_POS = 0x4;
    internal const int SIF_PAGE = 0x2;
    internal const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

    //internal struct ScrollInfoStruct
    //{
    //   internal int cbSize;
    //   internal int fMask;
    //   internal int nMin;
    //   internal int nMax;
    //   internal int nPage;
    //   internal int nPos;
    //   internal int nTrackPos;
    //}

    //internal struct DWMCOLORIZATIONPARAMS
    //{
    //   internal uint ColorizationColor,
    //       ColorizationAfterglow,
    //       ColorizationColorBalance,
    //       ColorizationAfterglowBalance,
    //       ColorizationBlurBalance,
    //       ColorizationGlassReflectionIntensity,
    //       ColorizationOpaqueBlend;
    //}

    //internal static Color GetWindowColorizationColor(bool opaque)
    //{
    //   DWMCOLORIZATIONPARAMS args = default;
    //   NativeMethods.DwmGetColorizationParameters(ref args);

    //   return Color.FromArgb(
    //       (byte)(opaque ? 255 : args.ColorizationColor >> 24),
    //       (byte)(args.ColorizationColor >> 16),
    //       (byte)(args.ColorizationColor >> 8),
    //       (byte)args.ColorizationColor
    //   );
    //}


    [DllImport("user32.dll")]
    internal static extern IntPtr GetWindowDC(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

    [Flags()]
    public enum RedrawWindowFlags : uint
    {
        Invalidate = 0X1,
        InternalPaint = 0X2,
        Erase = 0X4,
        Validate = 0X8,
        NoInternalPaint = 0X10,
        NoErase = 0X20,
        NoChildren = 0X40,
        AllChildren = 0X80,
        UpdateNow = 0X100,
        EraseNow = 0X200,
        Frame = 0X400,
        NoFrame = 0X800
    }

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 8)
            return GetWindowLongPtr64(hWnd, nIndex);
        else
            return GetWindowLongPtr32(hWnd, nIndex);
    }

    [DllImport("user32.dll")]
    internal static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

    //[DllImport("user32.dll")]
    //internal static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

    //[DllImport("user32.dll")]
    //internal static extern int GetScrollPos(IntPtr hWnd, int nBar);

    internal static void HideScrollBar(IntPtr handle, int wBar)
    {
        var style = (int)GetWindowLong(handle, GWL_STYLE);

        if (wBar == SB_HORZ)
            style = style & (~WS_HSCROLL);
        else if (wBar == SB_VERT)
            style = style & (~WS_VSCROLL);
        else if (wBar == SB_BOTH)
            style = style & (~WS_HSCROLL) & (~WS_VSCROLL);

        SetWindowLong(handle, GWL_STYLE, (uint)style);
    }

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    internal static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCaretPos(out Point p);

    [DllImport("user32", CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool ShowScrollBar(IntPtr hwnd, int wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CloseClipboard();

    //[DllImport("user32.dll", SetLastError = true)]
    //internal static extern int GetScrollInfo(IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);

    //[DllImport("dwmapi.dll", EntryPoint = "#127")]
    //internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS args);

    internal static void SetCue(this TextBox textBox, string cue)
    {
        SendMessage(textBox.Handle, (uint)EM_SETCUEBANNER, Zero, cue);
    }

    internal static void ClearCue(this TextBox textBox)
    {
        SetCue(textBox, string.Empty);
    }

    internal static void SetCue(this ComboBox comboBox, string cue)
    {
        cue = cue.TrimStart(' ');

        //handing off the reset of the combobox selected value to a delegate method - using methodinvoker on the forms main thread is an efficient to do this
        //see https://msdn.microsoft.com/en-us/library/system.windows.forms.methodinvoker(v=vs.110).aspx
        if (!comboBox.Items.Contains(cue))
            comboBox.FindForm().BeginInvoke((MethodInvoker)delegate { comboBox.Text = cue; });
        else
            comboBox.Text = cue;

        SendMessage(comboBox.Handle, (uint)CB_SETCUEBANNER, Zero, cue);
    }

    internal static void ClearCue(this ComboBox comboBox)
    {
        SetCue(comboBox, string.Empty);
    }
}
