wcl -DOS_WINDOWS *.c argParse/*.c comp/fs/*.c -0 -fe=imgEdit.exe > out1.txt
wcl -DOS_WINDOWS -Dtesting argParse/*.c comp/fs/*.c test/*.c str.c setup.c -0 -fe=test/test.exe > out2.txt
@echo off
cd test
test.exe > test.txt
type test.txt
