
# VDesk

Provides options for launching programs on virtual desktops. (Windows 10 only)

Windows10Interop code taken from http://stackoverflow.com/a/32417530

#####Download:

See the [releases page](https://github.com/eksime/VDesk/releases/).

#####Usage:

`vdesk [n] [command [args]]`

`n` specifies the index of the VD to launch the program on, and is optional.

If the command is omitted vdesk will launch `n` virtual desktops.

Generally, prepending a shortcut's 'target' field with `vdesk` will have it launch in its own virtual desktop. Command line arguments should be preserved.

VDesk can provide an "Open in new virtual desktop" context menu item, this can be done by running `vdesk -install`. To remove the context menu entries, run `vdesk -uninstall`.

#####Examples:
To launch notepad on a new desktop:

`vdesk notepad`

To launch notepad on desktop 3 and open `C:\some file.txt`:

`vdesk 3 notepad "C:\some file.txt"`

To launch a new VirtualBox vm fullscreen on its own virtual desktop:

`vdesk "C:\Path to Vbox\VirtualBox.exe" --comment "VM" --startvm "vm-id-no" --fullscreen`

## Copyright notice

Copyright (C) 2016

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
