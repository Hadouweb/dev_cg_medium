#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>

typedef struct      s_color
{
    char            cA;
    char            cB; 
}                   t_color;

typedef struct      s_sim
{
    int             y1;
    int             x1;
    int             y2;
    int             x2;
    int             rot;
}                   t_sim;

typedef struct      s_app
{
    t_color         *colors;
    char            **map;
    char            **sim_map;
    int             *max;
    int             *sim_max;
    int             col;
    int             rot;
    int             smash;
}                   t_app;

double ft_timer(clock_t begin)
{
    return (double)(clock() - begin) / 1000;
}

void    ft_print_max(int *max)
{
    for (int x = 0; x < 6; x++)
    {
        fprintf(stderr, "%d ", max[x]);
    }
    fprintf(stderr, "\n");
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
    int its_time = 0;
    app->smash = 0;
    for (int x = 0; x < 6; x++)
    {
        int m = ft_get_first_free_pos(app->map, x);
        if (m < 10)
            its_time++;
        app->max[x] = m;
    }
    if (its_time > 4)
        app->smash = 1;
    //ft_print_max(app->max);
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

void    ft_set_piece_on_map(t_app *app, int y1, int x1, int y2, int x2, t_color c)
{
    app->sim_map[y1][x1] = c.cA;
    app->sim_map[y2][x2] = c.cB;

    //app->sim_max[x1]--;
    //app->sim_max[x2]--;
}

void    ft_push_on_map(t_app *app, t_color c, char start, int rot, t_sim *s)
{
    int *m = app->sim_max;
    for (int x = start; x < 6; x++)
    {
        int authorize = ft_is_authorize(app, rot, x);
        if (authorize == 0)
        {
            //fprintf(stderr, "Authorize 0\n");
            ft_set_piece_on_map(app, m[x], x, m[x + 1], x + 1, c);
            s->y1 = m[x];
            s->x1 = x;
            s->y2 = m[x + 1];
            s->x2 = x + 1;
            s->rot = 0;
            return;
        }
        else if (authorize == 1)
        {
            //fprintf(stderr, "Authorize 1\n");
            ft_set_piece_on_map(app, m[x], x, m[x] - 1, x, c);
            s->y1 = m[x];
            s->x1 = x;
            s->y2 = m[x - 1];
            s->x2 = x;
            s->rot = 1;
            return;
        }
        else if (authorize == 2)
        {
            ft_set_piece_on_map(app, m[x], x, m[x - 1], x - 1, c);
            s->y1 = m[x];
            s->x1 = x;
            s->y2 = m[x - 1];
            s->x2 = x - 1;
            s->rot = 2;
            return;
        }
        else if (authorize == 3)
        {
            //fprintf(stderr, "Authorize 3\n");
            ft_set_piece_on_map(app, m[x] - 1, x, m[x], x, c);
            s->y1 = m[x] - 1;
            s->x1 = x;
            s->y2 = m[x];
            s->x2 = x;
            s->rot = 3;
            return;
        }
    }
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

void    ft_simulation_test(t_app *app)
{
    int old_score = 0;
    int count = 0;
    t_sim s;

    for (int x1 = 0; x1 < 6; x1++)
    {
        for (int r1 = 0; r1 < 4; r1++)
        {
            bzero(&s, sizeof(t_sim));

            ft_cpy_map(app->sim_map, app->map);  
            ft_copy_max(app->sim_max, app->max);

            ft_push_on_map(app, app->colors[0], x1, r1, &s);

            //ft_print_map(app->sim_map);

            int score_a = ft_contaminate(app->sim_map, s.y1, s.x1, app->colors[0].cA);
            int score_b = ft_contaminate(app->sim_map, s.y2, s.x2, app->colors[0].cB);

            //fprintf(stderr, "score_a : %d score_b : %d\n", score_a, score_b);
            //fprintf(stderr, "s.y1 : %d s.x1 : %d s.y2 : %d s.x2 : %d\n", s.y1, s.x1, s.y2, s.x2);
            
            int score = (score_a > score_b) ? score_a : score_b;

            if (score > old_score)
            {
                if ((app->smash == 0 && score < 4) || app->smash == 1)
                {
                    old_score = score;
                    app->rot = r1;
                    app->col = s.x1;
                }
            }
            //fprintf(stderr, "score : %d\n", score);
            //ft_print_max(app->sim_max);
            count++;
        }
    }
    //fprintf(stderr, "Count : %d\n", count);
    //fprintf(stderr, "Score : %d Col : %d Rot : %d\n", old_score , app->col, app->rot);
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
    bzero(&app, sizeof(t_app));

    while (1) 
    {
        clock_t begin = clock();

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