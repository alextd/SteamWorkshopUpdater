using System;
using System.IO;
using SteamWorkshopUploader;

namespace SteamWorkshopUpdater
{
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            if ( args.Length < 1 || args.Length > 2 || args[0] == "-h" )
            {
                Console.WriteLine( @"Usage:
    SteamWorkshopUpdater.exe [path_to_mod] [changenote]

Options:
    path_to_mod     Fully qualified path to the mod's base directory.       (required)
    changenote      Either a fully qualified path to a file, in which case  (optional)
                    the changenote will be read from that file, or a string
                    changenote. If not set, will default to the current 
                    timestamp.");

                return;
            }

            try
            {
                var mod = new Mod( args[0]);

                string changenote;
                if ( args.Length >= 2 && args[1] != null)
                {
                    // if file, read from file - otherwise set changenote
                    if ( File.Exists( args[1] ) )
                        changenote = File.ReadAllText( args[1] );
                    else
                        changenote = args[1];
                }
                else
                {
                    // fall back on default changenote
                    changenote = "[Auto-generated text]: Update on " + DateTime.Now + ".";
                }

                // Console.WriteLine( mod + "\nChangenote: " + changenote );

                Uploader.Init();
                if ( Uploader.Upload( mod, changenote ) )
                {
                    Console.WriteLine( "Upload done: https://steamcommunity.com/sharedfiles/filedetails/changelog/" + mod.PublishedFileId );
                }
                else
                {
                    Console.WriteLine( "Upload failed :(" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
            finally
            {
                Uploader.Shutdown();
            }

        }
    }
}
