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
        List<BfmeWorkshopEntryPreview> entries = new List<BfmeWorkshopEntryPreview>();

        private async void Search(string? ownerUuid = null)
        {
            if (ownerUuid == null)
                entries = await BfmeWorkshopQueryManager.Query(keyword: txtKeyword.Text, game: cbbGame.SelectedIndex - 1, type: -1, sortMode: cbbSortMode.SelectedIndex, page: (int)nudPage.Value);
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

            var query = await BfmeWorkshopQueryManager.Get(entries[lbxResults.SelectedIndex].Guid);

            txtGuid.Text = query.entry.Guid;
            txtName.Text = query.entry.Name;
            txtVersion.Text = query.entry.Version;
            txtDescription.Text = query.entry.Description;
            txtArtworkUrl.Text = query.entry.ArtworkUrl;
            txtAuthor.Text = query.entry.Author;
            txtOwner.Text = query.entry.Owner;
            txtGame.Text = query.entry.Game.ToString();
            txtType.Text = query.entry.Type.ToString();

            txtDownloads.Text = query.metadata.Downloads.ToString();
            txtVersions.Text = string.Join(", ", query.metadata.Versions);
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (lbxResults.SelectedIndex == -1)
                return;

            try
            {
                BfmeWorkshopLibraryManager.AddOrUpdate(await BfmeWorkshopDownloadManager.Download(entries[lbxResults.SelectedIndex].Guid));
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
                await BfmeWorkshopSyncManager.Sync(await BfmeWorkshopDownloadManager.Download(entries[lbxResults.SelectedIndex].Guid),
                OnProgressUpdate: (progress) =>
                {
                    pgbSync.Value = progress;
                });
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
