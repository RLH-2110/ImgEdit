wcl386 -DOS_WINDOWS *.c argParse/*.c comp/fs/*.c -fe=imgEdit.exe > out1.txt
wcl386 -DOS_WINDOWS -Dtesting argParse/*.c comp/fs/*.c test/*.c str.c setup.c -fe=test/test.exe > out2.txt
@echo off
cd test
test.exe > test.txt
type test.txt
