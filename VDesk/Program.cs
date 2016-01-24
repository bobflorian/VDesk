using System;
using libVDesk;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Microsoft.Win32;
using System.Threading;

namespace VDesk {
  static class Program {
    static readonly int executable = 0;
    static readonly int desktopIndex = 1;
    static readonly int command = 2;
    static readonly int arguments = 3;

    
    static void Main(string[] args) {
      if (args.Length == 0) return; //need at least 1 arg.

      switch (args[0]) {
        case "-install": install(); return;  //add registry entries for context menu
        case "-uninstall": uninstall(); return;  //remove registry entries for context menu
        case "-manage": manage(); return; //launch management gui
        case "-daemon": daemon(); return; //run as daemon
        default: run(); return;
      }
    }

    static void install() {
      RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*", true);

      RegistryKey shell = key.CreateSubKey("shell");

      RegistryKey vdesk = shell.CreateSubKey("VDesk");
      vdesk.SetValue("", "Open in new Virtual Desktop");

      RegistryKey command = vdesk.CreateSubKey("command");
      command.SetValue("", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\" %*");
      
      command.Close();
      vdesk.Close();
      shell.Close();
      key.Close();

      return;
    }

    static void uninstall() {
      RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell", true);

      if (key.OpenSubKey("VDesk") != null) //check key exists
        key.DeleteSubKeyTree("VDesk");
        
      key.Close();

      return;
    }

    static void manage() {
      return;
    }

    static void daemon() {
      int lastCount = Desktop.Count;
      int lastActive = 0;
      int n = 0;

      // Let other programs take priority
      Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

      while (true) {
        // check desktop count for changes
        if (lastCount != Desktop.Count) {
          lastCount = Desktop.Count;
          desktopCountChanged(lastCount);
        }

        // check if desktop has changed
        for (int i = 0; i < Desktop.Count; i++) {
          if (Desktop.FromIndex(i).IsVisible && lastActive != i) {
            lastActive = i;
            desktopChanged(i);
          }
        }

        //GC about every second
        n %= 60;
        if (++n == 1)
          GC.Collect();

        //run ~60 times per second.
        Thread.Yield();
        Thread.Sleep(16);
      }
    }

    static void desktopCountChanged(int count) {
      // todo: ready for gui integration
      return;
    }

    static void desktopChanged(int current) {
      // todo: ready for gui integration
      return;
    }
    
    static void run() {
      String[] clArgs = parseCommandLine(Environment.CommandLine);
      Process proc = new Process();

      int index = int.Parse(clArgs[desktopIndex]) - 1; //set desktop index
      proc.StartInfo.FileName = clArgs[command]; //set executable name
      proc.StartInfo.Arguments = clArgs[arguments]; //set arg list

      //If we're opening a program on desktop 10, ensure there are 10 desktops.
      for (int i = Desktop.Count - 1; i < index; i++)
        Desktop.Create();

      //get the desktop, or create a new one
      Desktop desk = index < 0 ? Desktop.Create() : Desktop.FromIndex(index);

      if (!clArgs[executable].Equals("")) { //if we're launching a program:
        try {
          //swtich to the desktop and try starting the program
          desk.MakeVisible();
          proc.Start();

        }
        catch (Win32Exception) {
          //Error launching program.
          Console.Error.WriteLine("Failed to start program.\nCheck executable path.");

        }
        finally {
          //If we created a desktop just for this program, remove it after the program has finished executing.
          if (index < 0) {
            proc.WaitForExit();
            desk.Remove();
          }

        }
      }

      return;
    }

    static String[] parseCommandLine(String cla) {
      String[] ret = new string[4];

      GroupCollection groups = Regex.Match(cla, @"(\""[^\""]+\""|[\w-:_\/\.\\]+) +(?:(-?\d+) ?)?(\""[^\""]+\""|[\w-:_\/.\\]+)? ?(.*)").Groups;
      
      for (int i = 1; i < 4; i++)
        ret[i-1] = groups[i].Value; //set return values
      
      if (ret[desktopIndex].Equals(""))
        ret[desktopIndex] = "0";  //if index is empty, set index = 0
      
      return ret;
    }
  }
}
