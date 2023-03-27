#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

int main(int argc, char** argv){
    char* buf = (char*) calloc(1, 16L * 1024L * 1024L * 1024L);
    for(long i = 0; i < 16L * 1024L * 1024L * 1024L; i++){
        buf[i] = (char) 0b0101010101010101;
    }
    printf("%p\n", buf);
    sleep(10000000);
    return 0; 
}