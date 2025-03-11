using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace MusicV1
{
    //view = view 
    //model = data
    //control = funtions (example: sort, search...)
    public partial class Form1 : Form
    {
        Model model = new Model();
        public Form1()
        {
            InitializeComponent();
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;

            //auto-loads "myalbums.csv" on startup
            LoadDefaultFile();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void btn_load_Click(object sender, EventArgs e) // "Import" button
        {
            
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV files|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;

                    // Read albums into model
                    model.medialist = DiscogsImport.ReadFile(filePath);

                    // Display them in the list box as objects
                    listBox1.Items.Clear();
                    foreach (Album album in model.medialist)
                    {
                        //listBox1.Items.Add($"{album.artist} - {album.title} ({album.release})");
                        listBox1.Items.Add(album);
                    }
                }
            }
        }
        private void LoadDefaultFile()
        {
            // Build the full path to "file.csv" (assumed to be in the same folder as the executable)
            string filePath = Path.Combine(Application.StartupPath, "collection.csv");

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Default file not found: " + filePath);
                return;
            }

            try
            {
                model.medialist = DiscogsImport.ReadFile(filePath);

                // Clear the listBox1 and add each album object
                listBox1.Items.Clear();
                foreach (Album album in model.medialist)
                {
                    listBox1.Items.Add(album);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading default file: " + ex.Message);
            }
        }

        private void btn_save_Click(object sender, EventArgs e) // "Save" button
        {
            
            if (model.medialist == null || model.medialist.Count == 0)
            {
                MessageBox.Show("No albums to save!");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files|*.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string savePath = sfd.FileName;

                    using (StreamWriter sw = new StreamWriter(savePath))
                    {
                        // (Optional) write a header line:
                        // sw.WriteLine("format,primaryGenre,primaryStyle,secondaryGenre,secondaryStyle,catalogNo,artist,title,year");

                        foreach (Album album in model.medialist)
                        {
                            // Match the same field order you used when reading:
                            // 0: format
                            // 1: primaryGenre
                            // 2: primaryStyle
                            // 3: secondaryGenre
                            // 4: secondaryStyle
                            // 5: catalogNo
                            // 6: artist
                            // 7: title
                            // 8: year

                            sw.WriteLine($"{album.format}," +
                                         $"{album.primaryGenre}," +
                                         $"{album.primaryStyle}," +
                                         $"{album.secondaryGenre}," +
                                         $"{album.secondaryStyle}," +
                                         $"{album.catalogNo}," +
                                         $"{album.artist}," +
                                         $"{album.title}," +
                                         $"{album.release}");
                        }
                    }

                    MessageBox.Show("Albums saved successfully!");
                }
            }
        }
        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the second list box
            listBox2.Items.Clear();

            // Check if something is selected
            if (listBox1.SelectedItem != null)
            {
                // Cast the selected item to Album
                Album selectedAlbum = (Album)listBox1.SelectedItem;

                // Display additional info in listBox2
                listBox2.Items.Add("Format: " + selectedAlbum.format);
                listBox2.Items.Add("PrimaryGenre: " + selectedAlbum.primaryGenre);
                listBox2.Items.Add("PrimaryStyle: " + selectedAlbum.primaryStyle);
                listBox2.Items.Add("SecondaryGenre: " + selectedAlbum.secondaryGenre);
                listBox2.Items.Add("SecondaryStyle: " + selectedAlbum.secondaryStyle);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            using (Add addForm = new Add())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    Album newAlbum = addForm.NewAlbum;

                    model.medialist.Add(newAlbum);
                    // redisplay the new album in the ListBox
                    listBox1.Items.Add(newAlbum);
                }
            }
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Please select an album to remove.");
                return;
            }

            model.medialist.RemoveAt(index);
            listBox1.Items.RemoveAt(index);
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This further not anymore supported");
            //if (listBox1.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Please select an item to edit.");
            //    return;
            //}
            //Album selectedAlbum = (Album)listBox1.SelectedItem;
            //using (Edit editForm = new Edit(selectedAlbum))
            //{
            //    if (editForm.ShowDialog() == DialogResult.OK)
            //    {
            //        // Refresh the item in the ListBox to update its display.
            //        int selectedIndex = listBox1.SelectedIndex;
            //        listBox1.Items[selectedIndex] = selectedAlbum;
            //    }
            //}
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {        
            listBox1.Items.Clear();
            foreach (var album in model.medialist)
            {
                listBox1.Items.Add(album);
            }
        }


        private void btn_search_Click(object sender, EventArgs e)
        {
            string searchTerm = search_box.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchTerm))
                return;

            // search for artist, title, or catalog number.
            var results = model.medialist.Where(a =>
                a.artist.ToLower().Contains(searchTerm) ||
                a.title.ToLower().Contains(searchTerm) ||
                a.catalogNo.ToLower().Contains(searchTerm)).ToList();

            listBox1.Items.Clear();
            foreach (var album in results)
            {
                listBox1.Items.Add(album);
            }
        }

        private void btn_status_Click(object sender, EventArgs e)
        {
            MusicV1.Chart chartForm = new MusicV1.Chart();

            // Pass the Album objects from model.medialist
            chartForm.ShowStats(model.medialist.OfType<Album>().ToList());

            // Show the chart form
            chartForm.ShowDialog();
        }
    }
}