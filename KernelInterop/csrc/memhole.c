#include "memhole.h"

// initializes a memhole_t struct
// note: this function does not connect to the memhole device (use connect_memhole() after this function)
//
// returns a pointer to a new memhole_t or 0 upon malloc error
memhole_t* memhole_create(){
    memhole_t* ret_val = (memhole_t*) malloc(sizeof(memhole_t));
    if(!ret_val) return 0;
    ret_val->fd = 0;
    ret_val->buf_size = 0;
    memset(&ret_val->op_sem, 0, sizeof(sem_t));
    return ret_val;
}

// frees a memhole_t
// note: this function does not disconnect from the memhole device (use disconnect_memhole() before this function)
//
// returns an error code or 0 upon success
int memhole_delete(memhole_t* memhole){
    if(memhole == NULL) return -EINVDEV;
    free(memhole);
    return 0;
}

// connects a memhole_t to the memhole kernel module
// note: only 1 connection is allowed at a time
// 
// returns an error code or 0 upon success
int memhole_connect(memhole_t* memhole){

    #ifdef MEMHOLEW_ALLOW_WRITE
    int mode = O_RDWR;
    #endif
    #ifndef MEMHOLEW_ALLOW_WRITE
    int mode = O_RDONLY;
    #endif
    #ifdef MEMHOLEW_CLOSE_ON_EXEC
    mode = mode | O_CLOEXEC;
    #endif

    if(memhole == NULL) return -EINVDEV;
    if(access(MEMHOLE_PATH, F_OK)) return -EMEMHNF;

    int ret = open(MEMHOLE_PATH, mode);

    if(ret < 0){
        return -EMEMBSY;
    }
    memhole->fd = ret;
    memhole->buf_size = -1;
    sem_init(&memhole->op_sem, 0, 1);
    return 0;
}

// disconnects a memhole_t from the memhole kernel module
// 
// returns an error code or 0 upon success
int memhole_disconnect(memhole_t* memhole){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd == 0) return -EINVDEV;
    return close(memhole->fd);
}

// attach memhole to pid's memory
// 
// returns an error code or 0 upon success
long memhole_attach_to_pid(memhole_t* memhole, int pid){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;
    if(kill(pid, 0)) return -EINVPID;

    long ret = lseek64(memhole->fd, pid, LSMSPID);
    if(ret == 0){
        return -EINVPID;
    }
    else if(ret == -1){
        return -EUSPOPN;
    }
    return 0;
}

// set the secondary PID for direct other process -> other process memory operations
// set PID to 0 to exit other process -> other process mode
//
// returns an error code or 0 upon success
long memhole_attach_secondary_pid(memhole_t* memhole, int pid){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;
    if(pid != 0 && kill(pid, 0)) return -EINVPID;

    long ret = lseek64(memhole->fd, pid, LSMSOPI);
    if(ret == 0 && pid != 0){
        return -EINVPID;
    }
    else if(ret == -1){
        return -EUSPOPN;
    }
    return 0;
}

// attach memhole to 2 processes' memory for other process -> other process operations
// set pid2 to 0 to exit other process -> other process mode
//
// returns an error code or 0 upon success
long memhole_attach_to_pids(memhole_t* memhole, int pid1, int pid2){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;
    if(kill(pid1, 0)) return -EINVPID;

    long ret = lseek64(memhole->fd, pid1, LSMSPID);
    if(ret == 0){
        return -EINVPID;
    }
    if(pid2 != 0 && kill(pid2, 0)) return -EINVPID;
    ret = lseek64(memhole->fd, pid2, LSMSOPI);
    if(ret == 0 && pid2 != 0){
        return -EINVPID;
    }
    return 0;
}

// set the memory position to start reading/writing from
// mode should be a memhole_mode_t enum; if you are not immediately calling read_memory or write_memory, use SKMFAST
// NOTE: the pos is NOT automatically incremented after a read/write
// 
// returns an error code or the memory address seeked to
// mode SKMSFNB will return -EMEMBSY if the semaphore could not be grabbed
long memhole_set_mem_pos(memhole_t* memhole, void* pos, memhole_mode_t mode){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;
    if(mode == SKMSAFE){
        sem_wait(&memhole->op_sem);
    }
    else if(mode == SKMSFNB){
        if(sem_trywait(&memhole->op_sem)){
            return -EMEMBSY;
        }
    }
    else if(mode != SKMFAST){
        printf("invalid mode used for set_memory_position()\n");
        return -EINVDEV;
    }
    return lseek64(memhole->fd, (long) pos, LSMSPOS);
}

// get the memory position currently used for reading/writing from
// 
// returns an error code or the memory address requested
long memhole_get_mem_pos(memhole_t* memhole){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;

    return lseek64(memhole->fd, 0, LSMGPOS);
}

// set the memhole buffer size
// this is done automatically if a read/write call is made that would exceed the buffer size
//
// returns an error code or 0 upon success
long memhole_set_buffer_size(memhole_t* memhole, long len){
    if(len > memhole->buf_size){
        if(lseek64(memhole->fd, len, LSMSBUF)){
            return -EKMALOC;
        }
    }
    return 0;
}

// read memory starting at the pos set with set_memory_position()
// buf should be the address to read memory to
//    In other process -> other process mode, this will be the memory address written to in the secondary process
//    In normal mode, this should be to an appropriately sized buffer
// mode should be a memhole_mode_t enum, likely equal to the one used in the last set_memory_position() call
// NOTE: the pos is NOT automatically incremented after a read/write
// 
// returns an error code or number of bytes read
long memhole_read(memhole_t* memhole, char* buf, long len, memhole_mode_t mode){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;
    if(!((mode == SKMSAFE) | (mode == SKMFAST) | (mode == SKMSFNB))){
        printf("invalid mode used in read_memory()\n");
        return -EINVDEV;
    }
    if(memhole_set_buffer_size(memhole, len)){
        return -EKMALOC;
    }
    long ret_val = read(memhole->fd, buf, len);
    if(mode & (SKMSAFE | SKMSFNB)){
        sem_post(&memhole->op_sem);
    }
    return ret_val;
}

// write memory starting at the pos set with set_memory_position()
// buf should be the address of the data to be written
//    In other process -> other process mode, this will be the starting memory address read from in the secondary process
//    In normal mode, this should point to a buffer with the desired data to be written
// mode should be a memhole_mode_t enum, likely equal to the one used in the last set_memory_position() call
// NOTE: the pos is NOT automatically incremented after a read/write
// 
// returns an error code or number of bytes written
long memhole_write(memhole_t* memhole, char* buf, long len, memhole_mode_t mode){
    if(memhole == NULL) return -EINVDEV;
    if(memhole->fd <= 0) return -EINVDEV;
    if(!((mode == SKMSAFE) | (mode == SKMFAST) | (mode == SKMSFNB))){
        printf("invalid mode used in write_memory()\n");
        return -EINVDEV;
    }
    if(memhole_set_buffer_size(memhole, len)){
        return -EKMALOC;
    }
    long ret_val = write(memhole->fd, buf, len);
    printf("RET VAL: %ld\n", ret_val);
    if(mode & (SKMSAFE | SKMSFNB)){
        sem_post(&memhole->op_sem);
    }
    return ret_val;
}