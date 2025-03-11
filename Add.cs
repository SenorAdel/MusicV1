using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicV1
{
    public partial class Add: Form
    {
        public Album NewAlbum { get; set; }

        public Add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(box_artist_add.Text) ||
                string.IsNullOrWhiteSpace(box_title_add.Text) ||
                string.IsNullOrWhiteSpace(box_catalogNo_add.Text) ||
                string.IsNullOrWhiteSpace(box_format_add.Text) ||
                string.IsNullOrWhiteSpace(box_primaryGenre_add.Text) ||
                string.IsNullOrWhiteSpace(box_secondaryGenre_add.Text) ||
                string.IsNullOrWhiteSpace(box_primaryStyle_add.Text) ||
                string.IsNullOrWhiteSpace(box_secondaryStyle_add.Text) ||
                string.IsNullOrWhiteSpace(box_release_add.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            // Create the album using the input values.
            Album album = new Album
            {
                artist = box_artist_add.Text,
                title = box_title_add.Text,
                catalogNo = box_catalogNo_add.Text,
                format = box_format_add.Text,
                primaryGenre = box_primaryGenre_add.Text,
                secondaryGenre = box_secondaryGenre_add.Text,
                primaryStyle = box_primaryStyle_add.Text,
                secondaryStyle = box_secondaryStyle_add.Text
            };

            // Try parsing the release year; if it fails, default to 0.
            if (int.TryParse(box_release_add.Text, out int releaseYear))
                album.release = releaseYear;
            else
                album.release = 0;

            // Set the new album and signal a successful result.
            NewAlbum = album;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_add_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_reset_add_Click(object sender, EventArgs e)
        {
            box_artist_add.Clear();
            box_title_add.Clear();
            box_catalogNo_add.Clear();
            box_format_add.Clear();
            box_primaryGenre_add.Clear();
            box_secondaryGenre_add.Clear();
            box_primaryStyle_add.Clear();
            box_secondaryStyle_add.Clear();
            box_release_add.Clear();
        }
    }
}
