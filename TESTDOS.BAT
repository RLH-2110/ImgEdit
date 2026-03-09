wcl -DOS_DOS src/comp/fs/libcmpfs.lib src/*.c src/argParse/*.c config/*.c -0 -fe=imgEdit.exe > out.txt
wcl -DOS_DOS -Dtesting src/comp/fs/libcmpfs.lib src/argParse/*.c test/*.c src/str.c src/setup.c config/*.c -0 -fe=test/test.exe > out2.txt
@echo off
cd test
test.exe > test.txt
type test.txt
