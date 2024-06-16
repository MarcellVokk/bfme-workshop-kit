using BfmeWorkshopKit.Data;
using BfmeWorkshopKit.Logic;
using System.Data;

namespace WorkshopEditor
{
    public partial class WorkshopBrowser : Form
    {
        public WorkshopBrowser()
        {
            InitializeComponent();
            cbbGame.SelectedIndex = 0;
            cbbSortMode.SelectedIndex = 0;
            Search();
        }

        List<BfmeWorkshopEntry> entries = new List<BfmeWorkshopEntry>();

        private async void Search(string? ownerUuid = null)
        {
            if (ownerUuid == null)
                entries = await BfmeWorkshopQueryManager.Search(txtKeyword.Text, cbbGame.SelectedIndex - 1, cbbSortMode.SelectedIndex, (int)nudPage.Value);
            else
                entries = await BfmeWorkshopQueryManager.Search(ownerUuid);

            lbxResults.Items.Clear();
            foreach (var result in entries)
                lbxResults.Items.Add(result.Name);
        }

        private void btnSearch_Click(object sender, EventArgs e) => Search();

        private void btnSearchByOwner_Click(object sender, EventArgs e) => Search(txtOwnerUuid.Text);

        private async void lbxResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxResults.SelectedIndex == -1)
                return;

            var results = await BfmeWorkshopQueryManager.Get(entries[lbxResults.SelectedIndex].Guid);

            txtName.Text = results.entry.Name;
            txtVersion.Text = results.entry.Version;
            txtDescription.Text = results.entry.Description;
            txtArtworkUrl.Text = results.entry.ArtworkUrl;
            txtAuthor.Text = results.entry.Author;
            txtOwner.Text = results.entry.Owner;
            txtGame.Text = results.entry.Game.ToString();
            txtType.Text = results.entry.Type.ToString();
            txtFiles.Text = string.Join("\r\n", results.entry.Files.Select(x => $"{x.Name} ({x.Url})"));

            txtDownloads.Text = results.metadata.Downloads.ToString();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (lbxResults.SelectedIndex == -1)
                return;

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await BfmeWorkshopDownloadManager.Download(entries[lbxResults.SelectedIndex].Guid, fbd.SelectedPath);
                    MessageBox.Show("Download complete!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void workshopEntryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var wee = new WorkshopEntryEditor();
            wee.Show();
        }
    }
}
