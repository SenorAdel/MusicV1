using System;
using System.Collections.Generic;
using System.IO;

namespace MusicV1
{
    internal class DiscogsImport : DataImport
    {
        public static List<MediaObject> ReadFile(string filePath)
        {
            var albumList = new List<MediaObject>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] fields = line.Split(',');
                    if (fields.Length < 9)
                        continue;

                    // 0: format
                    // 1: primaryGenre
                    // 2: primaryStyle
                    // 3: secondaryGenres
                    // 4: secondaryStyles
                    // 5: catalogNo
                    // 6: artist
                    // 7: title
                    // 8: year

                    var album = new Album
                    {
                        // From MediaObject
                        artist = fields[6],
                        title = fields[7],
                        catalogNo = fields[5],

                        // From Album
                        format = fields[0],
                        primaryGenre = fields[1],
                        primaryStyle = fields[2],
                        secondaryGenre = fields[3],
                        secondaryStyle = fields[4]
                    };

                    // Convert the release year (index 8)
                    if (int.TryParse(fields[8], out int year))
                        album.release = year;
                    else
                        album.release = 0;

                    albumList.Add(album);
                }
            }

            return albumList;
        }
    }
}
