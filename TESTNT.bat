wcl386 -DOS_WINDOWS src/comp/fs/libcmpfs.lib src/*.c src/argParse/*.c config/*.c -fe=imgEdit.exe > out.txt

wcl386 -DOS_WINDOWS -Dtesting src/comp/fs/libcmpfs.lib src/argParse/*.c test/*.c src/str.c src/setup.c config/*.c -fe=test/test.exe > out2.txt
@echo off
cd test
test.exe > test.txt
type test.txt
