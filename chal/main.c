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
    int             tmp_gb;
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
    if (its_time > 2)
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

void    ft_count_cp(t_app *app)
{
    ft_clean_map(app);
    ft_cpy_map(app->sim_map, app->sc.clean_map);
    //ft_print_map(app->sc.score_map);
    //ft_print_map(app->sc.clean_map);
    bzero(app->sc.nb_color, sizeof(app->sc.nb_color));
    ft_run_calcul(app);
}

void    ft_calcul_cp(t_app *app)
{
    int     max = app->calcul_score.tmp_cp;

    if (max > 1)
        app->calcul_score.cp = 8;
    else
        app->calcul_score.cp = 0;
    for (int i = 2; i < max; i++)
        app->calcul_score.cp *= 2;
}

void    ft_calcul_gb(t_app *app)
{
    if (app->calcul_score.tmp_gb <= 4)
        app->calcul_score.gb = 0;
}

void    ft_final_calcul(t_app *app)
{
    for (int i = 0; i < 5; i++)
    {
        if (app->sc.nb_color[i] == 1)
            app->calcul_score.cb++;
    }
    app->calcul_score.cb = 1;
    ft_calcul_gb(app);
    ft_calcul_cp(app);
    app->calcul_score.c = app->calcul_score.cp + app->calcul_score.cb + app->calcul_score.gb;
    app->calcul_score.current_score = (10 * app->calcul_score.b) * app->calcul_score.c;
    //fprintf(stderr, "B : %d | CP : %d | CB : %d | GB : %d | C : %d\n",
        //app->calcul_score.b, app->calcul_score.cp, app->calcul_score.cb,
        //app->calcul_score.gb, app->calcul_score.c);
    //fprintf(stderr, "Current Score : %d\n", app->calcul_score.current_score);
}

void    ft_run_calcul(t_app *app)
{
    ft_cpy_map(app->sc.score_map, app->sim_map);
    bzero(app->sc.nb_color, sizeof(app->sc.nb_color));
    for (int y = 0; y < 12; y++) 
    { 
        for (int x = 0; x < 6; x++)
        {
            if (app->sc.score_map[y][x] != '.' && app->sc.score_map[y][x] != '0')
            {
                int size_block = ft_contaminate(app->sc.score_map, y, x, app->sc.score_map[y][x]);
                if (size_block > 3)
                {
                    app->calcul_score.b = size_block;
                    app->calcul_score.tmp_gb = size_block;
                    app->calcul_score.tmp_cp++;
                    app->sc.nb_color[app->sim_map[y][x] - '0' - 1] = 1;
                    ft_final_calcul(app);
                    ft_cpy_map(app->sc.score_map, app->sim_map);
                    ft_contaminate(app->sc.score_map, y, x, app->sc.score_map[y][x]);
                    ft_count_cp(app);
                    //fprintf(stderr, "size_block : %d\n", size_block);
                    return;
                }
            }
        }
    }
    //ft_print_map(app->sc.score_map);
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

t_list  *ft_lstnew(void const *content, size_t content_size)
{
    t_list  *list;

    if ((list = (t_list*)malloc(sizeof(t_list))) == NULL)
        return (NULL);
    if (!content)
    {
        list->content = NULL;
        list->content_size = 0;
    }
    else
    {
        if ((list->content = malloc(content_size)) == NULL)
        {
            free(list);
            return (NULL);
        }
        memcpy(list->content, content, content_size);
        list->content_size = content_size;
    }
    list->next = NULL;
    return (list);
}

void    ft_lstpush_back(t_list **l, void const *c, size_t s)
{
    t_list  *list;

    list = *l;
    if (list)
    {
        while (list->next)
            list = list->next;
        list->next = ft_lstnew(c, s);
    }
    else
        *l = ft_lstnew(c, s);
}

void    ft_lstprint(t_list *lst, void (*print)(void *))
{
    while (lst)
    {
        fprintf(stderr, "%s", "content : [");
        if (print)
            (*print)(lst->content);
        else
            fprintf(stderr, "%s", lst->content);
        fprintf(stderr, "%s", "]\tsize : ");
        fprintf(stderr, "%d", lst->content_size);
        fprintf(stderr, "%s", "]\n");
        lst = lst->next;
    }
}

void    ft_print_node(void *content)
{
    t_node  *n;

    n = (t_node*)content;
    fprintf(stderr, "Col : %d | ", n->col);
    fprintf(stderr, "Rot : %d | ", n->rot);
    fprintf(stderr, "Deep : %d | ", n->deep);
    fprintf(stderr, "Score : %d", n->score);
}

int     ft_simulation_score(t_app *app, int x, int r, int deep)
{
    //if (score_a1 > 3 || score_b1 > 3 || score_a2 > 3 || score_b2 > 3 || score_a2 > 3 || score_b2 > 3)
    //{
        bzero(&app->calcul_score, sizeof(t_fs));
        ft_run_calcul(app);
        if (app->calcul_score.current_score > app->final_score)
        {
            app->final_score = app->calcul_score.current_score;
            fprintf(stderr, "Score : %d\n", app->final_score);
            //fprintf(stderr, "y1 : %d x1 : %d y2 : %d x2 : %d rot : %d\n", s.y1, s.x1, s.y2, s.x2, r1);
            app->lst = NULL;
            return (app->calcul_score.current_score);
        }
    //}
    return (-1);
}

void    ft_set_node(t_app *app, int col, int rot, int deep, int score)
{
    t_node  *n;

    n = NULL;

    n = (t_node*)malloc(sizeof(t_node));
    n->col = col;
    n->rot = rot;
    n->deep = deep;
    n->score = score;
    ft_lstpush_back(&app->lst, (void*)n, sizeof(t_node));
}

void    ft_calcul_score(t_app *app)
{
    ft_cpy_map(app->sim_map, app->map);  
    ft_copy_max(app->sim_max, app->max);
    //ft_print_map(app->sim_map);
    //ft_print_max(app->sim_max);
    int old_score = 0;
    int count = 0;
    t_sim s;
    t_sim s1;
    t_sim s2;
    int node_score;

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
                            bzero(&s, sizeof(t_sim));
                            bzero(&s1, sizeof(t_sim));
                            bzero(&s2, sizeof(t_sim));

                            ft_cpy_map(app->sim_map, app->map);  
                            ft_copy_max(app->sim_max, app->max);

                            ft_push_on_map(app, app->colors[0], x1, r1, &s);
                            app->sim_max[s.x1]--;
                            app->sim_max[s.x2]--;

                            //ft_print_map(app->sim_map);

                            ft_cpy_map(app->sc.score_map, app->sim_map);
                            int score_a1 = ft_contaminate(app->sc.score_map, s.y1, s.x1, app->colors[0].cA);
                            ft_cpy_map(app->sc.score_map, app->sim_map);
                            int score_b1 = ft_contaminate(app->sc.score_map, s.y2, s.x2, app->colors[0].cB);

                            if ((node_score = ft_simulation_score(app, s.x1, s.rot, 1)) != -1)
                            {
                                ft_set_node(app, s.x1, s.rot, 1, node_score);
                            }

                            if (score_a1 < 4 && score_b1 < 4)
                            {
                                ft_push_on_map(app, app->colors[1], x2, r2, &s1);
                                app->sim_max[s1.x1]--;
                                app->sim_max[s1.x2]--;
                                ft_cpy_map(app->sc.score_map, app->sim_map);
                                int score_a2 = ft_contaminate(app->sc.score_map, s1.y1, s1.x1, app->colors[1].cA);
                                ft_cpy_map(app->sc.score_map, app->sim_map);
                                int score_b2 = ft_contaminate(app->sc.score_map, s1.y2, s1.x2, app->colors[1].cB);

                                
                                if ((node_score = ft_simulation_score(app, s.x1, s.rot, 1)) != -1)
                                {
                                    ft_set_node(app, s.x1, s.rot, 2, node_score);
                                    ft_set_node(app, s1.x1, s1.rot, 2, node_score);
                                }

                                if (score_a2 < 4 && score_b2 < 4)
                                {
                                    ft_push_on_map(app, app->colors[2], x3, r3, &s2);
                                    app->sim_max[s2.x1]--;
                                    app->sim_max[s2.x2]--;
                                    ft_cpy_map(app->sc.score_map, app->sim_map);
                                    int score_a3 = ft_contaminate(app->sc.score_map, s2.y1, s2.x1, app->colors[2].cA);
                                    ft_cpy_map(app->sc.score_map, app->sim_map);
                                    int score_b3 = ft_contaminate(app->sc.score_map, s2.y2, s2.x2, app->colors[2].cB);

                                    if ((node_score = ft_simulation_score(app, s.x1, s.rot, 1)) != -1)
                                    {
                                        ft_set_node(app, s.x1, s.rot, 3, node_score);
                                        ft_set_node(app, s1.x1, s1.rot, 3, node_score);
                                        ft_set_node(app, s2.x1, s2.rot, 3, node_score);
                                    }
                                }
                            }

                            //ft_print_map(app->sim_map);
                            //fprintf(stderr, "score_a : %d score_b : %d\n", score_a, score_b);
                            //fprintf(stderr, "s.y1 : %d s.x1 : %d s.y2 : %d s.x2 : %d\n", s.y1, s.x1, s.y2, s.x2);
                            

                            //fprintf(stderr, "score : %d\n", score);
                            //ft_print_max(app->sim_max);
                            count++;
                        }
                    }
                }
            }
        }
    }
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
            //ft_print_max(app->sim_max);
            //fprintf(stderr, "s.y1 : %d s.x1 : %d s.y2 : %d s.x2 : %d s.rot : %d score : %d\n", s.y1, s.x1, s.y2, s.x2, s.rot, score);
            if (score > old_score && app->map[s.y1][s.x1] == '.' && app->map[s.y2][s.x2] == '.')
            {
                if (score < 4)
                {
                    fprintf(stderr, "s.y1 : %d s.x1 : %d s.y2 : %d s.x2 : %d s.rot : %d score : %d\n", s.y1, s.x1, s.y2, s.x2, s.rot, score);
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
    ft_calcul_score(app);

    if (app->final_score == 0)
    {
        //fprintf(stderr, "HERE\n");
        ft_cpy_map(app->sim_map, app->map); 
        ft_copy_max(app->sim_max, app->max);
        //ft_print_map(app->sim_map);
        ft_simulation_test(app);
    }
    else
    {
        t_list *l;

        l = app->lst;
        t_node  *n;

        n = (t_node*)l->content;

        while (l)
        {
            ft_lstprint(l, ft_print_node);
            l = l->next;
        }

        printf("%d %d\n", n->col, n->rot);
    }
}

int     main()
{
    t_app   app;
    bzero(&app, sizeof(t_app));

    while (1) 
    {
        //app.lst = NULL;
        clock_t begin = clock();
        //app.final_score = 0;
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