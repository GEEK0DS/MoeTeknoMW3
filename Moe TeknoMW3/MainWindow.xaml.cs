using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Security.Cryptography;
using Material_Design_TeknoMW3;
using static Fullrank.doRank;


namespace Moe_TeknoMW3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Proflie profile;
        private Random rng = new Random();
        private int prestige = 0;
        private int rank = 0;
        private int perkspointer = 0;
        private int classpointer = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("iw5mp.exe"))
            {
                MessageBox.Show("未找到iw5mp.exe！请检查你的游戏是否完整！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (textBox.Text == "127.0.0.1:27016"| textBox.Text == "")
            {
                StartProcess("iw5mp.exe", "");
            }
            else
            {
                if (!textBox.Text.Contains(":"))
                {
                    textBox.Text = (textBox.Text + ":27016");
                }
                StartProcess("iw5mp.exe", "+server " + textBox.Text);
            }
        }
        public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        private void CheckIp(string domain)
        {
            try
            {
                int port = 1;

                if (domain.Contains(":"))
                {
                    port = Convert.ToInt32(domain.Split(new char[] { ':' }, 2)[1]);
                    if (port > 65536 || port < 1)
                    {
                        port = 27016;
                    }

                    UriHostNameType result = Uri.CheckHostName(domain.Split(new char[] { ':' }, 2)[0]);

                    if (result == UriHostNameType.Unknown || result == UriHostNameType.Basic)
                    {
                    }
                    else
                    {
                    }
                }
                else
                {
                    UriHostNameType result2 = Uri.CheckHostName(domain);

                    if (result2 == UriHostNameType.Unknown || result2 == UriHostNameType.Basic)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private async void StartProcess(string proc, string arguements)
        {
            Version.Background = new SolidColorBrush(Color.FromRgb(211, 125, 0));
            label.Background = new SolidColorBrush(Color.FromRgb(255, 125, 0));
            button.Background = new SolidColorBrush(Color.FromRgb(255, 125, 0));
            button.IsEnabled = false;
            button.Content = "已运行";
            button2.IsEnabled = true;
            try
            {
                RunProc runproc = new RunProc { ExecutableName = proc, Commandargs = arguements };
                await runproc.Tick(proc == "iw5mp.exe" ? "teknomw3.dll" : "teknomw3_sp.dll");
            }
            catch (Exception)
            {
                MessageBox.Show("游戏启动失败！请检查游戏文件是否可用，并且已正确安装联机破解！");
            }
            finally
            {
                Version.Background = new SolidColorBrush(Color.FromRgb(0, 125, 211));
                label.Background = new SolidColorBrush(Color.FromRgb(0, 125, 255));
                button.Background = new SolidColorBrush(Color.FromRgb(0, 125, 255));
                button.IsEnabled = true;
                button.Content = "启动!";
                button2.IsEnabled = false;
            }
        }

        public void UpdateProfile()
        {
            LoadProfile();
        }
        private void LoadProfile()
        {
            try
            {
                if (File.Exists("teknogods.ini"))
                {
                    bool AutoChanged = false;
                    bool NeedChange = false;
                    IniParser ini = new IniParser("teknogods.ini");
                    profile = new Proflie();

                    profile.Name = ini.GetSetting("Settings", "Name");
                    if ((string.IsNullOrEmpty(profile.Name) || string.IsNullOrWhiteSpace(profile.Name)) || (profile.Name.Length > 15 || profile.Name.Length < 3))
                    {
                        profile.Name = "CHN_TeknoPlayer";
                        NeedChange = true;
                    }

                    profile.ID = Convert.ToInt64(ini.GetSetting("Settings", "ID"));
                    if (string.IsNullOrWhiteSpace(profile.ID.ToString()) || profile.ID.ToString() == "0")
                    {
                        var low = (long)rng.Next(0x1000, 0xFFFF);
                        var high = (long)rng.Next(0x1000, 0xFFFF);
                        profile.ID = Convert.ToInt64(low + string.Empty + high);
                        AutoChanged = true;
                    }

                    profile.FOV = Convert.ToInt32(ini.GetSetting("Settings", "FOV"));
                    if (profile.FOV > 90 || profile.FOV < 65)
                    {
                        profile.FOV = 75;
                        AutoChanged = true;
                    }

                    profile.Clantag = ini.GetSetting("Settings", "Clantag");
                    if (string.IsNullOrWhiteSpace(profile.Clantag) || profile.Clantag.Length > 4)
                    {
                        profile.Clantag = "SXXM";
                        AutoChanged = true;
                    }

                    profile.Title = ini.GetSetting("Settings", "Title");
                    if (profile.Title.Length > 25)
                    {
                        profile.Title = "";
                        AutoChanged = true;
                    }

                    string showconsole = ini.GetSetting("Settings", "ShowConsole");
                    if (string.IsNullOrEmpty(showconsole))
                    {
                        profile.ShowConsole = false;
                        AutoChanged = true;
                        goto LABEL_001;
                    }
                    profile.ShowConsole = Convert.ToBoolean(showconsole);

                    LABEL_001:

                    if (NeedChange)
                    {
                        MessageBox.Show("你的玩家名称似乎有问题，请重新调整你的玩家信息。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Settings st = new Settings(profile);
                        st.ShowDialog();
                        UpdateProfile();
                    }
                    else if (AutoChanged)
                    {
                        ini.AddSetting("Settings", "FOV", profile.FOV.ToString());
                        ini.AddSetting("Settings", "Clantag", profile.Clantag);
                        ini.AddSetting("Settings", "ShowConsole", profile.ShowConsole.ToString().ToLower());
                        ini.SaveSettings();
                    }
                    else
                    {
                    }
                }
                else
                {
                    MessageBox.Show("未检测到配置文件，你需要先设置你的玩家信息。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CreateNewProfile();

                    Settings st = new Settings(profile);
                    st.ShowDialog();
                    UpdateProfile();
                }
            }
            catch (Exception)
            {
                File.Delete("teknogods.ini");
                MessageBox.Show("读取配置文件时遇到问题，你的配置文件可能已损坏，请重新调整你的玩家信息。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                CreateNewProfile();

                Settings st = new Settings(profile);
                st.ShowDialog();
                UpdateProfile();
            }
        }
        private void CreateNewProfile()
        {
            var low = (long)rng.Next(0x1000, 0xFFFF);
            var high = (long)rng.Next(0x1000, 0xFFFF);

            profile.Name = "CHN_TeknoPlayer";
            profile.FOV = 75;
            profile.Clantag = "SXXM";
            profile.Title = "^5SuperTeknoMW3";
            profile.ShowConsole = false;

            try
            {
                File.WriteAllLines("teknogods.ini", new string[]
                {
                    "[Settings]",
                    "Name=" + profile.Name,
                    "ID=" + profile.ID,
                    "FOV=" + profile.FOV,
                    "Clantag=" + profile.Clantag,
                    "Title=" + profile.Title,
                    "ShowConsole=" + profile.ShowConsole.ToString().ToLower(),
                }
                );
            }
            catch (Exception)
            {
                MessageBox.Show("创建配置文件失败！请检查磁盘是否有写保护，以及是否有写入权限！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        private void OMLBD(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MLBD1(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Started(object sender, RoutedEventArgs e)
        {
            if (File.Exists("teknomw3.dll"))
            {
                string ver = GetMD5HashFromFile("teknomw3.dll");
                if (ver == "dfb6412a2dc47d44d00839763fe52d28")
                {
                    label1.Content = "TeknoMW3 Version:2739 Beta";
                }
                if (ver == "03bd2e345e6e8a7a0b494382f1a2ab1c")
                {
                    label1.Content = "TeknoMW3 Version:2737";
                }
                if (ver == "3d736173b49edf6a0b4a58d76cb2fa32")
                {
                    label1.Content = "TeknoMW3 Version:2738";
                }
            }

            try
            {
                LoadProfile();
                label.Content = "     Welcome:" + profile.Name;
            }
            catch (Exception)
            {
                MessageBox.Show("无法载入配置文件！请检查是否有足够的权限读取和修改此文件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            label.Content = label.Content += profile.Name;
            if (File.Exists("teknomw3.dll"))
            {
                string TeknoVer = GetMD5HashFromFile("teknomw3.dll");
                if (TeknoVer == "dfb6412a2dc47d44d00839763fe52d28")
                {
                    MessageBox.Show("如果你更改了某些设置，你的ID可能会改变，这将重置你游戏内的数据");
                }
            }
            Settings settings = new Settings(profile);
            settings.ShowDialog();
            UpdateProfile();
            label.Content = "     Welcome:" + profile.Name;
        }

        private void MLBD(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        public void Fuckup()
        {
            rank = 0x1cdba54;
            prestige = rank + 0x210;
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank, 0x19dfd4, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", prestige, 11, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x2c, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x58, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x20, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x80, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x74, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x7c, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xb0, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xd8, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 20, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 40, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x30, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x44, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x24, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x38, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x34, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x40, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 8, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x18, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x10, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x48, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x4c, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x5c, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 100, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 160, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xac, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xb0, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xa8, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xa4, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x98, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x9c, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x94, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x90, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 140, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 60, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 80, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x54, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 180, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x60, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x68, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x6c, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 12, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x1c, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 120, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x84, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x88, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x100, 0x2bd91, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 220, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xe4, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0xbc, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", rank + 0x10c, 0x2f44, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 2, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 1, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 3, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 4, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 5, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 6, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 7, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 8, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 9, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 10, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 11, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 12, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 13, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", perkspointer + 14, 0x7070707, 4);
            ReadWritingMemory.WriteInteger("iw5mp.exe", classpointer, 10, 4);
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Fuckup();
        }

        private void Exit(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Close();
            }
        }
    }
}
