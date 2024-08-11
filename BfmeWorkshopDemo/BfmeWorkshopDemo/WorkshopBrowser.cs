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
            cbbType.SelectedIndex = 0;
            cbbSortMode.SelectedIndex = 0;
            UpdateQuery();
        }

        List<string> games = ["BFME1", "BFME2", "RotWK"];
        List<BfmeWorkshopEntry> entries = new List<BfmeWorkshopEntry>();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            UpdateQuery();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            Sync();
        }

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
            txtVersion.Text = string.Join(", ", results.metadata.Versions);
        }

        private async void UpdateQuery()
        {
            entries = await BfmeWorkshopQueryManager.Query(txtKeyword.Text, cbbGame.SelectedIndex - 1, new[] { -2, -3, -1, 0, 1, 2, 3 }[cbbType.SelectedIndex], cbbSortMode.SelectedIndex, (int)nudPage.Value);

            lbxResults.Items.Clear();
            foreach (var result in entries)
                lbxResults.Items.Add($"[{games[result.Game]}] {result.Name}  |  {result.Author}");
        }

        private async void Sync()
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
    }
}
