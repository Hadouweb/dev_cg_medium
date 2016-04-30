#include <stdlib.h>
#include <stdio.h>
#include <string.h>

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
 
typedef struct point{
    int x;
    int y;
} Point;
int main()
{
    int width; // the number of cells on the X axis
    scanf("%d", &width); fgetc(stdin);
    fprintf(stderr, "width %d\n", width);
    int height; // the number of cells on the Y axis
    scanf("%d", &height); fgetc(stdin);
    fprintf(stderr, "height %d\n", height);

    int size = width * height;
    fprintf(stderr, "Size : %d\n", size);

    Point *elem;
    elem = (Point*)calloc(size, sizeof(Point));
    int t = 0;
    int s = 0;
    
    for (int i = 0; i < height; i++) {
        char line[31]; // width characters, each either 0 or .
        fgets(line,31,stdin); // width characters, each either 0 or
        
        for(int x = 0; x < width; x++) {
            if((char)line[x] == '0') {
                elem[t].x = x;
                elem[t].y = i;
                s++;
                t++;
            }
        }
        
    }
    
    //elem = (Point*)realloc(elem, s);
    
    for (int i = 0; i < s; i++) {
        fprintf(stderr, "%d, %d\n", elem[i].x, elem[i].y);
    }
    
    for (int i = 0; i < s; i++) {
        printf("%d %d ", elem[i].x, elem[i].y);
        
        int checkX = 0;
        int checkY = 0;
        int findX = 0;
        int w = 1;
        int max = 0;
        
        for (int j = i+1; j < s; j++) {
            if( elem[i].y == elem[j].y && checkX == 0) {
                printf("%d %d ", elem[j].x, elem[j].y);
                checkX = 1;
            }
        }
        
        if(checkX == 0)
            printf("-1 -1 ");
        
        for (int j = i+1; j < s; j++) {
            if( elem[i].x == elem[j].x && checkY == 0) {
                printf("%d %d ", elem[j].x, elem[j].y);
                checkY = 1;
            }
        }
        
        if(checkY == 0)
            printf("-1 -1");
        
        printf("\n");
    }
    

}