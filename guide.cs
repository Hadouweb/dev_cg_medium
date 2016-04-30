#include <stdlib.h>
#include <stdio.h>
#include <string.h>

typedef struct Sommet {
    int a, b;
} Sommet;

int linkAlreadyBlocked(int SI, int b, Sommet * nodesBlock, int count) {
    int i;

    for(i = 0; i < count; i++){
        if(SI == nodesBlock[i].a && b == nodesBlock[i].b) {
            return 5;
        }
        
        if(SI == nodesBlock[i].b && b == nodesBlock[i].a) {
            return 5;
        }
    }
    
    return -1;
}

int exist(int SI, Sommet *nodes, int *portes, int L, int E) {
    int i, y;
    fprintf(stderr, "SI %d\n", SI);
    
    for(i = 0; i < L; i++) {
        
        for(y = 0; y < E; y++) {
            //fprintf(stderr, "%d %d ", nodes[i].a, nodes[i].b);
            //fprintf(stderr, "portes %d\n", portes[y]);
            
            if(SI == nodes[i].a && portes[y] == nodes[i].b) {
                return portes[y];
            }
            
            if(SI == nodes[i].b && portes[y] == nodes[i].a) {
                return portes[y];
            }
        }
        
    }
    
    return -1;
}

int find(int x, Sommet *nodes, int N) {
    int i;
    
    for(i = 0; i < N; i++) {
        fprintf(stderr, "x %d ", x);
        fprintf(stderr, "a %d\n", nodes[i].a);
        
        if(x == nodes[i].a)
            return nodes[i].b;
            
        if(x == nodes[i].b)
            return nodes[i].a;
    }
}

int main()
{
    int N; // the total number of nodes in the level, including the gateways
    int L; // the number of links
    int E; // the number of exit gateways
    scanf("%d%d%d", &N, &L, &E);
    
    Sommet *nodes;
    nodes = (Sommet*)calloc(L, sizeof(Sommet));
    
    Sommet *nodesBlock;
    nodesBlock = (Sommet*)calloc(L, sizeof(Sommet));
    
    int posB = 0;
    
    for (int i = 0; i < L; i++) {
        int N1; // N1 and N2 defines a link between these nodes
        int N2;
        scanf("%d%d", &N1, &N2);
        nodes[i].a = N1;
        nodes[i].b = N2;
    }
    
    for (int i = 0; i < L; i++) {
        //fprintf(stderr, "%d %d\n", nodes[i].a, nodes[i].b);
    }
    
    int portes[E];
    
    for (int i = 0; i < E; i++) {
        int EI; // the index of a gateway node
        scanf("%d", &EI);
        portes[i] = EI;
        //fprintf(stderr, "Porte : %d\n", EI);
    }

    // game loop
    while (1) {
        int SI; // The index of the node on which the Skynet agent is positioned this turn
        scanf("%d", &SI);
        fprintf(stderr, "Skynet : %d\n", SI);
        int abl;
        
        int nextPorte = exist(SI, nodes, portes, L, E);
        
        if(nextPorte != -1) {
            fprintf(stderr, "Exist\n");
            abl = linkAlreadyBlocked(SI, nextPorte, nodesBlock, posB);
            
            if(abl != -1) {
                fprintf(stderr, "Sortie 1\n");
                printf("%d %d\n", SI, abl);
            } else {
                fprintf(stderr, "Sortie 2\n");
                printf("%d %d\n", SI, nextPorte);
            }
            
            nodesBlock[posB].a = SI;
            nodesBlock[posB].b = nextPorte;
            posB++;

        } else {
            
            fprintf(stderr, "No Exist\n");
            int bp = find(SI, nodes, N);
            
            abl = linkAlreadyBlocked(SI, bp, nodesBlock, posB);
            
            if(abl != -1) {
                fprintf(stderr, "Sortie 3\n");
                printf("%d %d\n", SI, abl);
            } else {
                fprintf(stderr, "Sortie 4\n");
                printf("%d %d\n", SI, bp);
            }
            
            nodesBlock[posB].a = SI;
            nodesBlock[posB].b = find(SI, nodes, N);
            posB++;
        }
    
    }
}