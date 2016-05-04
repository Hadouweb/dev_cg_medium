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

typedef struct      s_fs
{
    int             b;
    int             cp;
    int             tmp_cp;
    int             cb;
    int             gb;
    int             c;
    int             current_score;
}                   t_fs;

typedef struct      s_score
{
    char            **score_map;
    char            **clean_map;
    int             *score_max;
    int             *clean_max;
    int             score;
    char            nb_color[5];
}                   t_score;

typedef struct      s_list
{
    void            *content;
    size_t          content_size;
    struct s_list   *next;
}                   t_list;

typedef struct      s_node
{
    int             col;
    int             rot;
    int             deep;
    int             score;
}                   t_node;

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
    t_score         sc;
    t_fs            calcul_score;
    int             final_score;
    t_list          *lst;
    t_node          n;
    int             limit;
    int             old_deep;
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
    app->limit = 0;
    for (int x = 0; x < 6; x++)
    {
        int m = ft_get_first_free_pos(app->map, x);
        if (m < 8)
            its_time++;
        app->max[x] = m;
    }
    if (its_time > 2)
        app->limit = 1;
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

void    ft_set_clean_map(char **dst)
{
    for (int y = 0; y < 12; y++) 
    {
        memset(dst[y], '.', 6);
    }
}

void    ft_clean_map(t_app *app)
{
    ft_set_clean_map(app->sc.clean_map);
    for (int x = 0; x < 6; x++)
        app->sc.clean_max[x] = 11;
    for (int x = 0; x < 6; x++)
    {
        for (int y = 11; y >= 0; y--)
        {
            if (app->sc.score_map[y][x] != '.')
            {
                app->sc.clean_map[app->sc.clean_max[x]][x] = app->sc.score_map[y][x];
                app->sc.clean_max[x]--;
            }
        }
    }
}

void    ft_set_piece_on_map(t_app *app, int y1, int x1, int y2, int x2, t_color c)
{
    app->sim_map[y1][x1] = c.cA;
    app->sim_map[y2][x2] = c.cB;
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
            s->y2 = m[x] - 1;
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

void    ft_set_node(t_app *app, int col, int rot, int deep, int score)
{
    app->n.col = col;
    app->n.rot = rot;
    app->n.deep = deep;
    app->n.score = score;
}

int     ft_contaminate2(char **m, int y, int x, char c)
{
    if (y < 0 || y > 11 || x < 0 || x > 5)
        return 0;
    //fprintf(stderr, "y %d x %d c %c mc %c\n", y, x, c, m[y][x]);
    if (m[y][x] == '0')
    {
        m[y][x] = '.';
    }
    if (m[y][x] == c)
    {
        m[y][x] = '.';
        return (1
                + ft_contaminate2(m, y - 1, x, c)
                + ft_contaminate2(m, y + 1, x, c)
                + ft_contaminate2(m, y, x - 1, c)
                + ft_contaminate2(m, y, x + 1, c));
    }
    return 0;
}

void    ft_run_calcul(t_app *app)
{
    ft_cpy_map(app->sc.score_map, app->sim_map);
    bzero(app->sc.nb_color, sizeof(app->sc.nb_color));

    int total_block = 0;
    int gb = 0;
    for (int y = 0; y < 12; y++) 
    { 
        for (int x = 0; x < 6; x++)
        {
            if (app->sc.score_map[y][x] != '.' && app->sc.score_map[y][x] != '0')
            {
                int size_block = ft_contaminate(app->sc.score_map, y, x, app->sc.score_map[y][x]);
                if (size_block > 3)
                {
                    //app->sc.nb_color[app->sim_map[y][x] - '0' - 1] = 1;
                    ft_cpy_map(app->sc.score_map, app->sim_map);
    
                        
                    
                    ft_contaminate2(app->sc.score_map, y, x, app->sc.score_map[y][x]);

                    ft_cpy_map(app->sim_map, app->sc.score_map);
                    ft_print_map(app->sim_map);
                    total_block += size_block;
                    if (size_block < 5)
                        gb += 0;
                    else
                        gb += (size_block - 4);
                    //ft_count_cp(app, size_block);
                    //fprintf(stderr, "size_block : %d\n", size_block);
                    //return;
                }
            }
        }
    }

    if (total_block > 3)
    {
        ft_count_cp(app, total_block, gb);
        //fprintf(stderr, "Total block : %d\n", total_block);
        //ft_print_map(app->map);
        //ft_print_map(app->sc.score_map);
    }
    //ft_print_map(app->sc.score_map);
}

void    ft_final_calcul(t_app *app)
{
    for (int i = 0; i < 5; i++)
    {
        if (app->sc.nb_color[i] == 1)
            app->calcul_score.cb++;
    }
    app->calcul_score.cb = 0;
    ft_calcul_cp(app);
    app->calcul_score.c = app->calcul_score.cp + app->calcul_score.cb + app->calcul_score.gb;
    if (app->calcul_score.c < 1)
        app->calcul_score.c = 1;
    else if (app->calcul_score.c > 999)
        app->calcul_score.c = 999;
    app->calcul_score.current_score += (10 * app->calcul_score.b) * app->calcul_score.c;
    if (app->calcul_score.current_score >= 900)
    {
        fprintf(stderr, "B : %d | CP : %d | CB : %d | GB : %d | C : %d\n",
                app->calcul_score.b, app->calcul_score.cp, app->calcul_score.cb,
                app->calcul_score.gb, app->calcul_score.c);
        fprintf(stderr, "________Current Score : %d\n", app->calcul_score.current_score);
        //ft_print_map(app->sim_map);
    }
}

void    ft_count_cp(t_app *app, int size_block, int gb)
{
    app->calcul_score.tmp_cp++;
    app->calcul_score.b = size_block;
    app->calcul_score.gb = gb;

    ft_final_calcul(app);

    //if (size_block > 8)
    //    ft_print_map(app->sc.score_map);
    //if (app->calcul_score.current_score >= 900)
    //    ft_print_map(app->sc.score_map);
    ft_clean_map(app);
    //if (app->calcul_score.current_score >= 900)
    //    ft_print_map(app->sc.clean_map);
    ft_cpy_map(app->sim_map, app->sc.clean_map);
    //ft_print_map(app->sc.score_map);
    //ft_print_map(app->sc.clean_map);
    //bzero(app->sc.nb_color, sizeof(app->sc.nb_color));
    ft_run_calcul(app);
}

void    ft_calcul_cp(t_app *app)
{
    int     max = app->calcul_score.tmp_cp;

    if (max > 1)
        app->calcul_score.cp = 8;
    else
        app->calcul_score.cp = 0;
    for (int i = 1; i < max; i++)
        app->calcul_score.cp *= 2;
}

int     ft_simulation_score(t_app *app, int deep, t_sim *s)
{
    //if (score_a1 > 3 || score_b1 > 3 || score_a2 > 3 || score_b2 > 3 || score_a2 > 3 || score_b2 > 3)
    //{
        bzero(&app->calcul_score, sizeof(t_fs));
        ft_run_calcul(app);
        if (app->calcul_score.current_score > app->final_score && app->calcul_score.current_score < 3000)
        {
            app->final_score = app->calcul_score.current_score;
            fprintf(stderr, "B : %d | CP : %d | CB : %d | GB : %d | C : %d\n",
                app->calcul_score.b, app->calcul_score.cp, app->calcul_score.cb,
                app->calcul_score.gb, app->calcul_score.c);
            fprintf(stderr, "Current Score : %d\n", app->calcul_score.current_score);
            fprintf(stderr, "1Score : %d Deep : %d\n", app->final_score, deep);
            fprintf(stderr, "y1 : %d x1 : %d y2 : %d x2 : %d rot : %d\n\n", s->y1, s->x1, s->y2, s->x2, s->rot);
            //if (app->calcul_score.current_score > 1500)
            //    ft_print_map(app->sc.score_map);
            app->old_deep == 10;
            return (app->calcul_score.current_score);
        }
        else if (app->calcul_score.current_score == app->final_score && deep < app->old_deep)
        {
            app->final_score = app->calcul_score.current_score;
            fprintf(stderr, "B : %d | CP : %d | CB : %d | GB : %d | C : %d\n",
                app->calcul_score.b, app->calcul_score.cp, app->calcul_score.cb,
                app->calcul_score.gb, app->calcul_score.c);
            fprintf(stderr, "Current Score : %d\n", app->calcul_score.current_score);
            fprintf(stderr, "2Score : %d Deep : %d\n", app->final_score, deep);
            fprintf(stderr, "y1 : %d x1 : %d y2 : %d x2 : %d rot : %d\n\n", s->y1, s->x1, s->y2, s->x2, s->rot);
            //if (app->calcul_score.current_score > 1500)
             //   ft_print_map(app->sc.score_map);
            app->old_deep = deep;
            return (app->calcul_score.current_score);
        }
    //}
    return (-1);
}

void    ft_simulation_test(t_app *app)
{
    double old_score = 0;
    t_sim s;
    int old_min = 0;

    for (int x1 = 0; x1 < 6; x1++)
    {
        for (int r1 = 0; r1 < 4; r1++)
        {
            bzero(&s, sizeof(t_sim));

            ft_cpy_map(app->sim_map, app->map);  
            ft_copy_max(app->sim_max, app->max);

            ft_push_on_map(app, app->colors[0], x1, r1, &s);

            int score_a = ft_contaminate(app->sim_map, s.y1, s.x1, app->colors[0].cA);
            int score_b = ft_contaminate(app->sim_map, s.y2, s.x2, app->colors[0].cB);

            double score = score_a + score_b;
            int min = (s.y1 < s.y2) ? s.y1 : s.y2;

            if (score > old_score && app->map[s.y1][s.x1] == '.' && app->map[s.y2][s.x2] == '.')
            {
                if (score_a < 4 && score_b < 4 && min > old_min)
                {
                    old_min = min;
                    old_score = score;
                    app->rot = r1;
                    app->col = s.x1;
                }
            }
        }
    }
}

void    ft_calcul_score(t_app *app)
{
    ft_cpy_map(app->sim_map, app->map);  
    ft_copy_max(app->sim_max, app->max);

    t_sim s;
    t_sim s1;
    //t_sim s2;

    for (int x1 = 0; x1 < 6; x1++)
    {
        for (int r1 = 0; r1 < 4; r1++)
        {
            for (int x2 = 0; x2 < 6; x2++)
            {
                for (int r2 = 0; r2 < 4; r2++)
                {
                    //for (int x3 = 0; x3 < 6; x3++)
                    //{
                        //for (int r3 = 0; r3 < 4; r3++)
                        //{

                            bzero(&s, sizeof(t_sim));
                            bzero(&s1, sizeof(t_sim));
                            //bzero(&s2, sizeof(t_sim));

                            ft_cpy_map(app->sim_map, app->map);  
                            ft_copy_max(app->sim_max, app->max);

                            ft_push_on_map(app, app->colors[0], x1, r1, &s);
                            app->sim_max[s.x1]--;
                            app->sim_max[s.x2]--;
                            //ft_cpy_map(app->sc.score_map, app->sim_map);
                            //int score_a1 = ft_contaminate(app->sc.score_map, s.y1, s.x1, app->colors[0].cA);
                            //ft_cpy_map(app->sc.score_map, app->sim_map);
                            //int score_b1 = ft_contaminate(app->sc.score_map, s.y2, s.x2, app->colors[0].cB);

                            if (ft_simulation_score(app, 1, &s) != -1)
                            {
                                ft_set_node(app, s.x1, s.rot, 1, app->final_score);
                            }

                            ft_push_on_map(app, app->colors[1], x2, r2, &s1);
                            app->sim_max[s1.x1]--;
                            app->sim_max[s1.x2]--;
                            //ft_cpy_map(app->sc.score_map, app->sim_map);
                            //int score_a2 = ft_contaminate(app->sc.score_map, s1.y1, s1.x1, app->colors[1].cA);
                            //ft_cpy_map(app->sc.score_map, app->sim_map);
                            //int score_b2 = ft_contaminate(app->sc.score_map, s1.y2, s1.x2, app->colors[1].cB);

                            if (ft_simulation_score(app, 2, &s1) != -1)
                            {
                                ft_set_node(app, s.x1, s.rot, 2, app->final_score);
                            }

                            /*ft_push_on_map(app, app->colors[2], x3, r3, &s2);
                            app->sim_max[s2.x1]--;
                            app->sim_max[s2.x2]--;
                            //ft_cpy_map(app->sc.score_map, app->sim_map);
                            //int score_a2 = ft_contaminate(app->sc.score_map, s1.y1, s1.x1, app->colors[1].cA);
                            //ft_cpy_map(app->sc.score_map, app->sim_map);
                            //int score_b2 = ft_contaminate(app->sc.score_map, s1.y2, s1.x2, app->colors[1].cB);

                            if (ft_simulation_score(app, 3, &s2) != -1)
                            {
                                ft_set_node(app, s.x1, s.rot, 3, app->final_score);
                            }*/
                        //}
                    //}
                }
            }
        }
    }
}

void    ft_init(t_app *app)
{
    app->col = -1;
    app->rot = -1;
    app->smash = 0;
    app->final_score = 100;
    app->old_deep = 10;

    app->sim_map = (char**)malloc(12 * sizeof(char*));
    //ft_lstdel(&app->lst);
    bzero(&app->n, sizeof(t_node));
    for (int i = 0; i < 12; i++)
    {
        app->sim_map[i] = (char*)malloc(6);
    }
    app->sc.score_map = (char**)malloc(12 * sizeof(char*));
    for (int i = 0; i < 12; i++)
    {
        app->sc.score_map[i] = (char*)malloc(6);
    }

    app->sc.clean_map = (char**)malloc(12 * sizeof(char*));
    for (int i = 0; i < 12; i++)
    {
        app->sc.clean_map[i] = (char*)malloc(6);
    }

    app->sc.score_max = (int*)malloc(6 * sizeof(int));
    app->sim_max = (int*)malloc(6 * sizeof(int));
    app->sc.clean_max = (int*)malloc(6 * sizeof(int));
    ft_set_max(app);
}

void    ft_clean_zero(t_app *app)
{
    int old_score = 0;
    t_sim s;

    for (int x1 = 0; x1 < 6; x1++)
    {
        for (int r1 = 0; r1 < 4; r1++)
        {
            bzero(&s, sizeof(t_sim));

            ft_cpy_map(app->sim_map, app->map);  
            ft_copy_max(app->sim_max, app->max);

            ft_push_on_map(app, app->colors[0], x1, r1, &s);

            int score_a = ft_contaminate(app->sim_map, s.y1, s.x1, app->colors[0].cA);
            int score_b = ft_contaminate(app->sim_map, s.y2, s.x2, app->colors[0].cB);

            int score = (score_a > score_b) ? score_a : score_b;

            if (score > old_score && app->map[s.y1][s.x1] == '.' && app->map[s.y2][s.x2] == '.')
            {
                old_score = score;
                app->rot = r1;
                app->col = s.x1;
            }
        }
    }
}

int    ft_defense(t_app *app)
{
    int     nb_zero = 0;;

    for (int y = 0; y < 12; y++) 
    {
        for (int x = 0; x < 6; x++)
        {
            if (app->map[y][x] == '0')
                nb_zero++;
        }
    }
    if (nb_zero > 10)
    {
        fprintf(stderr, "nb_zero : %d\n", nb_zero);
        ft_clean_zero(app);
        return (1);
    }
    return (0);
}

void    ft_run(t_app *app)
{
    //if (ft_defense(app) == 1)
    //    return;
    ft_calcul_score(app);


    if (app->n.score > 0)
    {
        fprintf(stderr, "Col : %d | Rot : %d | Deep : %d | Score : %d\n", app->n.col, app->n.rot, app->n.deep, app->n.score);
        app->col = app->n.col;
        app->rot = app->n.rot;
    }
    if (app->final_score == 0 || ft_is_authorize(app, app->rot, app->col) == -1)
    {
        fprintf(stderr, "HERE\n");
        ft_cpy_map(app->sim_map, app->map); 
        ft_copy_max(app->sim_max, app->max);
        //ft_print_map(app->sim_map);
        ft_simulation_test(app);
    }
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
            char row[7];
            scanf("%s", row);
        }

        ft_init(&app);
        ft_run(&app);

        fprintf(stderr, "\n%f\n", ft_timer(begin));
        printf("%d %d\n", app.col, app.rot);
    }

    return 0;
}