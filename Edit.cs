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
    public partial class Edit: Form
    {
        private Album editingAlbum;

        public Edit(Album album)
        {
            InitializeComponent();
            editingAlbum = album;

            box_artist_edit.Text = editingAlbum.artist;
            box_title_edit.Text = editingAlbum.title;
            box_catalogNo_edit.Text = editingAlbum.catalogNo;
            box_format_edit.Text = editingAlbum.format;
            box_primaryGenre_edit.Text = editingAlbum.primaryGenre;
            box_secondaryGenre_edit.Text = editingAlbum.secondaryGenre;
            box_primaryStyle_edit.Text = editingAlbum.primaryStyle;
            box_secondaryStyle_edit.Text = editingAlbum.secondaryStyle;
            box_release_edit.Text = editingAlbum.release.ToString();
        }


        private void btn_edit_edit_Click(object sender, EventArgs e)
        {
            editingAlbum.artist = box_artist_edit.Text;
            editingAlbum.title = box_title_edit.Text;
            editingAlbum.catalogNo = box_catalogNo_edit.Text;
            editingAlbum.format = box_format_edit.Text;
            editingAlbum.primaryGenre = box_primaryGenre_edit.Text;
            editingAlbum.secondaryGenre = box_secondaryGenre_edit.Text;
            editingAlbum.primaryStyle = box_primaryStyle_edit.Text;
            editingAlbum.secondaryStyle = box_secondaryStyle_edit.Text;

            // Try to parse the release year
            if (int.TryParse(box_release_edit.Text, out int releaseYear))
                editingAlbum.release = releaseYear;
            else
                editingAlbum.release = 0;

            // Signal success and close
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_edit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
