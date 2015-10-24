using System;
using System.Windows.Forms;
using System.IO;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Threading.Tasks;


namespace Material_Design_TeknoMW3
{
    public partial class Settings : MaterialForm
    {
        private Proflie profile;
        private Random rng = new Random();
        public Settings(Proflie profile)
        {
            InitializeComponent();
            this.profile = profile;
            var materialSkinManagers = MaterialSkinManager.Instance;
            materialSkinManagers.AddFormToManage(this);
            materialSkinManagers.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManagers.ColorScheme = new ColorScheme(Primary.Orange800, Primary.Orange900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }
        private void SaveProfile()
        {
            TextWriter writer = new StreamWriter("teknogods.ini");

            try
            {
                writer.WriteLine("[Settings]");
                writer.WriteLine("Name=" + profile.Name);
                writer.WriteLine("ID=" + profile.ID);
                writer.WriteLine("FOV=" + profile.FOV);
                writer.WriteLine("Clantag=" + profile.Clantag);
                writer.WriteLine("Title=" + profile.Title);
                writer.WriteLine("ShowConsole=" + profile.ShowConsole.ToString().ToLower());
            }
            catch (Exception)
            {
                MessageBox.Show("创建配置文件失败！请检查磁盘是否有写保护，以及是否有写入权限！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            finally
            {
                writer.Close();
            }
        }
        private void Settings_Load(object sender, EventArgs e)
        {
            materialSingleLineTextField1.Text = profile.Name;
            materialSingleLineTextField4.Text = profile.FOV.ToString();
            materialSingleLineTextField2.Text = profile.Title;
            materialSingleLineTextField3.Text = profile.Clantag;
            materialSingleLineTextField5.Text = profile.ID.ToString();
            materialCheckBox1.Checked = profile.ShowConsole;
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            IniParser ini = new IniParser("teknogods.ini");
            if (string.IsNullOrEmpty(materialSingleLineTextField1.Text))
            {
                MessageBox.Show("请输入游戏昵称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(materialSingleLineTextField4.Text))
            {
                MessageBox.Show("请输入视野大小！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(materialSingleLineTextField1.Text))
            {
                MessageBox.Show("游戏昵称不符合要求！请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            profile.ID = Convert.ToInt64(ini.GetSetting("Settings", "ID"));
            if (string.IsNullOrWhiteSpace(profile.ID.ToString()) || profile.ID.ToString() == "0")
            {
                var low = (long)rng.Next(0x1000, 0xFFFF);
                var high = (long)rng.Next(0x1000, 0xFFFF);
                profile.ID = Convert.ToInt64(low + string.Empty + high);
            }

            profile.Name = materialSingleLineTextField1.Text;
            profile.FOV = Convert.ToInt32(materialSingleLineTextField4.Text);
            profile.Clantag = materialSingleLineTextField3.Text;
            profile.Title = materialSingleLineTextField2.Text;
            profile.ShowConsole = materialCheckBox1.Checked;

            if (profile.FOV > 90 || profile.FOV < 65)
            {
                MessageBox.Show("视野大小不符合要求！请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (profile.Clantag.Length > 6)
            {
                MessageBox.Show("战队不符合要求！请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (profile.Title.Length > 25)
            {
                MessageBox.Show("标签文本不符合要求！请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            SaveProfile();

            Close();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            Form[] frms = this.MdiChildren;

            DialogResult dialogResult = MessageBox.Show("是否确认不保存关闭？", "退出提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                    Close();
            }
            else
            {
                return;
            }
        }
        private long GetID()
        {
            var low = (long)rng.Next(0x1000, 0xFFFF);
            var high = (long)rng.Next(0x1000, 0xFFFF);
            long ID = Convert.ToInt64(low + string.Empty + high);
            return ID;
        }
        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            Form[] frms = this.MdiChildren;

            DialogResult dialogResult = MessageBox.Show("随机生成GUID将会清空你的游戏数据，是否继续", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                string GUID = GetID().ToString();
                materialSingleLineTextField5.Text = GUID;
            }
            else
            {
                return;
            }
        }

        private void materialSingleLineTextField5_Click(object sender, EventArgs e)
        {

        }

    }

}
