#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>

typedef struct 		s_node
{
	int				value;
	struct s_node	*child[22];
}					t_node;

typedef struct 		s_app
{
	t_node			*root;
	int				count;
}					t_app;

t_node	*new_node(int value)
{
	t_node	*node;
	int		i;

	i = 0;
	node = (t_node*)malloc(sizeof(t_node));
	node->value = value;

	while (i < 22)
	{
		node->child[i] = NULL;
		i++;
	}
	return (node);
}

void	ft_create_tree(int val, t_node *n, int deep, t_app *app)
{
	int		i;

	i = 0;
	app->count++;
	if (deep > 4)
		return;
	while (i < 24)
	{
		n->child[i] = new_node(val);
		ft_create_tree(val + 1, n->child[i], deep + 1, app);
		i++;
	}
	return;
}


int		main(void)
{
	t_app app;

	bzero(&app, sizeof(t_app));
	app.root = new_node(0);
	ft_create_tree(1, app.root, 0, &app);
	printf("%d\n", app.count);
	return (0);
}