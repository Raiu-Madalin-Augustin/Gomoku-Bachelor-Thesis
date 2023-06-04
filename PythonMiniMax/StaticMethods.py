def CreateBoard(arguments, player, size):
    board = [[0 for c in range(size)] for r in range(size)]
    arg = arguments.split("|")
    for x in range(len(arg)):
        arg[x] = arg[x].replace("[(", "")
        arg[x] = arg[x].replace(")", "")
        arg[x] = arg[x].replace("]", "")
        values = arg[x].split(",")
        piece = int(values[2])
        if piece == 1:
            piece = -1
        elif piece == 2:
            piece = 1
        board[int(values[0])][int(values[1])] = piece
    return board
