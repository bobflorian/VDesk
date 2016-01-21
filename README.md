# VDesk
Launch programs on new virtual desktops.

Windows10Interop code taken from http://stackoverflow.com/a/32417530

#####Usage:

`vdesk program [args]`

#####Examples:
To launch notepad on a new desktop:

`vdesk notepad`

To launch a new VirtualBox vm fullscreen on it's own virtual desktop:

`vdesk "C:\Path to Vbox\VirtualBox.exe" --comment "VM" --startvm "vm-id-no" --fullscreen`

For most shortcuts it is possible to append `vdesk` to the start of the target field in order to start the program on a new virtual desktop, for this to work `vdesk` must be in PATH.