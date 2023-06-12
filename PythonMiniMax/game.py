import StaticMethods
from ai import get_best_move
from board import BoardState


class Game:
    def __init__(self, player, size=19, depth=2, arguments=""):
        self.state = None
        self.size = size
        self.player = player
        self.arguments = arguments
        self.depth = depth
        self.finished = False
        self.restart()

    def restart(self, player_index=-1):
        self.is_max_state = True if player_index == -1 else False
        self.state = BoardState(15, StaticMethods.CreateBoard(self.arguments, self.player, self.size))
        self.ai_color = -player_index

    def aiplay(self):
        import time
        t = time.time()
        if self.state.color == self.ai_color:
            return False, (0, 0)
        move, value = get_best_move(self.state, self.depth, self.is_max_state)
        self.state = self.state.next(move)
        self.finished = self.state.is_terminal()
        # print(time.time() - t)
        return move
