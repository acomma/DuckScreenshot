using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DuckScreenshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int HotKeyId = 0x9001;     // 全局热键 ID
        private const int WmHotKey = 0x0312;
        private const uint ModControl = 0x0002;
        private const uint ModAlt = 0x0001;
        private const uint ModNoRepeat = 0x4000; // 按住时不重复触发

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true; // 取消关闭操作
            Hide();          // 隐藏窗口
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd)!.AddHook(WndProc);

            // 注册全局热键 Ctrl+Alt+A
            uint vk = (uint)KeyInterop.VirtualKeyFromKey(Key.A);
            if (!RegisterHotKey(hwnd, HotKeyId, ModControl | ModAlt | ModNoRepeat, vk))
            {
                System.Windows.MessageBox.Show("全局热键 Ctrl+Alt+A 注册失败（可能已被其他程序占用）");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HotKeyId);
            base.OnClosed(e);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WmHotKey && wParam.ToInt32() == HotKeyId)
            {
                System.Windows.MessageBox.Show("快捷键已生效");
                handled = true;
            }
            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}