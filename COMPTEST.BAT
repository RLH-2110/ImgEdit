wcl -DOS_DOS *.c argParse/*.c comp/fs/*.c -0 -fe=imgEdit.exe > out1.txt
wcl -DOS_DOS -Dtesting argParse/*.c comp/fs/*.c test/*.c str.c setup.c -0 -fe=test/test.exe > out2.txt
test/test.exe
