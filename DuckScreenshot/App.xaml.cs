using System.Configuration;
using System.Data;
using System.Windows;

namespace DuckScreenshot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon? notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 创建托盘图标
            notifyIcon = new NotifyIcon()
            {
                Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                Text = "鸭鸭截图",
                Visible = true,
            };

            // 双击托盘图标显示主窗口
            notifyIcon.DoubleClick += (s, args) => ShowMainWindow();

            // 创建右键菜单
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("打开", null, (s, args) => ShowMainWindow());
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("退出", null, (s, args) => ExitApplication());
            notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        private void ShowMainWindow()
        {
            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }

        private void ExitApplication()
        {
            notifyIcon!.Visible = false;
            notifyIcon?.Dispose();
            Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon?.Dispose();
            base.OnExit(e);
        }
    }

}
