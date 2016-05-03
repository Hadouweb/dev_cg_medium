#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>

typedef struct      s_color
{
    char            cA;
    char            cB; 
}                   t_color;

typedef struct      s_app
{
    t_color         *colors;
    char            **map;
    char            **sim_map;
    int             *max;
    int             *sim_max;
    int             col;
    int             rot;
}                   t_app;

double ft_timer(clock_t begin)
{
    return (double)(clock() - begin) / 1000;
}

void    ft_print_map(char **map)
{
    for (int y = 0; y < 12; y++) 
    {
        for (int x = 0; x < 6; x++)
        {
            fprintf(stderr, "%c ", map[y][x]);
        }
        fprintf(stderr, "\n");
    }
    fprintf(stderr, "____________\n");
}

int     ft_get_first_free_pos(char **map, int x)
{
    for (int y = 0; y < 12; y++)
    {
        if (map[y][x] != '.')
            return (y - 1);
    }
    return (11);
}

void    ft_set_max(t_app *app)
{
    app->max = (int*)malloc(6 * sizeof(int));
    
    for (int x = 0; x < 6; x++)
    {
        app->max[x] = ft_get_first_free_pos(app->map, x);
    }
    //ft_print_max(app->max);
}

void    ft_print_max(int *max)
{
    for (int x = 0; x < 6; x++)
    {
        fprintf(stderr, "%d ", max[x]);
    }
    fprintf(stderr, "\n");
}


void    ft_set_piece_on_map(t_app *app, int y1, int x1, int y2, int x2, t_color c)
{
    app->sim_map[y1][x1] = c.cA;
    app->sim_map[y2][x2] = c.cB;

    //app->sim_max[x1]--;
    //app->sim_max[x2]--;
}

int     ft_is_authorize(t_app *app, int rot, int x)
{
    int *m = app->sim_max;
    if (rot == 0 && (x + 1 < 6) && m[x] >= 0 && m[x + 1] >= 0)
        return (0);
    else if (rot == 1 && m[x] - 1 >= 0)
        return (1);
    else if (rot == 2 && (x - 1 > 0) && m[x] >= 0 && m[x - 1] >= 0)
        return (2);
    else if (rot == 3 && m[x] - 1 >= 0 && m[x] >= 0)
        return (3);
    return (-1);
}

int    ft_push_on_map(t_app *app, t_color c, char start, int rot)
{
    int *m = app->sim_max;
    for (int x = start; x < 6; x++)
    {
        int authorize = ft_is_authorize(app, rot, x);
        if (authorize == 0)
        {
            //fprintf(stderr, "Authorize 0\n");
            ft_set_piece_on_map(app, m[x], x, m[x + 1], x + 1, c);
            return (x);
        }
        else if (authorize == 1)
        {
            //fprintf(stderr, "Authorize 1\n");
            ft_set_piece_on_map(app, m[x], x, m[x] - 1, x, c);
            return (x);
        }
        else if (authorize == 2)
        {
            ft_set_piece_on_map(app, m[x], x, m[x - 1], x - 1, c);
            return (x);
        }
        else if (authorize == 3)
        {
            //fprintf(stderr, "Authorize 3\n");
            ft_set_piece_on_map(app, m[x] - 1, x, m[x], x, c);
            return (x);
        }
    }
    return (-1);
}

void    ft_simulation(t_app *app)
{
    int count = 0;
    for (int x1 = 0; x1 < 6; x1++)
    {
        for (int r1 = 0; r1 < 4; r1++)
        {
            for (int x2 = 0; x2 < 6; x2++)
            {
                for (int r2 = 0; r2 < 4; r2++)
                {
                    for (int x3 = 0; x3 < 6; x3++)
                    {
                        for (int r3 = 0; r3 < 4; r3++)
                        {
                            ft_cpy_map(app->sim_map, app->map); 
                            ft_copy_max(app->sim_max, app->max);
                            ft_push_on_map(app, app->colors[0], x1, r1);
                            ft_push_on_map(app, app->colors[1], x2, r2);
                            ft_push_on_map(app, app->colors[2], x3, r3);
                            count++;
                            //ft_print_map(app->sim_map);
                            //ft_print_max(app->sim_max);
                        }
                    }
                }
            }
        }
    }
    fprintf(stderr, "Count : %d\n", count);
}

int     ft_contaminate(char **m, int y, int x, char c)
{
    if (y < 0 || y > 11 || x < 0 || x > 5)
        return 0;
    //fprintf(stderr, "y %d x %d c %c mc %c\n", y, x, c, m[y][x]);
    if (m[y][x] == c)
    {
        m[y][x] = '.';
        return (1
                + ft_contaminate(m, y - 1, x, c)
                + ft_contaminate(m, y + 1, x, c)
                + ft_contaminate(m, y, x - 1, c)
                + ft_contaminate(m, y, x + 1, c));
    }
    return 0;
}

int     ft_calcul_score(t_app *app)
{
    int     old_total = 0;

    for (int y = 0; y < 12; y++) 
    {
        for (int x = 0; x < 6; x++)
        {
            if (app->sim_map[y][x] != '.' && app->sim_map[y][x] != '0')
            {
                int total = ft_contaminate(app->sim_map, y, x, app->sim_map[y][x]);
                if (total > old_total)
                    old_total = total;
            }
        }
    }
    return old_total;
}

int     ft_calcul_score2(t_app *app, int x, int r, t_color c)
{
    int     old_total = 0;
    int     total = 0;
    int     ta = 0;
    int     tb = 0;
    char    **map = app->sim_map;
    char    **max = app->sim_max;

    int authorize = ft_is_authorize(app, r, x);
    if (authorize == 0)
    {
        ft_print_map(map);
        ta = ft_contaminate(map, max[x], x, c.cA);
        ft_print_map(map);
        tb = ft_contaminate(map, max[x + 1], x + 1, c.cB);
        fprintf(stderr, "r0 ta : %d tb : %d y1 : %d x1 %d y2 : %d x2 : %d cA : %c cB : %c\n", ta, tb, max[x], x, max[x+1], x+1, c.cA, c.cB);
        total = (ta > tb) ? ta : tb;
        if (total > old_total)
            old_total = total;
    }
    else if (authorize == 1)
    {
        ft_print_map(map);  
        ta = ft_contaminate(map, max[x], x, c.cA);
        ft_print_map(map);
        tb = ft_contaminate(map, max[x] - 1, x, c.cB);      
        fprintf(stderr, "r1 ta : %d tb : %d x1 : %d x2 : %d cA : %c cB : %c\n", ta, tb, x, x, c.cA, c.cB);
        total = (ta > tb) ? ta : tb;
        if (total > old_total)
            old_total = total;
    }
    else if (authorize == 2)
    {
        ft_print_map(map);  
        ta = ft_contaminate(map, max[x], x, c.cA);
        ft_print_map(map);
        tb = ft_contaminate(map, max[x - 1], x - 1, c.cB);    
        fprintf(stderr, "r2 ta : %d tb : %d x1 : %d x2 : %d cA : %c cB : %c\n", ta, tb, x, x-1, c.cA, c.cB);
        total = (ta > tb) ? ta : tb;
        if (total > old_total)
        {
            old_total = total;
        }
    }
    else if (authorize == 3)
    {
        ft_print_map(map);  
        ta = ft_contaminate(map, max[x] - 1, x, c.cA);
        ft_print_map(map);
        tb = ft_contaminate(map, max[x], x, c.cB);      
        fprintf(stderr, "r3 ta : %d tb : %d x1 : %d x2 : %d cA : %c cB : %c\n", ta, tb, x, x, c.cA, c.cB);
        total = (ta > tb) ? ta : tb;
        if (total > old_total)
            old_total = total;
    }
    return old_total;
}

void    ft_simulation_test(t_app *app)
{
    int old_score = 0;
    int count = 0;
    for (int x1 = 0; x1 < 6; x1++)
    {
        for (int r1 = 0; r1 < 4; r1++)
        {
            ft_cpy_map(app->sim_map, app->map);  
            ft_copy_max(app->sim_max, app->max);
            int current_x1 = ft_push_on_map(app, app->colors[0], x1, r1);
            //ft_print_map(app->sim_map);
            int score = ft_calcul_score2(app, current_x1, r1, app->colors[0]);
            fprintf(stderr, "Score : %d Col : %d Rot : %d\n", score , current_x1, r1);
            if (score > old_score)
            {
                old_score = score;
                app->rot = r1;
                app->col = current_x1;
            }
            //fprintf(stderr, "score : %d\n", score);
            //ft_print_max(app->sim_max);
            count++;
        }
    }
    fprintf(stderr, "Count : %d\n", count);
    fprintf(stderr, "Score : %d Col : %d Rot : %d\n", old_score , app->col, app->rot);
}


void    ft_copy_max(int *dst, int *src)
{
    for (int x = 0; x < 6; x++)
    {
        dst[x] = src[x];
    }
}

void    ft_cpy_map(char **dst, char **src)
{
    for (int y = 0; y < 12; y++) 
    {
        memcpy(dst[y], src[y], 6);
    }
}

void    ft_init(t_app *app)
{
    app->col = -1;
    app->rot = -1;
    app->sim_map = (char**)malloc(12 * sizeof(char*));
    for (int i = 0; i < 12; i++)
    {
        app->sim_map[i] = (char*)malloc(6);
    }
    app->sim_max = (int*)malloc(6 * sizeof(int));
    //fprintf(stderr, "%s\n", app->sim_map[0]);
    //fprintf(stderr, "%d\n", sizeof(app->map));
    /*for (int i = 0; i < 1000000; i++)
    {
        ft_cpy_map(app->sim_map, app->map);
    }
    app->sim_map[11][0] = '@';
    app->map[11][1] = '?';
    //ft_cpy_map(app->sim_map, app->map);
    ft_print_map(app->map);
    ft_print_map(app->sim_map);*/
    ft_set_max(app);
    ft_simulation_test(app);
}

int     main()
{
    t_app   app;

    while (1) 
    {
        clock_t begin = clock();

        bzero(&app, sizeof(t_app));
        app.colors = malloc(8 * sizeof(t_color));
        for (int i = 0; i < 8; i++) 
        {
            int ca;
            int cb;
            scanf("%d %d", &ca, &cb);
            app.colors[i].cA = ca + '0';
            app.colors[i].cB = cb + '0';
        }

        app.map = (char**)malloc(12 * sizeof(char*));
        for (int i = 0; i < 12; i++) 
        {
            char row[7];
            scanf("%s", row);
            app.map[i] = strdup(row);
        }

        for (int i = 0; i < 12; i++) 
        {
            char row[7]; // One line of the map ('.' = empty, '0' = skull block, '1' to '5' = colored block)
            scanf("%s", row);
        }

        ft_init(&app);

        int i = 0;
        double time_spent;
            int a;
            int b;
        /*while (i < 50000000) 
        {
            a = 4;
            b = 5;
            a |= b >> 4;
            if ((time_spent = ft_timer(begin)) > 98)
                break;
            i++;
        }*/

        fprintf(stderr, "\n%f %d\n", ft_timer(begin), i);
        printf("%d %d\n", app.col, app.rot);
    }

    return 0;
}