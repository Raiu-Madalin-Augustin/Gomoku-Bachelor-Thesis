import sys

from game import Game

if __name__ == '__main__':
    args: str = sys.argv[1]
    args = args[1:]
    # print(args)
    player = sys.argv[1][0]
    game_runner = Game(player, 15, 1, args)
    x, y = game_runner.aiplay()
    print(x, y)
