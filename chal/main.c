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

char    **ft_copy_map(char **src)
{
    char **dst = (char**)malloc(12 * sizeof(char*));
    for (int y = 0; y < 12; y++)
    {
        dst[y] = strdup(src[y]);
    }
    return (dst);
}

void    ft_print_max(int *max)
{
    for (int x = 0; x < 6; x++)
    {
        fprintf(stderr, "%d ", max[x]);
    }
    fprintf(stderr, "\n");
}

char    **ft_copy_max(int *src)
{
    int *dst = (int*)malloc(6 * sizeof(int));
    for (int x = 0; x < 6; x++)
    {
        dst[x] = src[x];
    }
    return (dst);
}

void    ft_set_piece_on_map(t_app *app, int y1, int x1, int y2, int x2, t_color c)
{
    app->sim_map[y1][x1] = c.cA;
    app->sim_map[y2][x2] = c.cB;

    app->sim_max[x1]--;
    app->sim_max[x2]--;
}

int     ft_is_authorize(t_app *app, int rot, int x)
{
    int *m = app->sim_max;
    if (rot == 0 && (x + 1 < 6) && m[x] >= 0 && m[x + 1] >= 0)
        return (0);
    else if (rot == 1 && m[x] - 1 >= 0)
        return (1);
    else if (rot == 2 && (x - 1 >= 0) && m[x] >= 0 && m[x - 1] >= 0)
        return (2);
    else if (rot == 3 && m[x] >= 1)
        return (3);
    return (-1);
}

void    ft_push_on_map(t_app *app, t_color c, char start, int rot)
{
    int *m = app->sim_max;
    for (int x = start; x < 6; x++)
    {
        if (ft_is_authorize(app, rot, x) == 0)
        {
            fprintf(stderr, "Authorize 0\n");
            ft_set_piece_on_map(app, m[x], x, m[x + 1], x + 1, c);
            return;
        }
        else if (ft_is_authorize(app, rot, x) == 1)
        {
            fprintf(stderr, "Authorize 1\n");
            ft_set_piece_on_map(app, m[x], x, m[x] - 1, x, c);
            return;
        }
        else if (ft_is_authorize(app, rot, x) == 2)
        {
            fprintf(stderr, "Authorize 2\n");
            ft_set_piece_on_map(app, m[x], x, m[x - 1], x - 1, c);
            return;
        }
        else if (ft_is_authorize(app, rot, x) == 3)
        {
            fprintf(stderr, "Authorize 3\n");
            ft_set_piece_on_map(app, m[x] - 1, x, m[x], x, c);
            return;
        }
    }
}

void    ft_simulation_pos(t_app *app)
{
    int count = 0;
    for (int x1 = 0; x1 < 6; x1++)
    {
        app->sim_map = ft_copy_map(app->map);
        app->sim_max = ft_copy_max(app->max);
        ft_push_on_map(app, app->colors[0], x1, 3);
        count++;
        ft_print_map(app->sim_map);
        ft_print_max(app->sim_max);
    }
    //fprintf(stderr, "Count : %d\n", count);
}


/*void    ft_simulation(t_app *app)
{
    int count = 0;
    for (int x1 = 0; x1 < 5; x1++)
    {
        for (int x2 = 0; x2 < 5; x2++)
        {
            for (int x3 = 0; x3 < 5; x3++)
            {
                for (int x4 = 0; x4 < 5; x4++)
                {
                    for (int x5 = 0; x5 < 5; x5++)
                    {
                        for (int x6 = 0; x6 < 5; x6++)
                        {
                            app->sim_map = ft_copy_map(app->map);
                            app->sim_max = strdup(app->max);
                            ft_push_on_map(app, app->colors[0], x1);
                            ft_push_on_map(app, app->colors[1], x2);
                            ft_push_on_map(app, app->colors[3], x3);
                            ft_push_on_map(app, app->colors[4], x4);
                            ft_push_on_map(app, app->colors[5], x5);
                            ft_push_on_map(app, app->colors[6], x6);
                            count++;
                        }
                    }
                }
                //ft_print_map(app->sim_map);
            }
        }
    }
    fprintf(stderr, "Count : %d\n", count);
}*/

void    ft_init(t_app *app)
{
    //ft_print_map(app);
    ft_set_max(app);
    ft_simulation_pos(app);
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
        printf("0 0\n");
    }

    return 0;
}