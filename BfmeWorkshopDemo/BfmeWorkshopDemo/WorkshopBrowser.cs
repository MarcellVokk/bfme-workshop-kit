using BfmeFoundationProject.WorkshopKit.Data;
using BfmeFoundationProject.WorkshopKit.Logic;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;

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

        List<string> games = ["BFME1", "BFME2", "RotWK"];
        List<BfmeWorkshopEntry> entries = new List<BfmeWorkshopEntry>();

        private async void Search(string? ownerUuid = null)
        {
            if (ownerUuid == null)
                entries = await BfmeWorkshopQueryManager.Query(txtKeyword.Text, cbbGame.SelectedIndex - 1, cbbSortMode.SelectedIndex, (int)nudPage.Value);
            else
                entries = await BfmeWorkshopQueryManager.Query(ownerUuid);

            lbxResults.Items.Clear();
            foreach (var result in entries)
                lbxResults.Items.Add($"[{games[result.Game]}] {result.Name}  |  {result.Author}");
        }

        private void btnSearch_Click(object sender, EventArgs e) => Search();

        private void btnSearchByOwner_Click(object sender, EventArgs e) => Search(txtOwnerUuid.Text);

        private async void lbxResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxResults.SelectedIndex == -1)
                return;

            var results = await BfmeWorkshopQueryManager.Get(entries[lbxResults.SelectedIndex].Guid);

            txtGuid.Text = results.entry.Guid;
            txtName.Text = results.entry.Name;
            txtVersion.Text = results.entry.Version;
            txtDescription.Text = results.entry.Description;
            txtArtworkUrl.Text = results.entry.ArtworkUrl;
            txtAuthor.Text = results.entry.Author;
            txtOwner.Text = results.entry.Owner;
            txtGame.Text = results.entry.Game.ToString();
            txtType.Text = results.entry.Type.ToString();
            txtFiles.Text = string.Join("\r\n", results.entry.Files.Select(x => $"{x.Name} ({x.Url})"));
            txtDepepdencies.Text = string.Join("\r\n", results.entry.Dependencies);

            txtDownloads.Text = results.metadata.Downloads.ToString();
            txtVersions.Text = string.Join(", ", results.metadata.Versions);
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (lbxResults.SelectedIndex == -1)
                return;

            try
            {
                await BfmeWorkshopLibraryManager.AddToLibrary(entries[lbxResults.SelectedIndex].Guid);
                MessageBox.Show("Added to library!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnSync_Click(object sender, EventArgs e)
        {
            if (lbxResults.SelectedIndex == -1)
                return;

            pgbSync.Visible = true;

            try
            {
                await BfmeWorkshopSyncManager.Sync((await BfmeWorkshopQueryManager.Get(entries[lbxResults.SelectedIndex].Guid)).entry, (progress) => pgbSync.Value = progress, (title, progress) => lblStatus.Text = title != "" ? $"{title} {progress}%" : "");
                MessageBox.Show("Sync complete!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            pgbSync.Visible = false;
        }

        private void workshopEntryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
