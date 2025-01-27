#include <stdio.h>
#include <stdbool.h>

typedef struct {
  // column, row
  char board[7][6];
} Board;

int makeMove(bool turn);
void printBoard(Board board);
Board updateBoard(Board board, int move, bool turn);
Board checkAndRemoveRows(Board *board, bool turn, int *p1, int *p2);
bool checkWin(Board board, bool turn);

int main() {
  int p1 = 0;
  int p2 = 0;
  bool turn = false;
  Board board;

  for (int i = 0; i < 7; i++) {
    for (int j = 0; j < 6; j++) {
      board.board[i][j] = '0';
    }
  }

  do {
    turn = !turn;
    printBoard(board);
    printf("Player 1 Points: %d | Player 2 Points: %d\n", p1, p2);

    int move = makeMove(turn);
    board = updateBoard(board, move, turn);

    // Check for win and update points
    board = checkAndRemoveRows(&board, turn, &p1, &p2);

  } while (1);
}

void printBoard(Board board) {
  printf("0123456\n");
  printf("-----------------\n");

  for (int row = 0; row < 6; row++) {
    for (int column = 0; column < 7; column++) {
      printf("%c", board.board[column][row]);
    }
    printf("%c", '\n');
  }
  printf("-----------------\n");
}

Board updateBoard(Board board, int move, bool turn) {
  for (int row = 5; row >= 0; row--) {
    if (board.board[move][row] == '0') {
      board.board[move][row] = turn ? 'X' : '+';
      break;
    }
  }
  return board;
}

int makeMove(bool turn) {
  int input;

  if (turn) {
    printf("Player 1 \n");
  } else {
    printf("Player 2  \n");
  }

  printf("Make a move: ");
  scanf("%d", &input);

  if (input < 0 || input > 6) {
    printf("Column out of range. Please choose between 0 and 6.\n");
    return makeMove(turn);
  }

  return input;
}

Board checkAndRemoveRows(Board *board, bool turn, int *p1, int *p2) {
  char symbol = turn ? 'X' : '+';

  for (int row = 0; row < 6; row++) {
    int count = 0;
    for (int col = 0; col < 7; col++) {
      if (board->board[col][row] == symbol) {
        count++;
        if (count == 4) {
          if (turn) {
            (*p1)++;
          } else {
            (*p2)++;
          }

          for (int r = row; r > 0; r--) {
            for (int c = 0; c < 7; c++) {
              board->board[c][r] = board->board[c][r - 1];
            }
          }

          for (int c = 0; c < 7; c++) {
            board->board[c][0] = '0';
          }

          return *board;
        }
      } else {
        count = 0;
      }
    }
  }
  return *board;
}
