> [!NOTE]
> This progam is currently being rewritten in C, you can see the old C# version in the [deprecated branch](https://github.com/RLH-2110/ImgEdit/tree/deprecated).

> [!WARNING]
> This readme is not complete


When the rewrite is done, it will be able to create floppy disk images with FAT 12.

My goal is to make this compatible with Linux, Windows NT, MS-DOS and Haiku

Here is what you need to do to compile the source for the different operating systems.
Linux and Haiku require Make and GCC, which are usually preinstalled.  
On Windows you have to install them yourself or compile my code manually with your own compiler.  
  
Some operating systems like DOS or early windows versions have scripts that requrie the <a href="https://github.com/open-watcom/open-watcom-v2">Open Watcom</a> compiler

Make for Windows: https://gnuwin32.sourceforge.net/packages/make.htm  
GCC for Windows: https://www.mingw-w64.org/  

If you compile with the .BAT files, you also have to go to src/comp/fs/ and run one of those .BAT files first, to compile the library used by this project!
you can find instuctions in the readme there, but the batfile naming convention there is the same as for this project.  
  
the used library (compFS) can be found here: https://github.com/RLH-2110/compFS and here: https://gitlab.com/OwOUwU621/compfs  
  
to see what operating systems where tested what versions where tested, and when, see testing.csv

<table>
    <thead>
        <tr>
            <th>Compiling</th>
            <th>Requirements</th>
            <th>Command</th>
            <th>Testing Command</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Linux</td>
            <td>GCC, Make</td>
            <td>make</td>
            <td>make test</td>
        </tr>
        <tr>
            <td>Windows<br /></td>
            <td>
                GCC, Make<br /><b>OR</b><br />
                <a href="https://github.com/open-watcom/open-watcom-v2">Open Watcom</a><br />
            </td>
            <td>make<br /><b>OR</b><br />COMPNT</td>
            <td>make test<br /><b>OR</b><br />TESTNT</td>
        </tr>
        <tr>
            <td>DOS</td>
            <td><a href="https://github.com/open-watcom/open-watcom-v2">Open Watcom</a></td>
            <td>COMPDOS</td>
            <td>TESTDOS</td>
        </tr>
        <tr>
            <td>Haiku</td>
            <td>GCC, Make</td>
            <td>make</td>
            <td>make test</td>
        </tr>
    </tbody>
</table>

# Potential Build Errors

## COMPNT / NTTEST

### The name specified is not recognized
If you get this error:
```
The name specified is not recognized as an
internal or external command, operable program or batch file.  
```
  
It means wcl is not found in your current cmd.  
you can use an cmd from open watcom, that should have a path to wlc, you can find it on windows like this:  
`Start -> Programs -> Open Watcom 2.0 C - C++ -> Build Enviroment`



