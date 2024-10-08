> [!NOTE]
> This progamm is currently being rewritten in C, you can see the old C# version in the [deprecated branch](https://github.com/RLH-2110/ImgEdit/tree/deprecated).

> [!WARNING]
> This readme is not compleate


When the rewrite is done, it will be able to create floppy disk images with FAT 12.

My goal is to make this compatible with Linux, Windows NT+, MS-DOS and Haiku

Here is what you need to do to compile the source for the different operating systems.
Linux and Haiku, Make and GCC are usually preinstalled, on Windows you have to install them yourself or compile my code manually with your own compiler.  

Make for Windows: https://gnuwin32.sourceforge.net/packages/make.htm  
GCC for Windows: https://www.mingw-w64.org/  

<table><thead><tr><th>Compiling</th><th>Requirements</th><th>Command</th><th>Versions Tested</th></tr></thead><tbody><tr><td>Linux</td><td>GCC, Make</td><td>make</td><td>Ubuntu 22.04.4 LTS</td></tr><tr><td>Windows<br></td><td>GCC, Make<br><b>OR</b><br> <a href="https://github.com/open-watcom/open-watcom-v2">Open Watcom</a><br></td><td>make<br><b>OR</b><br>COMPNT</td><td>Windows 10 22h2,<br> Windows NT 4.0 Workstation</td></tr><tr><td>DOS</td><td><a href="https://github.com/open-watcom/open-watcom-v2">Open Watcom</a></td><td>COMPDOS</td><td>6.22</td></tr><tr><td>Haiku</td><td>GCC, Make</td><td>make</td><td>R1/Beta4(hrev56578+59)</td></tr></tbody></table>

