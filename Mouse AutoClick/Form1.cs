using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mouse_AutoClick
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private bool flag;
        private Point p;
        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKeys();
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);

            TopMost = true;
            flag = false;
           // var th = new System.Threading.Timer(_ => ss(),null,TimeSpan.Zero,TimeSpan.FromMilliseconds(1000));
            


        }

        private void ss()
        {
            label1.Invoke((MethodInvoker)(() => label1.Text = "X = " + Cursor.Position.X + "  Y = " + Cursor.Position.Y));


            //CURSORINFO pci;
            //pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            //GetCursorInfo(out pci);

            //label3.Invoke((MethodInvoker)(() => label3.Text = pci.hCursor.ToString()));



            if (flag)
            {

                SetCursorPos(p.X, p.Y);
                SetCursorPos(p.X + 2, p.Y+2);
                SetCursorPos(p.X, p.Y);
                if (IsHandCursor())
                {
                    Thread.Sleep(500);
                    clicker(Cursor.Position.X, Cursor.Position.Y);

                }
            }




        }

        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwflags, int dx, int dy, int cbuttons, int dwExtraInfo);


        #region WindowsAPI
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion


        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        private IntPtr thisWindow;



        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
                                        // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
                                        //    0             The cursor is hidden.
                                        //    CURSOR_SHOWING    The cursor is showing.
            public IntPtr hCursor;          // Handle to the cursor. 
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        public void clicker(int x, int y)
        {


            
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            Thread.Sleep(100);
        }

        private bool IsWaitCursor()
        {
            var h = Cursors.WaitCursor.Handle;

            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);

            return pci.hCursor == h;
        }

        private bool IsDefaultCursor()
        {
            var h = Cursors.Default.Handle;

            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);

            return pci.hCursor == h;
        }

        private bool IsHandCursor()
        {
            var h = 65567;

            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);

            return pci.hCursor == (IntPtr)h;
        }



        public enum fsModifiers
        {
            None = 0x0000,
            Alt = 0x0001,
            Control = 0x0002,
            Shift = 0x0004, // Changes!
            Window = 0x0008,
        }



        public void RegisterHotKeys()
        {
            RegisterHotKey(FindWindow(null, "Form1"), 1, (uint)fsModifiers.None, (uint)Keys.F7);
            RegisterHotKey(FindWindow(null, "Form1"), 1, (uint)fsModifiers.None, (uint)Keys.F8);
        }

        public void UnRegisterHotKeys()
        {
            UnregisterHotKey(FindWindow(null, "Form1"), 1);
        }

        protected override void WndProc(ref Message keyPressed)
        {
            if (keyPressed.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)keyPressed.LParam >> 16) & 0xFFFF);
                int modifier = ((int)keyPressed.LParam & 0xFFFF);
                if (key == Keys.F7)
                {
                    p.X = Cursor.Position.X;
                    p.Y = Cursor.Position.Y;
                    Opacity = 0.5;
                    BackColor = Color.Red;
                    timer1.Start();
                    flag = true;
                }
                else if (key == Keys.F8)
                {
                    Opacity = 1;
                    timer1.Stop();
                    BackColor = Color.WhiteSmoke;
                    flag = false;
                }
            }
            base.WndProc(ref keyPressed);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hi");
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnRegisterHotKeys();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {

            if (BackColor == Color.Red)
            {
                BackColor = Color.Green;
            }
            else
            {
                BackColor = Color.Red;

            }


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ss();
        }
    }
}


