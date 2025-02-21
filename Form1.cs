using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicV1
{
    //view = view 
    //model = data
    // control = funtions (example: sort, search...)
    public partial class Form1 : Form
    {
        Model model = new Model();
        public Form1()
        {
            InitializeComponent();

            //auto-loads "myalbums.csv" on startup
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

                    // Store the imported albums in _albums (not a local variable)
                    model.medialist = DiscogsImport.ReadFile(filePath);

                    // Display them in the list box
                    listBox1.Items.Clear();
                    foreach (Album album in model.medialist)
                    {
                        //listBox1.Items.Add($"{album.artist} - {album.title} ({album.release})");
                        listBox1.Items.Add($"  `artist:{album.artist}`---`title:{album.title}`---`catalogNo:{album.catalogNo}###From Album###`---`format:{album.format}`---`primaryGenre:{album.primaryGenre}`---`primaryStyle:{album.primaryStyle}`---`secondaryGenre:{album.secondaryGenre}`---`secondaryStyle:{album.secondaryStyle}`");
 
                    }
                }
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
    }
}
