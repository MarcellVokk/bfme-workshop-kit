using BfmeWorkshopKit.Data;
using BfmeWorkshopKit.Logic;
using Newtonsoft.Json;

namespace WorkshopEditor
{
    public partial class WorkshopEntryEditor : Form
    {
        public WorkshopEntryEditor()
        {
            InitializeComponent();
            txtGuid.Text = Guid.NewGuid().ToString();
            files.Add(new BfmeWorkshopFile() { Guid = Guid.NewGuid().ToString(), Name = "New file", Url = "", Md5 = "", Language = "ALL" });
            listBox1.Items.Add("New file");
            listBox1.SelectedIndex = 0;
            cbbGame.SelectedIndex = 0;
            cbbType.SelectedIndex = 0;
        }

        private List<BfmeWorkshopFile> files = new List<BfmeWorkshopFile>();

        public string GenerateJson()
        {
            BfmeWorkshopEntry entry = new BfmeWorkshopEntry
            {
                Guid = txtGuid.Text,
                Name = txtName.Text,
                Version = txtVersion.Text,
                Description = txtDescription.Text,
                ArtworkUrl = txtArtworkUrl.Text,
                Author = txtAuthor.Text,
                Owner = txtOwner.Text,
                Game = cbbGame.SelectedIndex,
                Type = cbbType.SelectedIndex,
                CreationTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                Files = files
            };

            txtJson.Text = JsonConvert.SerializeObject(entry, Formatting.Indented);
            return txtJson.Text;
        }

        public void LoadJson(string json)
        {
            BfmeWorkshopEntry entry = JsonConvert.DeserializeObject<BfmeWorkshopEntry>(json);

            txtGuid.Text = entry.Guid;
            txtName.Text = entry.Name;
            txtVersion.Text = entry.Version;
            txtDescription.Text = entry.Description;
            txtArtworkUrl.Text = entry.ArtworkUrl;
            txtAuthor.Text = entry.Author;
            txtOwner.Text = entry.Owner;
            cbbGame.SelectedIndex = entry.Game;
            cbbType.SelectedIndex = entry.Type;
            files = entry.Files;

            listBox1.Items.Clear();
            foreach (var file in files)
                listBox1.Items.Add(file.Name);

            if (files.Count > 0)
                listBox1.SelectedIndex = 0;
            else
            {
                listBox1.SelectedIndex = -1;
                gbxFileEdit.Visible = false;
            }

            GenerateJson();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            files.Add(new BfmeWorkshopFile() { Guid = Guid.NewGuid().ToString(), Name = "New file", Url = "", Md5 = "", Language = "ALL" });
            listBox1.Items.Add("New file");

            GenerateJson();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                files.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);

                GenerateJson();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                txtFileName.Text = files[listBox1.SelectedIndex].Name;
                txtFileUrl.Text = files[listBox1.SelectedIndex].Url;
                txtFileMd5.Text = files[listBox1.SelectedIndex].Md5;
                cbbFileLanguage.SelectedItem = files[listBox1.SelectedIndex].Language;
                gbxFileEdit.Visible = true;
            }
            else
            {
                gbxFileEdit.Visible = false;
            }
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            BfmeWorkshopFile file = files[listBox1.SelectedIndex];
            file.Name = txtFileName.Text;
            files[listBox1.SelectedIndex] = file;
            listBox1.Items[listBox1.SelectedIndex] = txtFileName.Text;

            GenerateJson();
        }

        private void txtFileUrl_TextChanged(object sender, EventArgs e)
        {
            BfmeWorkshopFile file = files[listBox1.SelectedIndex];
            file.Url = txtFileUrl.Text;
            files[listBox1.SelectedIndex] = file;

            GenerateJson();
        }

        private void txtFileMd5_TextChanged(object sender, EventArgs e)
        {
            BfmeWorkshopFile file = files[listBox1.SelectedIndex];
            file.Md5 = txtFileMd5.Text;
            files[listBox1.SelectedIndex] = file;

            GenerateJson();
        }

        private void cbbFileLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BfmeWorkshopFile file = files[listBox1.SelectedIndex];
            file.Language = cbbFileLanguage.SelectedItem?.ToString() ?? "ALL";
            files[listBox1.SelectedIndex] = file;

            GenerateJson();
        }

        private void txtName_TextChanged(object sender, EventArgs e) => GenerateJson();

        private void txtVersion_TextChanged(object sender, EventArgs e) => GenerateJson();

        private void txtDescription_TextChanged(object sender, EventArgs e) => GenerateJson();

        private void txtArtworkUrl_TextChanged(object sender, EventArgs e) => GenerateJson();

        private void txtAuthor_TextChanged(object sender, EventArgs e) => GenerateJson();

        private void cbbGame_SelectedIndexChanged(object sender, EventArgs e) => GenerateJson();

        private void cbbType_SelectedIndexChanged(object sender, EventArgs e) => GenerateJson();

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bfme Workshop Entry File|*.json";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadJson(File.ReadAllText(ofd.FileName));
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bfme Workshop Entry File|*.json";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, GenerateJson());
            }
        }

        private async void publishToWorkshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loginForm = new ArenaLoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await BfmeWorkshopUploadManager.Publish(loginForm.AuthInfo, JsonConvert.DeserializeObject<BfmeWorkshopEntry>(GenerateJson()));
                    MessageBox.Show("Entry published!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void removeFromWorkshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loginForm = new ArenaLoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await BfmeWorkshopUploadManager.Delete(loginForm.AuthInfo, txtGuid.Text);
                    MessageBox.Show("Entry deleted!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
